# SimpleResults.Functional

[![NuGet](https://img.shields.io/nuget/v/SoftwareMadeSimple.SimpleResults.Functional)](https://www.nuget.org/packages/SoftwareMadeSimple.SimpleResults.Functional)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Functional extensions for [SoftwareMadeSimple.SimpleResults](https://github.com/ReneWiersma/SimpleResults), providing `Map`, `Bind`, `MapError`, `BindError`, `Match`, `Unit`, and applicative `Lift`/`Apply` with error accumulation.

## Installation

```
dotnet add package SoftwareMadeSimple.SimpleResults.Functional
```

## Usage

### Map

Transform the success value of a `Result` without unwrapping it. If the result is a failure, the error is passed through unchanged.

```csharp
Result<int, string> result = 5;

Result<string, string> mapped = result.Map(x => x.ToString());
// mapped.Value == "5"
```

`Map` calls can be chained:

```csharp
Result<int, string> result = 3;

var mapped = result
    .Map(x => x + 1)
    .Map(x => x * 10);
// mapped.Value == 40
```

### Bind

Chain operations that themselves return a `Result`. Unlike `Map`, the function passed to `Bind` returns a `Result`, which avoids double-wrapping. This short-circuits on the first failure.

```csharp
Result<int, string> Validate(int x) =>
    x > 0 ? x : "must be positive";

Result<int, string> result = 5;

Result<int, string> bound = result.Bind(Validate);
// bound.Value == 5
```

Chain multiple operations that can each independently fail:

```csharp
Result<int, string> Parse(string s) =>
    int.TryParse(s, out var n) ? n : "not a number";

Result<int, string> Validate(int x) =>
    x > 0 ? x : "must be positive";

Result<string, string> input = "42";

var bound = input
    .Bind(Parse)
    .Bind(Validate);
// bound.Value == 42
```

### MapError

Transform the error value of a `Result`, leaving a success unchanged. Useful for converting between error types at layer boundaries.

```csharp
Result<int, string> result = "not found";

Result<int, int> mapped = result.MapError(e => e.Length);
// mapped.Error == 9
```

### BindError

Recover from errors by applying a function that returns a new `Result`. Enables fallback strategies.

```csharp
Result<int, string> TryCache(string error) =>
    error == "not found" ? 42 : error;

Result<int, string> result = "not found";

Result<int, string> recovered = result.BindError(TryCache);
// recovered.Value == 42
```

Chain multiple recovery attempts:

```csharp
Result<int, string> FirstFallback(string _) => "first fallback failed";
Result<int, string> SecondFallback(string _) => 99;

Result<int, string> result = "original error";

var recovered = result
    .BindError(FirstFallback)
    .BindError(SecondFallback);
// recovered.Value == 99
```

### Match

Collapse a `Result` into a single value by providing a function for each case. This is the standard way to exit the `Result` type.

```csharp
Result<int, string> result = 42;

string message = result.Match(
    value => $"Got {value}",
    error => $"Failed: {error}");
// message == "Got 42"
```

`Match` is the natural endpoint of a functional pipeline:

```csharp
Result<int, string> Validate(int x) =>
    x > 0 ? x : "must be positive";

Result<int, string> result = 5;

string output = result
    .Bind(Validate)
    .Map(x => x * 2)
    .Match(
        value => $"Result: {value}",
        error => $"Error: {error}");
// output == "Result: 10"
```

An `Action`-based overload is also available for performing side effects without returning a value:

```csharp
Result<int, string> result = 42;

result.Match(
    value => Console.WriteLine($"Got {value}"),
    error => Console.WriteLine($"Failed: {error}"));
// prints "Got 42"
```

### Unit

A type that represents the absence of a value, used in place of `void` since `void` cannot be a generic type parameter. This enables `Result<Unit, E>` for operations that either succeed with no meaningful value or fail.

```csharp
Result<Unit, string> SaveToDatabase(Person person)
{
    // save logic
    return Unit.Default;
}

Result<Unit, string> result = SaveToDatabase(person);

string message = result.Match(
    _ => "Saved successfully",
    error => $"Failed: {error}");
```

### Lift & Apply

Use `Lift` to wrap a function into a `Result`, then chain `Apply` calls to feed in each validated argument. Unlike `Bind` (which short-circuits on the first error), `Apply` runs every validation and collects **all** failures.

```csharp
record Person(string Name, int Age);

Result<string, ValidationErrors> nameResult = ValidateName(input.Name);
Result<int, ValidationErrors> ageResult = ValidateAge(input.Age);

var result =
    LiftResult<ValidationErrors>
        .Lift((string name, int age) => new Person(name, age))
        .Apply(nameResult)
        .Apply(ageResult);

if (result.IsSuccess)
{
    Person person = result.Value;
}
else
{
    IEnumerable<ValidationError> errors = result.Error;
    // all validation errors are collected here
}
```

`Lift` supports functions with up to **16 parameters**. Each additional parameter is applied with another `.Apply()` call:

```csharp
record Address(string Street, string City, string Zip);

var result =
    LiftResult<ValidationErrors>
        .Lift((string street, string city, string zip) => new Address(street, city, zip))
        .Apply(streetResult)
        .Apply(cityResult)
        .Apply(zipResult);
```

## API Summary

| Method | Signature | Description |
|---|---|---|
| `Map` | `Result<T, E> ? Func<T, R> ? Result<R, E>` | Transform the success value |
| `MapError` | `Result<T, E> ? Func<E, F> ? Result<T, F>` | Transform the error value |
| `Bind` | `Result<T, E> ? Func<T, Result<R, E>> ? Result<R, E>` | Chain failable operations |
| `BindError` | `Result<T, E> ? Func<E, Result<T, F>> ? Result<T, F>` | Recover from errors |
| `Match` | `Result<T, E> ? Func<T, R> ? Func<E, R> ? R` | Eliminate the Result type |
| `Match` | `Result<T, E> ? Action<T> ? Action<E> ? void` | Perform side effects per case |
| `Lift` | `Func<T1, ..., R> ? Result<Func<...>, E>` | Lift a function into Result |
| `Apply` | `Result<Func<T, R>, E> ? Result<T, E> ? Result<R, E>` | Apply with error accumulation |
| `Unit` | — | Value type representing void |

## Requirements

- .NET 10+
- [SoftwareMadeSimple.SimpleResults](https://www.nuget.org/packages/SoftwareMadeSimple.SimpleResults) 1.1.0+

## License

[MIT](LICENSE)

# SimpleResults.Functional

[![NuGet](https://img.shields.io/nuget/v/SoftwareMadeSimple.SimpleResults.Functional)](https://www.nuget.org/packages/SoftwareMadeSimple.SimpleResults.Functional)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Functional applicative extensions for [SoftwareMadeSimple.SimpleResults](https://github.com/ReneWiersma/SimpleResults), providing `Lift` and `Apply` with error accumulation.

## Installation

```
dotnet add package SoftwareMadeSimple.SimpleResults.Functional
```

## Why?

When validating multiple fields independently, you typically want to collect **all** errors rather than short-circuiting on the first failure. The applicative pattern enables exactly this — validate each field separately, then combine the results while accumulating every error.

## Usage

### Lift & Apply

Use `Lift` to wrap a function into a `Result`, then chain `Apply` calls to feed in each validated argument. If any argument fails, all errors are accumulated.

```csharp
using SoftwareMadeSimple.SimpleResults;
using SoftwareMadeSimple.SimpleResults.Functional;

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
    // use person
}
else
{
    IEnumerable<ValidationError> errors = result.Error;
    // all validation errors are collected here
}
```

### Error accumulation

Unlike monadic `Bind` (which short-circuits on the first error), `Apply` runs every validation and collects all failures:

```csharp
Result<string, ValidationErrors> nameResult = new List<ValidationError>
{
    new("Name is required"),
    new("Name must be at least 2 characters")
};

Result<int, ValidationErrors> ageResult = new List<ValidationError>
{
    new("Age must be positive")
};

var result =
    LiftResult<ValidationErrors>
        .Lift((string name, int age) => new Person(name, age))
        .Apply(nameResult)
        .Apply(ageResult);

// result.Error contains all 3 validation errors
```

### Functions with more parameters

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

## Requirements

- .NET 10+
- [SoftwareMadeSimple.SimpleResults](https://www.nuget.org/packages/SoftwareMadeSimple.SimpleResults) 1.1.0+

## License

[MIT](LICENSE)

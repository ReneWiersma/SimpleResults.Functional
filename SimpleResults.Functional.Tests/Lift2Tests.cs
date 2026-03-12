using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class Lift2Tests
{
    private sealed record TestPerson(string Name, int Age);

    [Test]
    public void Success()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;

        var result =
            LiftResult<ValidationErrors>.Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        Assert.That(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(new TestPerson("Alice", 42)));
    }

    [Test]
    public void SingleFailureOnFirst()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = new List<ValidationError> { new("Invalid age") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        Assert.That(result.IsFailure);
        Assert.That(result.Error.Count(), Is.EqualTo(1));
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid age")));
    }
}
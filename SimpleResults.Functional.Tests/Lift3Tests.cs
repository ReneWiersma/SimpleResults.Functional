using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class Lift3Tests
{
    private sealed record TestPerson(string Name, int Age, string ThirdValue);

    [Test]
    public void Success()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = "Another value";

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third) => new TestPerson(name, age, third))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult);

        Assert.That(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(new TestPerson("Alice", 42, "Another value")));
    }

    [Test]
    public void SingleFailureOnThird()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = new List<ValidationError> { new("Invalid value") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third) => new TestPerson(name, age, third))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult);

        Assert.That(result.IsFailure);
        Assert.That(result.Error.Count(), Is.EqualTo(1));
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid value")));
    }
}
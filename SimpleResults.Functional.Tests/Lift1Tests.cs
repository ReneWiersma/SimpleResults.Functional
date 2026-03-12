using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class Lift1Tests
{
    private sealed record TestPerson(string Name);

    [Test]
    public void Success()
    {
        Result<string, ValidationErrors> nameResult = "Alice";

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name) => new TestPerson(name))
                .Apply(nameResult);

        Assert.That(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(new TestPerson("Alice")));
    }


    [Test]
    public void SingleFailureOnFirst()
    {
        Result<string, ValidationErrors> nameResult = new List<ValidationError> { new("Invalid name") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name) => new TestPerson(name))
                .Apply(nameResult);

        Assert.That(result.IsFailure);
        Assert.That(result.Error.Count(), Is.EqualTo(1));
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid name")));
    }
}
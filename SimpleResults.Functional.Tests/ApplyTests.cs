using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class ApplyTests
{
    private sealed record TestPerson(string Name, int Age);

    [Test]
    public void SingleFailureOnFirst()
    {
        Result<string, ValidationErrors> nameResult = new List<ValidationError> { new("Invalid name") };
        Result<int, ValidationErrors> ageResult = 42;

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(1));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid name")));
        }
    }

    [Test]
    public void MultipleFailuresOnFirst()
    {
        Result<string, ValidationErrors> nameResult = new List<ValidationError> { new("Invalid name 1"), new("Invalid name 2") };
        Result<int, ValidationErrors> ageResult = 42;

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(2));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid name 1")));
        }
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid name 2")));
    }

    [Test]
    public void SingleFailureOnSecond()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = new List<ValidationError> { new("Invalid age") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(1));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid age")));
        }
    }

    [Test]
    public void MultipleFailuresOnSecond()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = new List<ValidationError> { new("Invalid age 1"), new("Invalid age 2") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(2));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid age 1")));
        }
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid age 2")));
    }

    [Test]
    public void MultipleFailuresOnBoth()
    {
        Result<string, ValidationErrors> nameResult = new List<ValidationError> { new("Invalid name 1"), new("Invalid name 2") };
        Result<int, ValidationErrors> ageResult = new List<ValidationError> { new("Invalid age 1"), new("Invalid age 2") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age) => new TestPerson(name, age))
                .Apply(nameResult)
                .Apply(ageResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(4));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid name 1")));
        }
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid name 2")));
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid age 1")));
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid age 2")));
    }
}
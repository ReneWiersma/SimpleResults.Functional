using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests.LiftTests;

public sealed class Lift5Tests
{
    private sealed record TestPerson(string Name, int Age, string ThirdValue, string FourthValue, string FifthValue);


    [Test]
    public void Success()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = "Third value";
        Result<string, ValidationErrors> fourthResult = "Fourth value";
        Result<string, ValidationErrors> fifthResult = "Fifth value";

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third, string fourth, string fifth) => new TestPerson(name, age, third, fourth, fifth))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult)
                .Apply(fourthResult)
                .Apply(fifthResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.Value, Is.EqualTo(new TestPerson("Alice", 42, "Third value", "Fourth value", "Fifth value")));
        }
    }

    [Test]
    public void SingleFailureOnFifth()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = "Some value";
        Result<string, ValidationErrors> fourthResult = "Fourth value";
        Result<string, ValidationErrors> fifthResult = new List<ValidationError> { new("Invalid value") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third, string fourth, string fifth) => new TestPerson(name, age, third, fourth, fifth))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult)
                .Apply(fourthResult)
                .Apply(fifthResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(1));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid value")));
        }
    }
}
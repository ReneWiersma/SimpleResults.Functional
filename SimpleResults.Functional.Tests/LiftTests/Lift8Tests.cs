using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests.LiftTests;

public sealed class Lift8Tests
{
    private sealed record TestPerson(string Name,int Age, string ThirdValue, string FourthValue, string FifthValue, string SixthValue, string SeventhValue, string EighthValue);

    [Test]
    public void Success()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = "Third value";
        Result<string, ValidationErrors> fourthResult = "Fourth value";
        Result<string, ValidationErrors> fifthResult = "Fifth value";
        Result<string, ValidationErrors> sixthResult = "Sixth value";
        Result<string, ValidationErrors> seventhResult = "Seventh value";
        Result<string, ValidationErrors> eighthResult = "Eighth value";

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third, string fourth, string fifth, string sixth, string seventh, string eighth) =>
                    new TestPerson(name, age, third, fourth, fifth, sixth, seventh, eighth))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult)
                .Apply(fourthResult)
                .Apply(fifthResult)
                .Apply(sixthResult)
                .Apply(seventhResult)
                .Apply(eighthResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.Value, Is.EqualTo(new TestPerson("Alice", 42, "Third value", "Fourth value", "Fifth value", "Sixth value", "Seventh value", "Eighth value")));
        }
    }

    [Test]
    public void SingleFailureOnEighth()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = "Some value";
        Result<string, ValidationErrors> fourthResult = "Fourth value";
        Result<string, ValidationErrors> fifthResult = "Fifth value";
        Result<string, ValidationErrors> sixthResult = "Sixth value";
        Result<string, ValidationErrors> seventhResult = "Seventh value";
        Result<string, ValidationErrors> eighthResult = new List<ValidationError> { new("Invalid value") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third, string fourth, string fifth, string sixth, string seventh, string eighth) =>
                    new TestPerson(name, age, third, fourth, fifth, sixth, seventh, eighth))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult)
                .Apply(fourthResult)
                .Apply(fifthResult)
                .Apply(sixthResult)
                .Apply(seventhResult)
                .Apply(eighthResult);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error.Count(), Is.EqualTo(1));
            Assert.That(result.Error, Does.Contain(new ValidationError("Invalid value")));
        }
    }
}
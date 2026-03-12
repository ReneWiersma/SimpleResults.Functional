using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class Lift15Tests
{
    private sealed record TestPerson(string Name, int Age, string ThirdValue, string FourthValue, string FifthValue, string SixthValue, string SeventhValue, string EighthValue, string NinthValue, string TenthValue, string EleventhValue, string TwelfthValue, string ThirteenthValue, string FourteenthValue, string FifteenthValue);

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
        Result<string, ValidationErrors> ninthResult = "Ninth value";
        Result<string, ValidationErrors> tenthResult = "Tenth value";
        Result<string, ValidationErrors> eleventhResult = "Eleventh value";
        Result<string, ValidationErrors> twelfthResult = "Twelfth value";
        Result<string, ValidationErrors> thirteenthResult = "Thirteenth value";
        Result<string, ValidationErrors> fourteenthResult = "Fourteenth value";
        Result<string, ValidationErrors> fifteenthResult = "Fifteenth value";

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third, string fourth, string fifth, string sixth, string seventh, string eighth, string ninth, string tenth, string eleventh, string twelfth, string thirteenth, string fourteenth, string fifteenth) =>
                    new TestPerson(name, age, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult)
                .Apply(fourthResult)
                .Apply(fifthResult)
                .Apply(sixthResult)
                .Apply(seventhResult)
                .Apply(eighthResult)
                .Apply(ninthResult)
                .Apply(tenthResult)
                .Apply(eleventhResult)
                .Apply(twelfthResult)
                .Apply(thirteenthResult)
                .Apply(fourteenthResult)
                .Apply(fifteenthResult);

        Assert.That(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(new TestPerson("Alice", 42, "Third value", "Fourth value", "Fifth value", "Sixth value", "Seventh value", "Eighth value", "Ninth value", "Tenth value", "Eleventh value", "Twelfth value", "Thirteenth value", "Fourteenth value", "Fifteenth value")));
    }

    [Test]
    public void SingleFailureOnFifteenth()
    {
        Result<string, ValidationErrors> nameResult = "Alice";
        Result<int, ValidationErrors> ageResult = 42;
        Result<string, ValidationErrors> thirdResult = "Some value";
        Result<string, ValidationErrors> fourthResult = "Fourth value";
        Result<string, ValidationErrors> fifthResult = "Fifth value";
        Result<string, ValidationErrors> sixthResult = "Sixth value";
        Result<string, ValidationErrors> seventhResult = "Seventh value";
        Result<string, ValidationErrors> eighthResult = "Eighth value";
        Result<string, ValidationErrors> ninthResult = "Ninth value";
        Result<string, ValidationErrors> tenthResult = "Tenth value";
        Result<string, ValidationErrors> eleventhResult = "Eleventh value";
        Result<string, ValidationErrors> twelfthResult = "Twelfth value";
        Result<string, ValidationErrors> thirteenthResult = "Thirteenth value";
        Result<string, ValidationErrors> fourteenthResult = "Fourteenth value";
        Result<string, ValidationErrors> fifteenthResult = new List<ValidationError> { new("Invalid value") };

        var result =
            LiftResult<ValidationErrors>
                .Lift((string name, int age, string third, string fourth, string fifth, string sixth, string seventh, string eighth, string ninth, string tenth, string eleventh, string twelfth, string thirteenth, string fourteenth, string fifteenth) =>
                    new TestPerson(name, age, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth))
                .Apply(nameResult)
                .Apply(ageResult)
                .Apply(thirdResult)
                .Apply(fourthResult)
                .Apply(fifthResult)
                .Apply(sixthResult)
                .Apply(seventhResult)
                .Apply(eighthResult)
                .Apply(ninthResult)
                .Apply(tenthResult)
                .Apply(eleventhResult)
                .Apply(twelfthResult)
                .Apply(thirteenthResult)
                .Apply(fourteenthResult)
                .Apply(fifteenthResult);

        Assert.That(result.IsFailure);
        Assert.That(result.Error.Count(), Is.EqualTo(1));
        Assert.That(result.Error, Does.Contain(new ValidationError("Invalid value")));
    }
}
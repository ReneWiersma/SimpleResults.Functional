using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class UnitTests
{
    [Test]
    public void AllUnitValuesAreEqual()
    {
        var a = Unit.Default;
        var b = new Unit();

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void EqualityOperatorsReturnTrue()
    {
        var a = Unit.Default;
        var b = new Unit();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(a == b);
            Assert.That(a != b, Is.False);
        }
    }

    [Test]
    public void ToStringReturnsUnitLiteral()
    {
        Assert.That(Unit.Default.ToString(), Is.EqualTo("()"));
    }

    [Test]
    public void GetHashCodeIsConsistent()
    {
        Assert.That(Unit.Default.GetHashCode(), Is.EqualTo(new Unit().GetHashCode()));
    }

    [Test]
    public void UnitCanBeUsedAsSuccessValueInResult()
    {
        Result<Unit, string> result = Unit.Default;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.Value, Is.EqualTo(Unit.Default));
        }
    }

    [Test]
    public void UnitResultCanRepresentAFailedCommand()
    {
        Result<Unit, string> result = "operation failed";

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsFailure);
            Assert.That(result.Error, Is.EqualTo("operation failed"));
        }
    }

    [Test]
    public void UnitResultWorksWithMatch()
    {
        Result<Unit, string> result = Unit.Default;

        var matched = result.Match(
            _ => "success",
            error => $"failed: {error}");

        Assert.That(matched, Is.EqualTo("success"));
    }
}

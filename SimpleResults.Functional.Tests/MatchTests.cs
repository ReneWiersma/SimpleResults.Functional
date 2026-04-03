using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class MatchTests
{
    [Test]
    public void SuccessResultMatchesOnSuccessBranch()
    {
        Result<int, string> result = 42;

        var matched = result.Match(
            value => $"Value: {value}",
            error => $"Error: {error}");

        Assert.That(matched, Is.EqualTo("Value: 42"));
    }

    [Test]
    public void FailureResultMatchesOnFailureBranch()
    {
        Result<int, string> result = "something went wrong";

        var matched = result.Match(
            value => $"Value: {value}",
            error => $"Error: {error}");

        Assert.That(matched, Is.EqualTo("Error: something went wrong"));
    }

    [Test]
    public void MatchCanProduceADifferentType()
    {
        Result<int, string> result = 5;

        var matched = result.Match(
            value => value > 0,
            _ => false);

        Assert.That(matched, Is.True);
    }

    [Test]
    public void SuccessBranchIsNotInvokedOnFailure()
    {
        Result<int, string> result = "error";
        bool wasCalled = false;

        result.Match(
            value =>
            {
                wasCalled = true;
                return value;
            },
            error => error.Length);

        Assert.That(wasCalled, Is.False);
    }

    [Test]
    public void FailureBranchIsNotInvokedOnSuccess()
    {
        Result<int, string> result = 42;
        bool wasCalled = false;

        result.Match(
            value => value,
            error =>
            {
                wasCalled = true;
                return error.Length;
            });

        Assert.That(wasCalled, Is.False);
    }

    [Test]
    public void MatchWithValidationErrorsCollectsMessages()
    {
        Result<string, ValidationErrors> result = new List<ValidationError>
        {
            new("Invalid name"),
            new("Invalid age")
        };

        var matched = result.Match(
            value => value,
            errors => string.Join("; ", errors.Select(e => e.Message)));

        Assert.That(matched, Is.EqualTo("Invalid name; Invalid age"));
    }

    [Test]
    public void MatchComposesWithMapAndBind()
    {
        static Result<int, string> Parse(int x) =>
            x > 0 ? x : "must be positive";

        Result<int, string> result = 123;

        var matched = result
            .Bind(Parse)
            .Map(x => x * 2)
            .Match(
                value => $"Result: {value}",
                error => $"Failed: {error}");

        Assert.That(matched, Is.EqualTo("Result: 246"));
    }

    [Test]
    public void ActionMatchCallsOnSuccessForSuccessResult()
    {
        Result<int, string> result = 42;
        int captured = 0;

        result.Match(
            value => captured = value,
            _ => { });

        Assert.That(captured, Is.EqualTo(42));
    }

    [Test]
    public void ActionMatchCallsOnFailureForFailureResult()
    {
        Result<int, string> result = "something went wrong";
        string captured = "";

        result.Match(
            _ => { },
            error => captured = error);

        Assert.That(captured, Is.EqualTo("something went wrong"));
    }

    [Test]
    public void ActionMatchDoesNotCallOnSuccessForFailureResult()
    {
        Result<int, string> result = "error";
        bool wasCalled = false;

        result.Match(
            _ => wasCalled = true,
            _ => { });

        Assert.That(wasCalled, Is.False);
    }

    [Test]
    public void ActionMatchDoesNotCallOnFailureForSuccessResult()
    {
        Result<int, string> result = 42;
        bool wasCalled = false;

        result.Match(
            _ => { },
            _ => wasCalled = true);

        Assert.That(wasCalled, Is.False);
    }
}

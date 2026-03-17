namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class BindErrorTests
{
    [Test]
    public void FailureErrorIsBoundToRecoveredResult()
    {
        static Result<int, string> Recover(string error) => error == "not found" ? 0 : error;
        Result<int, string> result = "not found";

        var bound = result.BindError(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.Zero);
        }
    }

    [Test]
    public void SuccessResultPreservesOriginalValue()
    {
        static Result<int, string> Recover(string _) => 0;
        Result<int, string> result = 42;

        var bound = result.BindError(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(42));
        }
    }

    [Test]
    public void BindErrorFunctionIsNotInvokedOnSuccess()
    {
        static Result<int, string> Recover(string _) => 0;
        Result<int, string> result = 42;
        bool wasCalled = false;

        var bound = result.BindError(e =>
        {
            wasCalled = true;
            return Recover(e);
        });

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(wasCalled, Is.False);
        }
    }

    [Test]
    public void BindErrorCanTransformTheErrorType()
    {
        static Result<int, long> Recover(string error) => Result<int, long>.Failure(error.Length);
        Result<int, string> result = "not found";

        var bound = result.BindError(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo(9L));
        }
    }

    [Test]
    public void BindErrorRecoveryCanFailWithNewError()
    {
        static Result<int, string> Recover(string _) => "recovery also failed";
        Result<int, string> result = "original error";

        var bound = result.BindError(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("recovery also failed"));
        }
    }

    [Test]
    public void ChainedBindErrorsAttemptMultipleRecoveries()
    {
        static Result<int, string> FirstFallback(string _) => "first fallback failed";
        static Result<int, string> SecondFallback(string _) => 99;
        Result<int, string> result = "original error";

        var bound = result
            .BindError(FirstFallback)
            .BindError(SecondFallback);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(99));
        }
    }
}

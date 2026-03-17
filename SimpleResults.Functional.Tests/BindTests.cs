using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class BindTests
{
    [Test]
    public void SuccessResultIsBoundToTransformedResult()
    {
        static Result<int, string> MultiplyByTwo(int x) => x * 2;
        Result<int, string> result = 5;

        var bound = result.Bind(MultiplyByTwo);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(10));
        }
    }

    [Test]
    public void FailureResultPreservesOriginalError()
    {
        static Result<int, string> MultiplyByTwo(int x) => x * 2;
        Result<int, string> result = "something went wrong";

        var bound = result.Bind(MultiplyByTwo);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("something went wrong"));
        }
    }

    [Test]
    public void BindChangesTheValueType()
    {
        static Result<long, string> ToLong(int x) => x;
        Result<int, string> result = 42;

        var bound = result.Bind(ToLong);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(42L));
        }
    }

    [Test]
    public void BindFunctionIsNotInvokedOnFailure()
    {
        static Result<int, string> MultiplyByTwo(int x) => x * 2;
        Result<int, string> result = "error";
        bool wasCalled = false;

        var bound = result.Bind(x =>
        {
            wasCalled = true;
            return MultiplyByTwo(x);
        });

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(wasCalled, Is.False);
        }
    }

    [Test]
    public void BindFunctionReturningFailurePropagatesError()
    {
        static Result<int, string> Handle(int _) => "validation failed";
        Result<int, string> result = 5;

        var bound = result.Bind(Handle);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("validation failed"));
        }
    }

    [Test]
    public void ChainedBindsComposeCorrectly()
    {
        static Result<int, string> Increment(int x) => x + 1;
        static Result<int, string> MultiplyByTen(int x) => x * 10;
        static Result<long, string> ToLong(int x) => x;
        Result<int, string> result = 3;

        var bound = result
            .Bind(Increment)
            .Bind(MultiplyByTen)
            .Bind(ToLong);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(40L));
        }
    }

    [Test]
    public void ChainedBindsShortCircuitOnFirstFailure()
    {
        static Result<int, string> FailingStep(int _) => "first error";
        static Result<int, string> MultiplyByTen(int x) => x * 10;
        static Result<long, string> ToLong(int x) => x;
        Result<int, string> result = 3;

        var bound = result
            .Bind(FailingStep)
            .Bind(MultiplyByTen)
            .Bind(ToLong);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("first error"));
        }
    }

    [Test]
    public void BindWithValidationErrorsOnSuccess()
    {
        static Result<string, ValidationErrors> Normalize(string name) => name.ToUpperInvariant();
        Result<string, ValidationErrors> result = "Alice";

        var bound = result.Bind(Normalize);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo("ALICE"));
        }
    }

    [Test]
    public void BindWithValidationErrorsPreservesAllErrors()
    {
        static Result<string, ValidationErrors> Normalize(string name) => name.ToUpperInvariant();
        Result<string, ValidationErrors> result = new List<ValidationError>
        {
            new("Invalid name 1"),
            new("Invalid name 2")
        };

        var bound = result.Bind(Normalize);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error.Count(), Is.EqualTo(2));
            Assert.That(bound.Error, Does.Contain(new ValidationError("Invalid name 1")));
            Assert.That(bound.Error, Does.Contain(new ValidationError("Invalid name 2")));
        }
    }
}

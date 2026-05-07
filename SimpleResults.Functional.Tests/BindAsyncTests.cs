using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class BindAsyncTests
{
    [Test]
    public async Task SuccessResultIsBoundToTransformedResult()
    {
        static Task<Result<int, string>> MultiplyByTwo(int x) => Task.FromResult<Result<int, string>>(x * 2);
        Result<int, string> result = 5;

        var bound = await result.BindAsync(MultiplyByTwo);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(10));
        }
    }

    [Test]
    public async Task FailureResultPreservesOriginalError()
    {
        static Task<Result<int, string>> MultiplyByTwo(int x) => Task.FromResult<Result<int, string>>(x * 2);
        Result<int, string> result = "something went wrong";

        var bound = await result.BindAsync(MultiplyByTwo);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("something went wrong"));
        }
    }

    [Test]
    public async Task BindAsyncFunctionIsNotInvokedOnFailure()
    {
        Result<int, string> result = "error";
        bool wasCalled = false;

        var bound = await result.BindAsync(x =>
        {
            wasCalled = true;
            return Task.FromResult<Result<int, string>>(x * 2);
        });

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(wasCalled, Is.False);
        }
    }

    [Test]
    public async Task BindAsyncChangesTheValueType()
    {
        static Task<Result<long, string>> ToLong(int x) => Task.FromResult<Result<long, string>>(x);
        Result<int, string> result = 42;

        var bound = await result.BindAsync(ToLong);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(42L));
        }
    }

    [Test]
    public async Task BindAsyncFunctionReturningFailurePropagatesError()
    {
        static Task<Result<int, string>> Handle(int _) => Task.FromResult<Result<int, string>>("validation failed");
        Result<int, string> result = 5;

        var bound = await result.BindAsync(Handle);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("validation failed"));
        }
    }

    [Test]
    public async Task TaskResultIsBoundToTransformedResult()
    {
        static Task<Result<int, string>> MultiplyByTwo(int x) => Task.FromResult<Result<int, string>>(x * 2);
        Task<Result<int, string>> resultTask = Task.FromResult<Result<int, string>>(5);

        var bound = await resultTask.BindAsync(MultiplyByTwo);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(10));
        }
    }

    [Test]
    public async Task TaskFailureResultPreservesOriginalError()
    {
        static Task<Result<int, string>> MultiplyByTwo(int x) => Task.FromResult<Result<int, string>>(x * 2);
        Task<Result<int, string>> resultTask = Task.FromResult<Result<int, string>>("something went wrong");

        var bound = await resultTask.BindAsync(MultiplyByTwo);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsFailure);
            Assert.That(bound.Error, Is.EqualTo("something went wrong"));
        }
    }

    [Test]
    public async Task ChainedBindAsyncComposeCorrectly()
    {
        static Task<Result<int, string>> Increment(int x) => Task.FromResult<Result<int, string>>(x + 1);
        static Task<Result<int, string>> MultiplyByTen(int x) => Task.FromResult<Result<int, string>>(x * 10);
        static Task<Result<long, string>> ToLong(int x) => Task.FromResult<Result<long, string>>(x);
        Task<Result<int, string>> resultTask = Task.FromResult<Result<int, string>>(3);

        var bound = await resultTask
            .BindAsync(Increment)
            .BindAsync(MultiplyByTen)
            .BindAsync(ToLong);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(40L));
        }
    }
}

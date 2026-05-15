namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class BindErrorAsyncTests
{
    [Test]
    public async Task FailureErrorIsBoundToRecoveredResult()
    {
        static Task<Result<int, string>> Recover(string error) =>
            Task.FromResult<Result<int, string>>(error == "not found" ? 0 : error);
        Result<int, string> result = "not found";

        var bound = await result.BindErrorAsync(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.Zero);
        }
    }

    [Test]
    public async Task SuccessResultPreservesOriginalValue()
    {
        static Task<Result<int, string>> Recover(string _) => Task.FromResult<Result<int, string>>(0);
        Result<int, string> result = 42;

        var bound = await result.BindErrorAsync(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(42));
        }
    }

    [Test]
    public async Task BindErrorAsyncFunctionIsNotInvokedOnSuccess()
    {
        Result<int, string> result = 42;
        bool wasCalled = false;

        var bound = await result.BindErrorAsync(e =>
        {
            wasCalled = true;
            return Task.FromResult<Result<int, string>>(0);
        });

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(wasCalled, Is.False);
        }
    }

    [Test]
    public async Task TaskFailureErrorIsBoundToRecoveredResult()
    {
        static Task<Result<int, string>> Recover(string error) =>
            Task.FromResult<Result<int, string>>(error == "not found" ? 0 : error);
        Task<Result<int, string>> resultTask = Task.FromResult<Result<int, string>>("not found");

        var bound = await resultTask.BindErrorAsync(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.Zero);
        }
    }

    [Test]
    public async Task TaskSuccessResultPreservesOriginalValue()
    {
        static Task<Result<int, string>> Recover(string _) => Task.FromResult<Result<int, string>>(0);
        Task<Result<int, string>> resultTask = Task.FromResult<Result<int, string>>(42);

        var bound = await resultTask.BindErrorAsync(Recover);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(bound.IsSuccess);
            Assert.That(bound.Value, Is.EqualTo(42));
        }
    }
}

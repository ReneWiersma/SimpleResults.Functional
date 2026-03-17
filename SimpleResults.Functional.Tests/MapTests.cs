using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class MapTests
{
    [Test]
    public void SuccessResultIsMappedToTransformedValue()
    {
        Result<int, string> result = 5;

        var mapped = result.Map(x => x * 2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsSuccess);
            Assert.That(mapped.Value, Is.EqualTo(10));
        }
    }

    [Test]
    public void FailureResultPreservesOriginalError()
    {
        Result<int, string> result = "something went wrong";

        var mapped = result.Map(x => x * 2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(mapped.Error, Is.EqualTo("something went wrong"));
        }
    }

    [Test]
    public void MapChangesTheValueType()
    {
        Result<int, string> result = 42;

        var mapped = result.Map(x => x.ToString());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsSuccess);
            Assert.That(mapped.Value, Is.EqualTo("42"));
        }
    }

    [Test]
    public void MappingFunctionIsNotInvokedOnFailure()
    {
        Result<int, string> result = "error";
        bool wasCalled = false;

        var mapped = result.Map(x =>
        {
            wasCalled = true;
            return x * 2;
        });

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(wasCalled, Is.False);
        }
    }

    [Test]
    public void SuccessWithValidationErrorsIsMapped()
    {
        Result<string, ValidationErrors> result = "Alice";

        var mapped = result.Map(name => name.ToUpperInvariant());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsSuccess);
            Assert.That(mapped.Value, Is.EqualTo("ALICE"));
        }
    }

    [Test]
    public void FailureWithValidationErrorsPreservesAllErrors()
    {
        Result<string, ValidationErrors> result = new List<ValidationError>
        {
            new("Invalid name 1"),
            new("Invalid name 2")
        };

        var mapped = result.Map(name => name.ToUpperInvariant());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(mapped.Error.Count(), Is.EqualTo(2));
            Assert.That(mapped.Error, Does.Contain(new ValidationError("Invalid name 1")));
            Assert.That(mapped.Error, Does.Contain(new ValidationError("Invalid name 2")));
        }
    }

    [Test]
    public void ChainedMapsComposeCorrectly()
    {
        Result<int, string> result = 3;

        var mapped = result
            .Map(x => x + 1)
            .Map(x => x * 10)
            .Map(x => x.ToString());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsSuccess);
            Assert.That(mapped.Value, Is.EqualTo("40"));
        }
    }

    [Test]
    public void ChainedMapsShortCircuitOnFailure()
    {
        Result<int, string> result = "error";

        var mapped = result
            .Map(x => x + 1)
            .Map(x => x * 10)
            .Map(x => x.ToString());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(mapped.Error, Is.EqualTo("error"));
        }
    }
}
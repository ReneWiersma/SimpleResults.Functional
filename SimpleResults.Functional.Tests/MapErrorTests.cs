using SoftwareMadeSimple.SimpleResults.Functional;

namespace SoftwareMadeSimple.SimpleResults.Functional.Tests;

public sealed class MapErrorTests
{
    [Test]
    public void FailureErrorIsMappedToTransformedError()
    {
        Result<int, string> result = "not found";

        var mapped = result.MapError(e => e.ToUpperInvariant());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(mapped.Error, Is.EqualTo("NOT FOUND"));
        }
    }

    [Test]
    public void SuccessResultPreservesOriginalValue()
    {
        Result<int, string> result = 42;

        var mapped = result.MapError(e => e.ToUpperInvariant());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsSuccess);
            Assert.That(mapped.Value, Is.EqualTo(42));
        }
    }

    [Test]
    public void MapErrorChangesTheErrorType()
    {
        Result<int, string> result = "not found";

        var mapped = result.MapError(e => e.Length);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(mapped.Error, Is.EqualTo(9));
        }
    }

    [Test]
    public void MappingFunctionIsNotInvokedOnSuccess()
    {
        Result<int, string> result = 42;
        bool wasCalled = false;

        var mapped = result.MapError(e =>
        {
            wasCalled = true;
            return e.ToUpperInvariant();
        });

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsSuccess);
            Assert.That(wasCalled, Is.False);
        }
    }

    [Test]
    public void MapErrorConvertsValidationErrorsToDomainErrors()
    {
        Result<string, ValidationErrors> result = new List<ValidationError>
        {
            new("Invalid name"),
            new("Invalid age")
        };

        var mapped = result.MapError(errors => string.Join("; ", errors.Select(e => e.Message)));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mapped.IsFailure);
            Assert.That(mapped.Error, Is.EqualTo("Invalid name; Invalid age"));
        }
    }
}

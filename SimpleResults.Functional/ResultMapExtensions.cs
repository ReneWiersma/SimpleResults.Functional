namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultMapExtensions
{
    public static Result<R, E> Map<T, R, E>(this Result<T, E> result, Func<T, R> f) =>
        result.IsSuccess
            ? f(result.Value)
            : result.Error;
}

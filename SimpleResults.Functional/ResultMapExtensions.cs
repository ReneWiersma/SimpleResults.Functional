namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultMapExtensions
{
    public static Result<R, E> Map<T, R, E>(this Result<T, E> result, Func<T, R> f) =>
        result.IsSuccess
            ? f(result.Value)
            : result.Error;

    public static Result<T, F> MapError<T, E, F>(this Result<T, E> result, Func<E, F> f) =>
        result.IsSuccess
            ? result.Value
            : f(result.Error);
}

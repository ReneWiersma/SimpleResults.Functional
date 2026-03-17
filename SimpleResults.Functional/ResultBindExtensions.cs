namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultBindExtensions
{
    public static Result<R, E> Bind<T, R, E>(this Result<T, E> result, Func<T, Result<R, E>> f) =>
        result.IsSuccess
            ? f(result.Value)
            : result.Error;
}
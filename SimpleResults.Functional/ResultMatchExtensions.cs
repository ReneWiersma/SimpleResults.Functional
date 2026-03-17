namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultMatchExtensions
{
    public static R Match<T, E, R>(this Result<T, E> result, Func<T, R> onSuccess, Func<E, R> onFailure) =>
        result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);
}
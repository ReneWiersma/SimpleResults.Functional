namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultMatchExtensions
{
    public static R Match<T, E, R>(this Result<T, E> result, Func<T, R> onSuccess, Func<E, R> onFailure) =>
        result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);

    public static void Match<T, E>(this Result<T, E> result, Action<T> onSuccess, Action<E> onFailure)
    {
        if (result.IsSuccess)
            onSuccess(result.Value);
        else
            onFailure(result.Error);
    }
}
namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultBindExtensions
{
    public static Result<R, E> Bind<T, R, E>(this Result<T, E> result, Func<T, Result<R, E>> f) =>
        result.IsSuccess
            ? f(result.Value)
            : result.Error;

    public static Result<T, F> BindError<T, E, F>(this Result<T, E> result, Func<E, Result<T, F>> f) =>
        result.IsSuccess
            ? result.Value
            : f(result.Error);

    public static async Task<Result<R, E>> BindAsync<T, R, E>(this Task<Result<T, E>> resultTask, Func<T, Task<Result<R, E>>> f)
    {
        var result = await resultTask;

        return result.IsSuccess
            ? await f(result.Value)
            : result.Error;
    }

    public static async Task<Result<R, E>> BindAsync<T, R, E>(this Result<T, E> result, Func<T, Task<Result<R, E>>> f) =>
        result.IsSuccess
            ? await f(result.Value)
            : result.Error;

    public static async Task<Result<T, F>> BindErrorAsync<T, E, F>(this Task<Result<T, E>> resultTask, Func<E, Task<Result<T, F>>> f)
    {
        var result = await resultTask;

        return result.IsSuccess
            ? result.Value
            : await f(result.Error);
    }

    public static async Task<Result<T, F>> BindErrorAsync<T, E, F>(this Result<T, E> result, Func<E, Task<Result<T, F>>> f) =>
        result.IsSuccess
            ? result.Value
            : await f(result.Error);
}
namespace SoftwareMadeSimple.SimpleResults.Functional;

public static class ResultApplyExtensions
{
    extension<T, R, E>(Result<Func<T, R>, IEnumerable<E>> resultFunc)
    {
        public Result<R, IEnumerable<E>> Apply(Result<T, IEnumerable<E>> resultValue)
        {
            if (resultFunc.IsSuccess && resultValue.IsSuccess)
            {
                var func = resultFunc.Value;
                var value = resultValue.Value;
                return Result<R, IEnumerable<E>>.Success(func(value));
            }

            // Accumulate errors
            IEnumerable<E> errors = [];

            if (resultFunc.IsFailure)
                errors = errors.Concat(resultFunc.Error);

            if (resultValue.IsFailure)
                errors = errors.Concat(resultValue.Error);

            return Result<R, IEnumerable<E>>.Failure(errors);
        }
    }
}

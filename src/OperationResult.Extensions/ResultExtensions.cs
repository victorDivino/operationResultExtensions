namespace OperationResult.Extensions;

public static class ResultExtensions
{
    public static async Task<Result> Then(this Task<Result> previus, Func<Task<Result>> next)
    {
        try
        {
            Result previusResult = await previus.ConfigureAwait(false);

            return previusResult.IsSuccess ? await next().ConfigureAwait(false) : previusResult.Exception;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public static async Task<Result<T>> Then<T>(this Task<Result> previus, Func<Task<Result<T>>> next)
    {
        try
        {
            Result previusResult = await previus.ConfigureAwait(false);

            return previusResult.IsSuccess ? await next().ConfigureAwait(false) : previusResult.Exception;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public static async Task<Result<TPrevius>> Then<TPrevius>(this Task<Result<TPrevius>> previus, Func<TPrevius, Task<Result>> next)
    {
        try
        {
            var previusResult = await previus.ConfigureAwait(false);

            if (!previusResult.IsSuccess)
                return previusResult.Exception;

            var nextResult = await next(previusResult.Value).ConfigureAwait(false);

            if (!nextResult.IsSuccess)
                return nextResult.Exception;

            return previusResult;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
    
    public static async Task<Result<TResult>> Then<TPrevius, TNext, TResult>(this Task<Result<TPrevius>> previus, Func<TPrevius, Task<Result<TNext>>> next, Func<TPrevius, TNext, TResult> result)
    {
        try
        {
            var previusResult = await previus;

            if (!previusResult.IsSuccess)
                return previusResult.Exception;

            var nextResult = await next(previusResult.Value).ConfigureAwait(false);

            if (!nextResult.IsSuccess)
                return nextResult.Exception;

            return result(previusResult.Value, nextResult.Value);
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public static async Task<Result> ThenChangeInNoResult<T>(this Task<Result<T>> previus)
    {
        try
        {
            var previusResult = await previus.ConfigureAwait(false);

            return previusResult.IsSuccess ? previusResult.ChangeInNoResult() : previusResult.Exception;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
}

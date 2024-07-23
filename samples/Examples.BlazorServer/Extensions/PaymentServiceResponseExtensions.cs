namespace Examples.BlazorServer.Extensions;

public static class PaymentServiceResponseExtensions
{
    public static Result<T> ToResult<T>(this T response)
        where T : PaymentServiceResponse
    {
        return response.IsSuccess
            ? Result.Success(response)
            : Result.Failure<T>($"Payment Gateway error: {response.ErrorMessage}({response.HttpStatusCode})");
    }
    public static async Task<Result<T>> ToResult<T>(this Task<T> responseAsync)
        where T : PaymentServiceResponse
    {
        var response = await responseAsync.ConfigureAwait(false);
        return response.ToResult();
    }
}

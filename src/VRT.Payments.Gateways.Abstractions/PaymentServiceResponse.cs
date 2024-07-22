namespace VRT.Payments.Gateways;

public record PaymentServiceResponse
{
    required public int HttpStatusCode { get; init; }
    required public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static PaymentServiceResponse Success()
    {
        return new PaymentServiceResponse()
        {
            HttpStatusCode = (int)System.Net.HttpStatusCode.OK,
            IsSuccess = true
        };
    }
    public static PaymentServiceResponse Fail(string message)
    {
        return Fail(message, (int)System.Net.HttpStatusCode.BadRequest);
    }

    public static PaymentServiceResponse Fail(string message, int httpStatusCode)
    {
        return new PaymentServiceResponse()
        {
            HttpStatusCode = httpStatusCode,
            IsSuccess = false,
            ErrorMessage = message
        };
    }
}

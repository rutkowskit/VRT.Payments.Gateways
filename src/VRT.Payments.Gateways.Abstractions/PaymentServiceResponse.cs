namespace VRT.Payments.Gateways;

/// <summary>
/// Basic Payment service response
/// </summary>
public record PaymentServiceResponse
{
    /// <summary>
    /// Http numeric status code
    /// </summary>
    required public int HttpStatusCode { get; init; }

    /// <summary>
    /// Set to true if response indicate success
    /// </summary>
    required public bool IsSuccess { get; init; }

    /// <summary>
    /// Error message 
    /// </summary>
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

namespace VRT.Payments.Gateways;

/// <summary>
/// Payment status information in Gateway System
/// </summary>
public sealed record GetPaymentStatusResponse : PaymentServiceResponse
{
    /// <summary>
    /// Order id in Gateways System
    /// </summary>
    required public string OrderId { get; init; }
    /// <summary>
    /// Payment status in Gateways System
    /// </summary>
    required public PaymentStatus Status { get; init; } = PaymentStatus.None;

    /// <summary>
    /// Order id in Order Creators System (ReferenceId)
    /// </summary>
    public string? ExtOrderId { get; init; }

    public static GetPaymentStatusResponse Fail(string orderId, string message)
        => Fail(orderId, message, (int)System.Net.HttpStatusCode.BadRequest);

    public static GetPaymentStatusResponse Fail(string orderId, string message, int httpStatusCode)
    {
        var result = new GetPaymentStatusResponse()
        {
            OrderId = orderId,
            IsSuccess = false,
            ErrorMessage = message,
            Status = PaymentStatus.None,
            HttpStatusCode = httpStatusCode
        };
        return result;
    }
}

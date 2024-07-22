namespace VRT.Payments.Gateways;

public sealed record GetPaymentStatusResponse : PaymentServiceResponse
{
    required public string OrderId { get; init; }
    required public PaymentStatus Status { get; init; } = PaymentStatus.None;

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

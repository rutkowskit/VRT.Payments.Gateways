using VRT.Payments.Gateways.PayU.Services.DataContracts;

namespace VRT.Payments.Gateways.Abstractions;

public sealed record GetPaymentStatusResponse : PaymentServiceResponse
{
    required public string OrderId { get; init; }
    required public PaymentStatus Status { get; init; } = PaymentStatus.None;

    internal static GetPaymentStatusResponse Fail(string message)
        => Fail("", message);

    internal static GetPaymentStatusResponse Fail(string orderId, string message)
        => Fail(orderId, message, System.Net.HttpStatusCode.BadRequest.ToString());

    internal static GetPaymentStatusResponse Fail(string orderId, string message, string code)
    {
        var result = new GetPaymentStatusResponse()
        {
            OrderId = orderId,
            IsSuccess = false,
            ErrorMessage = message,
            Status = PaymentStatus.None,
            HttpStatusCode = code
        };
        return result;
    }
    internal static GetPaymentStatusResponse Success(Shared.Order order)
    {
        var result = new GetPaymentStatusResponse()
        {
            OrderId = order.orderId,
            IsSuccess = true,
            Status = order.GetPaymentStatus(),
            HttpStatusCode = System.Net.HttpStatusCode.OK.ToString()
        };
        return result;
    }
}

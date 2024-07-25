namespace VRT.Payments.Gateways.PayU.Services.DataContracts;
internal static class OrderExtensions
{
    public static GetPaymentStatusResponse ToSuccess(this Shared.Order order)
    {
        var result = new GetPaymentStatusResponse()
        {
            OrderId = order.orderId,
            ExtOrderId = order.extOrderId,
            IsSuccess = true,
            Status = order.GetPaymentStatus(),
            HttpStatusCode = (int)System.Net.HttpStatusCode.OK
        };
        return result;
    }
    public static GetPaymentStatusResponse ToPaymentStatusResponse(this Shared.Order order,
        PaymentServiceResponse response)
    {
        var result = new GetPaymentStatusResponse()
        {
            OrderId = order.orderId,
            ExtOrderId = order.extOrderId,
            IsSuccess = response.IsSuccess,
            Status = response.IsSuccess ? order.GetPaymentStatus() : PaymentStatus.None,
            HttpStatusCode = response.HttpStatusCode,
            ErrorMessage = response.ErrorMessage
        };
        return result;
    }
}

namespace VRT.Payments.Gateways.PayU.Services.DataContracts;
internal static class OrderExtensions
{
    public static GetPaymentStatusResponse Success(this Shared.Order order)
    {
        var result = new GetPaymentStatusResponse()
        {
            OrderId = order.orderId,
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
            IsSuccess = response.IsSuccess,
            Status = response.IsSuccess ? order.GetPaymentStatus() : PaymentStatus.None,
            HttpStatusCode = response.HttpStatusCode,
            ErrorMessage = response.ErrorMessage
        };
        return result;
    }
}

using Microsoft.AspNetCore.Http;

namespace VRT.Payments.Gateways.Abstractions;

public interface IPaymentService
{
    Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
    Task<GetPaymentStatusResponse> GetPaymentStatus(string orderId);
    Task<GetPaymentStatusResponse> GetPaymentStatusFromNotification(HttpRequest request);
}

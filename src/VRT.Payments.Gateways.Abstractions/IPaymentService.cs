using Microsoft.AspNetCore.Http;

namespace VRT.Payments.Gateways;

public interface IPaymentService
{
    string GetProviderName();
    Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
    Task<CancelOrderResponse> CancelOrder(string orderId);
    Task<GetPaymentStatusResponse> GetPaymentStatus(string orderId);
    Task<GetPaymentStatusResponse> GetPaymentStatusFromNotification(HttpRequest request);
}

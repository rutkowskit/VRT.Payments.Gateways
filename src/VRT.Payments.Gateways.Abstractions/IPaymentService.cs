using Microsoft.AspNetCore.Http;

namespace VRT.Payments.Gateways;

/// <summary>
/// Payment service
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Gets the name of the payment provider
    /// </summary>
    /// <returns>Name of the provider</returns>
    string GetProviderName();
    /// <summary>
    /// Creates an order in the payment gateway
    /// </summary>
    /// <param name="request">Request data</param>
    /// <returns>Result of an order creation</returns>
    Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
    /// <summary>
    /// Cancels an order in the payment gateway
    /// </summary>
    /// <param name="orderId">Payment gateway Order Id to cancel</param>
    /// <returns>Cancellation result</returns>
    Task<CancelOrderResponse> CancelOrder(string orderId);
    /// <summary>
    /// Gets the current payment status of an order
    /// </summary>
    /// <param name="orderId">Payment gateway Order Id</param>
    /// <returns>Payment status of an order</returns>
    Task<GetPaymentStatusResponse> GetPaymentStatus(string orderId);
    /// <summary>
    /// Resolves the payment status from http request received by the notification endpoint (sent by payment gateway)
    /// </summary>
    /// <param name="request">Http request from payment gateway</param>
    /// <returns>Payment status of an order retrieved from notification http request</returns>
    Task<GetPaymentStatusResponse> GetPaymentStatusFromNotification(HttpRequest request);
}

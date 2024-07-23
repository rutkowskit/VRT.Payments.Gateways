using Refit;
using VRT.Payments.Gateways.PayU.Services.DataContracts;

namespace VRT.Payments.Gateways.PayU.Services;

[Headers("Content-Type: application/json")]
internal interface IPayUOrdersClient
{
    [Post("/api/v2_1/orders")]
    internal Task<ApiResponse<CreateOrder.Response>> CreateOrder(CreateOrder.Request request);

    [Get("/api/v2_1/orders/{orderId}")]
    internal Task<ApiResponse<GetOrder.Response>> GetOrder(string orderId);

    [Delete("/api/v2_1/orders/{orderId}")]
    internal Task<ApiResponse<CancelOrder.Response>> CancelOrder(string orderId);
}

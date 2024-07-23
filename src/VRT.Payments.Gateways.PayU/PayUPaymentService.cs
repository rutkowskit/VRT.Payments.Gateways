using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using System.Runtime.CompilerServices;
using VRT.Payments.Gateways.PayU.Options;
using VRT.Payments.Gateways.PayU.Services;
using VRT.Payments.Gateways.PayU.Services.DataContracts;
using Contracts = VRT.Payments.Gateways.PayU.Services.DataContracts;

namespace VRT.Payments.Gateways.PayU;

internal sealed partial class PayUPaymentService : IPaymentService
{
    private readonly IPayUOrdersClient _ordersService;
    private readonly IOptions<PayUOptions> _options;
    private readonly ILogger<PayUPaymentService> _logger;

    public PayUPaymentService(
        IPayUOrdersClient ordersService,
        IOptions<PayUOptions> options,
        ILogger<PayUPaymentService> logger)
    {
        _ordersService = ordersService;
        _options = options;
        _logger = logger;
    }
    public string GetProviderName() => Constants.GatewayName;

    public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
    {
        var createOrderRequest = Contracts.CreateOrder.Request.From(request, _options.Value);
        var response = await _ordersService.CreateOrder(createOrderRequest);
        TryLogError(response.Error);
        var result = ToServiceResponse(request, response);
        return result;
    }
    public async Task<CancelOrderResponse> CancelOrder(string orderId)
    {
        var serviceResponse = await _ordersService.CancelOrder(orderId);
        var isSuccess = serviceResponse.IsSuccessStatusCode && serviceResponse.Content.status.statusCode == "SUCCESS";
        TryLogError(serviceResponse.Error);
        return new CancelOrderResponse()
        {
            HttpStatusCode = (int)serviceResponse.StatusCode,
            IsSuccess = isSuccess,
            OrderId = orderId,
            ExtOrderId = serviceResponse.Content?.extOrderId,
            ErrorMessage = isSuccess
                ? null
                : serviceResponse.Error?.Message ?? serviceResponse.Error?.Content
        };
    }
    public async Task<GetPaymentStatusResponse> GetPaymentStatus(string orderId)
    {
        var orderResponse = await _ordersService.GetOrder(orderId);
        var order = orderResponse.Content?.orders?.FirstOrDefault();
        var status = orderResponse.IsSuccessStatusCode && order is not null
            ? order.GetPaymentStatus()
            : PaymentStatus.None;

        TryLogError(orderResponse.Error);
        var result = new GetPaymentStatusResponse()
        {
            OrderId = orderId,
            Status = status,
            HttpStatusCode = (int)orderResponse.StatusCode,
            IsSuccess = orderResponse.IsSuccessStatusCode,
            ErrorMessage = orderResponse.Error?.Content ?? orderResponse.Error?.Message
        };
        return result;
    }

    private static CreateOrderResponse ToServiceResponse(CreateOrderRequest request, ApiResponse<CreateOrder.Response> response)
    {
        var isSuccess = response.IsSuccessStatusCode && response.Content?.status?.statusCode == "SUCCESS";
        return new CreateOrderResponse()
        {
            IsSuccess = isSuccess,
            HttpStatusCode = (int)response.StatusCode,
            ErrorMessage = isSuccess
                ? null
                : response.Error?.Message ?? response.Error?.Content ?? "Unknown error",
            ExtOrderId = response.Content?.extOrderId ?? request.ExtOrderId,
            RedirectUrl = response.Content?.redirectUri,
            OrderId = response.Content?.orderId
        };
    }
    private void TryLogError(ApiException? exception, [CallerMemberName] string? caller = null)
    {
        if (exception != null)
        {
            _logger.LogError(exception, "PayUPaymentService api exception: {Caller} {Message}, {Content}", caller, exception.Message, exception.Content);
        }
    }
    private void LogException(Exception? exception, [CallerMemberName] string? caller = null)
    {
        if (exception != null)
        {
            _logger.LogError(exception, "PayUPaymentService api exception: {Caller} {Message}", caller, exception.Message);
        }
    }


}

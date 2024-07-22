namespace VRT.Payments.Gateways;

public sealed record CreateOrderResponse : PaymentServiceResponse
{
    public string? RedirectUrl { get; init; }
    public string? OrderId { get; init; }
    public string? ExtOrderId { get; init; }
}

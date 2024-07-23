namespace VRT.Payments.Gateways;

public sealed record CancelOrderResponse : PaymentServiceResponse
{
    public string? OrderId { get; init; }
    public string? ExtOrderId { get; init; }
}

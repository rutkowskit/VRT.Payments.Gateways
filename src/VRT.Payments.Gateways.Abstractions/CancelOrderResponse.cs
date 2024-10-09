namespace VRT.Payments.Gateways;

/// <summary>
/// Response after order cancellation
/// </summary>
public sealed record CancelOrderResponse : PaymentServiceResponse
{
    /// <summary>
    /// Canceled order id
    /// </summary>
    public string? OrderId { get; init; }
    /// <summary>
    /// Canceled Order id in store owner's system
    /// </summary>
    public string? ExtOrderId { get; init; }
}

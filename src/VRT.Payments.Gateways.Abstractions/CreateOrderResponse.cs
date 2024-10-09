namespace VRT.Payments.Gateways;

/// <summary>
/// Response after order creation
/// </summary>
public sealed record CreateOrderResponse : PaymentServiceResponse
{
    /// <summary>
    /// Redirect url
    /// </summary>
    public string? RedirectUrl { get; init; }
    /// <summary>
    /// Payment Gateway Order Id assigned after creation
    /// </summary>
    public string? OrderId { get; init; }
    /// <summary>
    /// Order Id in the store owner's system
    /// </summary>
    public string? ExtOrderId { get; init; }
}

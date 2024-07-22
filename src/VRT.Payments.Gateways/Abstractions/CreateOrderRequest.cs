namespace VRT.Payments.Gateways.Abstractions;

public sealed record CreateOrderRequest
{
    required public string CustomerIp { get; init; }
    required public string Description { get; init; }
    /// <summary>
    /// Currency code in ISO format. eg. EUR, PLN, USD
    /// </summary>
    required public string CurrencyCode { get; init; }
    required public decimal TotalPrice { get; init; }
    required public LineItemDto[] LineItems { get; init; } = [];
    public string? ExtOrderId { get; init; }
    public BuyerDto? Buyer { get; init; }
    public string? NotifyUrl { get; init; }
    public string? ContinueUrl { get; init; }

    public sealed class LineItemDto
    {
        required public string Name { get; init; }
        required public decimal UnitPrice { get; init; }
        required public decimal Quantity { get; init; }
        public bool IsNonPhysical { get; init; }
    }
    public sealed record BuyerDto
    {
        required public string FirstName { get; init; }
        required public string LastName { get; init; }
        required public string Email { get; init; }
        public string? LanguageIso { get; init; }
        public string? ExtCustomerId { get; init; }
    }
}
namespace VRT.Payments.Gateways;

/// <summary>
/// Order creation data
/// </summary>
public sealed record CreateOrderRequest
{
    /// <summary>
    /// Ip of a customer creating the request
    /// </summary>
    required public string CustomerIp { get; init; }
    /// <summary>
    /// Order description
    /// </summary>
    required public string Description { get; init; }
    /// <summary>
    /// Currency code in ISO format. eg. EUR, PLN, USD
    /// </summary>
    required public string CurrencyCode { get; init; }
    /// <summary>
    /// Total order price
    /// </summary>
    required public decimal TotalPrice { get; init; }
    /// <summary>
    /// Order line items
    /// </summary>
    required public LineItemDto[] LineItems { get; init; } = [];
    /// <summary>
    /// Order id in Clients system
    /// </summary>
    public string? ExtOrderId { get; init; }
    /// <summary>
    /// Buyer data
    /// </summary>
    public BuyerDto? Buyer { get; init; }
    /// <summary>
    /// Notification listener url (calling client side)
    /// </summary>
    public string? NotifyUrl { get; init; }
    /// <summary>
    /// Redirection url after finalizing/canceling order on Payment Gateway
    /// </summary>
    public string? ContinueUrl { get; init; }

    public sealed class LineItemDto
    {
        /// <summary>
        /// Name of a product/service
        /// </summary>
        required public string Name { get; init; }
        /// <summary>
        /// Price per unit
        /// </summary>
        required public decimal UnitPrice { get; init; }
        /// <summary>
        /// Quantity of a product in specific units
        /// </summary>
        required public decimal Quantity { get; init; }
        /// <summary>
        /// Indicates if a product is phisical or virtual
        /// </summary>
        public bool IsNonPhysical { get; init; }
    }
    /// <summary>
    /// Buyer data model
    /// </summary>
    public sealed record BuyerDto
    {
        /// <summary>
        /// First name of a buyer
        /// </summary>
        required public string FirstName { get; init; }
        /// <summary>
        /// Last name of a buyer
        /// </summary>
        required public string LastName { get; init; }
        /// <summary>
        /// Buyers email
        /// </summary>
        required public string Email { get; init; }
        /// <summary>
        /// Buyers language code in iso format. E.g "en", "pl"
        /// </summary>
        public string? LanguageIso { get; init; }
        /// <summary>
        /// Customer Id in store owner system
        /// </summary>
        public string? ExtCustomerId { get; init; }
    }
}
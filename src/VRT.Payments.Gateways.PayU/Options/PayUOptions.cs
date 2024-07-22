using System.Diagnostics.CodeAnalysis;

namespace VRT.Payments.Gateways.PayU.Options;

public sealed class PayUOptions : IPaymentGatewayOptions<PayUOptions>
{
    public static string ConfigurationSectionName { get; } = "PaymentGateways:PayU";

    [NotNull]
    public string BaseApiUrl { get; set; } = "https://secure.snd.payu.com";

    [NotNull]
    public string ClientId { get; set; } = null!;

    [NotNull]
    public string ClientSecret { get; set; } = null!;

    [NotNull]
    public string PointOfSellId { get; set; } = null!;

    [NotNull]
    public string SecondKey { get; set; } = null!;
}

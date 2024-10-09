namespace VRT.Payments.Gateways;

/// <summary>
/// Payment gateway options
/// </summary>
/// <typeparam name="TSelf">Generic options type</typeparam>
public interface IPaymentGatewayOptions<TSelf>
    where TSelf : class, IPaymentGatewayOptions<TSelf>
{
    /// <summary>
    /// Name of a section in configuration service
    /// </summary>
    abstract static string ConfigurationSectionName { get; }
}
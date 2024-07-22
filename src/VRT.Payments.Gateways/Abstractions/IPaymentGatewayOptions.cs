namespace VRT.Payments.Gateways.Abstractions;


public interface IPaymentGatewayOptions<TSelf>
    where TSelf : class, IPaymentGatewayOptions<TSelf>
{
    abstract static string ConfigurationSectionName { get; }
}
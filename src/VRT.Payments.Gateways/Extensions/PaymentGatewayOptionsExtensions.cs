using Microsoft.Extensions.DependencyInjection;

namespace VRT.Payments.Gateways.Extensions;

public static class PaymentGatewayOptionsExtensions
{
    /// <summary>
    /// Add options using IConfiguration
    /// </summary>
    /// <typeparam name="T">Options</typeparam>
    /// <param name="services">Service collection</param>
    /// <param name="validator">Validating function</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddPaymentGatewayOptions<T>(this IServiceCollection services,
        Func<T, bool>? validator = null)
        where T : class, IPaymentGatewayOptions<T>
    {
        var cfg = services
            .AddOptions<T>()
            .BindConfiguration(T.ConfigurationSectionName);

        if (validator is not null)
        {
            cfg.Validate(validator);
            cfg.ValidateOnStart();
        }
        return services;
    }
}

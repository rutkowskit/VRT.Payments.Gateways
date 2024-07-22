using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using VRT.Payments.Gateways.Common;
using VRT.Payments.Gateways.PayU;
using VRT.Payments.Gateways.PayU.Options;
using VRT.Payments.Gateways.PayU.Services;

namespace VRT.Payments.Gateways;

public static partial class DependencyInjection
{
    public static IServiceCollection AddPayUPaymentService(this IServiceCollection services)
    {
        services.AddTransient<PayUAuthHeaderHandler>();
        services
            .AddOptions<PayUOptions>()
            .BindConfiguration(PayUOptions.ConfigurationSectionName)
            .ValidateOnStart();

        var refitSettings = GetRefitSettings();
        services
            .AddRefitClient<IPayUAuthClient>(refitSettings)
            .ConfigurePayUHttpClient()
            .WithNoRedirectHttpHandler();

        services
            .AddRefitClient<IPayUOrdersClient>(refitSettings)
            .AddHttpMessageHandler<PayUAuthHeaderHandler>()
            .ConfigurePayUHttpClient()
            .WithNoRedirectHttpHandler();

        services.AddTransient<IPaymentService, PayUPaymentService>();
        return services;
    }

    private static IHttpClientBuilder ConfigurePayUHttpClient(this IHttpClientBuilder builder)
    {
        return builder.ConfigureHttpClient((provider, client) =>
         {
             var config = provider.GetRequiredService<IOptions<PayUOptions>>();
             client.BaseAddress = new Uri(config.Value.BaseApiUrl);
         });
    }

    private static RefitSettings GetRefitSettings()
    {
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            IgnoreReadOnlyProperties = true,
            IgnoreReadOnlyFields = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(options)
        };
        return refitSettings;
    }
    private static IHttpClientBuilder WithNoRedirectHttpHandler(this IHttpClientBuilder builder)
    {
        return builder.ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new RedirectMessageHttpClientHandler();
        });
    }
}

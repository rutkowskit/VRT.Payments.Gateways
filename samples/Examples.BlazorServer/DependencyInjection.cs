using Examples.BlazorServer.Database;
using Examples.BlazorServer.FileStorage;

namespace Examples.BlazorServer;

internal static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<PaymentsDatabase>();
        services.AddSingleton<FileStorageOptions>();
        services.AddSingleton<IFileStorage, DefaultFileStorage>();
        services.AddPayUPaymentService();
        return services;
    }
}

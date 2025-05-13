using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailProvider.Models;

public class AzureCommunicationSettings
{
    public string SenderAddress { get; set; } = null!;
}

public static class AzureCommunicationSettingsExtensions
{
    public static IServiceCollection AddAzureCommunicationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureCommunicationSettings>(
            configuration.GetSection("Values:AzureCommunication__"));
        return services;
    }
}

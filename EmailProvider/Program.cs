using Azure.Communication.Email;
using Azure.Messaging.ServiceBus;
using EmailProvider.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var senderAddress = builder.Configuration["SenderAddress"] ?? throw new InvalidOperationException();
var serviceBus = builder.Configuration["ServiceBus"] ?? throw new InvalidOperationException();
var connectionString = builder.Configuration["ConnectionString"] ?? throw new InvalidOperationException();


builder.Services.AddSingleton(_ => new EmailClient(connectionString));
builder.Services.AddSingleton(_ => new ServiceBusClient(serviceBus));
builder.Services.AddSingleton(sp =>
    {
        var emailClient = sp.GetRequiredService<EmailClient>();
        return new EmailService(emailClient, senderAddress);
    }
);
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
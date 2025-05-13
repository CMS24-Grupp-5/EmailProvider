using Azure.Communication.Email;
using Azure.Messaging.ServiceBus;
using EmailProvider.Models;
using EmailProvider.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddAzureCommunicationSettings(builder.Configuration);


builder.Services.AddSingleton(x => new EmailClient(builder.Configuration["Values:AzureCommunication__ConnectionString"]));
builder.Services.AddSingleton(x => new ServiceBusClient(builder.Configuration["Values:ServiceBus"]));
builder.Services.AddSingleton<EmailService>();
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
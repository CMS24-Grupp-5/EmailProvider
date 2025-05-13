using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmailProvider;

public class EmailServiceProvider
{
    private readonly ILogger<EmailServiceProvider> _logger;

    public EmailServiceProvider(ILogger<EmailServiceProvider> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EmailServiceProvider))]
    public async Task Run(
        [ServiceBusTrigger("emailservice", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        await messageActions.CompleteMessageAsync(message);
        
    }
}
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using EmailProvider.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmailProvider.Actions;

public class EmailRequestTrigger
{
    private readonly ILogger<EmailRequestTrigger> _logger;

    public EmailRequestTrigger(ILogger<EmailRequestTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EmailRequestTrigger))]
    public async Task Run(
        [ServiceBusTrigger("emailservice", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        try
        {
            var request = JsonSerializer.Deserialize<EmailSendRequest>(
                message.Body.ToString(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            
            ArgumentNullException.ThrowIfNull(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        

        await messageActions.CompleteMessageAsync(message);
        
    }
}
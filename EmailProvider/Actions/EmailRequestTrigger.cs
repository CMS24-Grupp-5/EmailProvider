using System.Text.Json;
using Azure.Messaging.ServiceBus;
using EmailProvider.Models;
using EmailProvider.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmailProvider.Actions;

public class EmailRequestTrigger
{
    private readonly ILogger<EmailRequestTrigger> _logger;
    private readonly EmailService _emailService;

    public EmailRequestTrigger(ILogger<EmailRequestTrigger> logger, EmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
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

            var result = await _emailService.SendAsync(request);

            if (result.Succeeded)
            {
                _logger.LogInformation("Email result: {message}", result.Message);
                await messageActions.CompleteMessageAsync(message);
            }
            else
            {
                _logger.LogWarning("Failed to send email: {message}", result.Message);
                await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object>{{"Reason", "Email failed to send"}});
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process the message");
            await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object>{{"Reason", e.Message}});
        }
    }
}
using Azure;
using Azure.Communication.Email;
using EmailProvider.Models;

namespace EmailProvider.Services;

public class EmailService
{
    private readonly EmailClient _client;
    private readonly string _senderAddress;

    public EmailService(EmailClient client, string senderAddress)
    {
        _client = client;
        _senderAddress = senderAddress;
    }
    public async Task<EmailResult> SendAsync(EmailSendRequest request)
    {
        try
        {
            if (request.Recipients.Count == 0)
            {
                throw new ArgumentException("At least one recipient is required.", nameof(request));
            }

            var recipients = request.Recipients.Select(x => new EmailAddress(x)).ToList();

            var message = new EmailMessage(
                senderAddress: _senderAddress,
                content: new EmailContent(request.Subject)
                {
                    PlainText = request.PlainText,
                    Html = request.Html
                },
                recipients: new EmailRecipients(recipients)
            );

            var result = await _client.SendAsync(WaitUntil.Completed, message);

            return new EmailResult { Succeeded = result.HasCompleted, Message = "Email was sent successfully." };
        }
        catch (Exception e)
        {
            return new EmailResult { Message = $"There was an error: {e.Message}" };
        }
    }
}
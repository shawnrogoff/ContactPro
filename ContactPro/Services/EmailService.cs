using ContactPro.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ContactPro.Services;

public class EmailService : IEmailSender
{
    private readonly MailSettings _mailSettings;

    public EmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailSender = _mailSettings.Email ?? Environment.GetEnvironmentVariable("Email");

        MimeMessage newEmail = new();

        foreach (var emailAddress in email.Split(";"))
        {
            newEmail.To.Add(MailboxAddress.Parse(emailAddress));
        }

        newEmail.Subject = subject;

        BodyBuilder emailBody = new BodyBuilder();

        emailBody.HtmlBody = htmlMessage;

        newEmail.Body = emailBody.ToMessageBody();
        newEmail.Sender = MailboxAddress.Parse(emailSender);

        using SmtpClient smtpClient = new();

        try
        {
            var host = _mailSettings.Host ?? Environment.GetEnvironmentVariable("Host");
            var port = _mailSettings.Port != 0 ? _mailSettings.Port : int.Parse(Environment.GetEnvironmentVariable("Port")!);
            var password = _mailSettings.Password ?? Environment.GetEnvironmentVariable("Password");

            Console.WriteLine("Now going to try to connect to smtpClient...");

            await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            Console.WriteLine("Connection successful...");
            
            Console.WriteLine("Now going to try to authenticate...");
            await smtpClient.AuthenticateAsync(emailSender, password);

            Console.WriteLine("Authentication successful...");

            Console.WriteLine("Attempting to send email...");
            await smtpClient.SendAsync(newEmail);

            Console.WriteLine("Disconnecting...");
            await smtpClient.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            var error = ex.Message;
            Console.WriteLine(error);
            throw;
        }
    }
}

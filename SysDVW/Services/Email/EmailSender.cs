using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SysDVW.Services.Email
{

    public class EmailSender : IEmailSender
    {

        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var _emailMessage = CreateEmailMessage(message);

            Send(_emailMessage);
        }

        public async Task SendEmailAsync(Message message)
        {
            var _mailMessage = CreateEmailMessage(message);

            await SendAsync(_mailMessage);
        }


        private MimeMessage CreateEmailMessage(Message message)
        {
            var _emailMessage = new MimeMessage();
            _emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From)); //new MailboxAddress(_emailConfig.From)
            _emailMessage.To.AddRange(message.To);
            _emailMessage.Subject = message.Subject;

            var _bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    _bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            _emailMessage.Body = _bodyBuilder.ToMessageBody();
            return _emailMessage;
        }


        private void Send(MimeMessage mailMessage)
        {
            using (var _client = new SmtpClient())
            {
                try
                {
                    _client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    _client.AuthenticationMechanisms.Remove("XOAUTH2");
                    _client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    _client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    //log an error message or throw an exception, or both.
                    throw ex;
                }
                finally
                {
                    _client.Disconnect(true);
                    _client.Dispose();
                }
            }
        }


        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    //log an error message or throw an exception, or both.
                    throw ex;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

    }
}

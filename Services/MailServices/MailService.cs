using System.Collections.Generic;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Biddo.Services.Models;
using MailKit.Security;

namespace Biddo.Services.MailServices
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult SendMail(MailDto request)
        {

            try
            {
                // Define the email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("MailConfig:HostMail").Value));
                //email.To.Add(MailboxAddress.Parse("appointmentschedular404@gmail.com"));
                    // Add recipients
                InternetAddressList toRecipients = new InternetAddressList();
                foreach (var recipient in request.ToList)
                {
                toRecipients.Add(MailboxAddress.Parse(recipient));
                }
                // TODO
                email.To.AddRange(toRecipients);
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };


                // SMTP
                using (var smtp = new SmtpClient())
                {
                    //smtp.Connect("smtp.gmail.com", 587, false);

                    //// Authenticate with the Gmail account credentials


                    // Configuration FAKE SMTP
                    smtp.Connect(_configuration.GetSection("MailConfig:MailHost").Value, 587,SecureSocketOptions.StartTls);
                    smtp.Authenticate(_configuration.GetSection("MailConfig:HostMail").Value, _configuration.GetSection("MailConfig:HostPassword").Value);

                    // Send the message
                    smtp.Send(email);

                    smtp.Disconnect(true);

                }
            
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

        }

    }
}

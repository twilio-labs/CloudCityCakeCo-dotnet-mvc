using System;
using System.IO;
using System.Threading.Tasks;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Enums;
using CloudCityCakeCo.Models.Helpers;
using CloudCityCakeCo.Services.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CloudCityCakeCo.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly SendGridAccount _account;

        public EmailService(IOptions<SendGridAccount> account)
        {
            _account = account.Value ?? throw new ArgumentNullException(nameof(account));
        }

        public async Task<ServiceResponse> SendEmail(CakeOrder cakeOrder)
        {
            var email = CreateEmail(cakeOrder);
            var message = MailHelper.CreateSingleEmail(
                email.From,
                email.To,
                email.Subject,
                email.PlainTextContent,
                email.HtmlContent);
            
            message.AddAttachment(email.Filename, EncodedAttachment(email.FilePath));
            
            var client = new SendGridClient(_account.ApiKey);

            var _ =  await client.SendEmailAsync(message);
            
            return new ServiceResponse
            {
                Message = "email sent",
                ServiceResponseStatus = ServiceResponseStatus.Ok
            };
        }

        private Email CreateEmail(CakeOrder cakeOrder)
        {
            var body =
                $"Hello {cakeOrder.User.Name}, Your cake order has been accepted. Please find your invoice attached.";
            var invoicePath = CreateInvoice();

            var email = new Email
            {
                To = new EmailAddress(cakeOrder.User.Email, cakeOrder.User.Name),
                From = new EmailAddress(_account.EmailFromAddress, "The Cloud City Cake Co."),
                Subject = $"Your invoice for order id {cakeOrder.Id}",
                HtmlContent = $"<strong>{body}<strong>",
                PlainTextContent = body,
                Filename = $"{cakeOrder.Id}.pdf",
                FilePath = invoicePath
            };

            return email;
        }

        private string CreateInvoice()
        {
            return @"\Users\Layla\source\repos\CloudCityCakeCo-dotnet-mvc\Code\invoice.pdf";
        }


        private string EncodedAttachment(string path)
        {
            Byte[] bytes = File.ReadAllBytes(path);
            string file = Convert.ToBase64String(bytes);
            return file;
        }
    }
}
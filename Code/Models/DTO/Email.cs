using SendGrid.Helpers.Mail;

namespace CloudCityCakeCo.Models.DTO
{
    public class Email
    {
        public EmailAddress From { get; set; }
        public EmailAddress To { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
        
        public string Filename { get; set; }
        public string FilePath { get; set; }
    }
}
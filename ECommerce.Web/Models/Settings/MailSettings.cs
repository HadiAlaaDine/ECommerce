namespace ECommerce.Web.Models.Settings
{
    public class MailSettings
    {
        public required SmtpSettings Smtp { get; set; }
        public required string From { get; set; }
    }

    public class SmtpSettings
    {
        public required string Host { get; set; }
        public required string Port { get; set; }
        public required string User { get; set; }
        public required string Pass { get; set; }
    }
}
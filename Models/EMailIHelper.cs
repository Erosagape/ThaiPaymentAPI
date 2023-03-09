using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
namespace ThaiPaymentAPI.Models
{
    public static class EMailHelper
    {
        public static ErrorResponse SentEMail(MailMessage mail)
        {
            ErrorResponse err = new ErrorResponse()
            {
                success = true,
                error = "OK",
                data = "Mail Sent to " + mail.toPerson
            };
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(mail.fromPerson,mail.fromEmail));
                email.To.Add(new MailboxAddress(mail.toPerson, mail.toEmail));
                email.Subject = mail.subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = mail.body
                };
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(mail.smtpHost, mail.smtpPort, mail.useSSL);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate(mail.emailAddress, mail.emailPassword);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                err = new ErrorResponse()
                {
                    success = false,
                    error = e.Message,
                    data = e.StackTrace
                };
            }
            return err;
        }
        public static string TestSentMail()
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Me", "no-reply@aih-consultant.com"));
                email.To.Add(new MailboxAddress("You", "leoputti@hotmail.com"));
                email.Subject = "Test Mail Sent using MailKit";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "<b>This is the Mail Body</b><br/><u>Presented by Mail Kit</u>"
                };
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 465,true);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("littlepuppet123@gmail.com", "unfrpexlowndiqrf");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                return "Mail Sent";
            } catch(Exception e)
            {
                return e.Message;
            }
        }
        public static MailMessage GetNewEMail()
        {
            var o = new Models.SystemConfig()
                                    .Gets()
                                    .Where(
                                        e => e.ConfigCode.Equals("CONFIG_EMAIL")
                                     ).ToList();

            MailMessage msg = new MailMessage();
            if (o.Count > 0)
            {
                msg = new MailMessage()
                {
                    smtpHost = o.FirstOrDefault(e => e.ConfigKey.Equals("SMTP_HOST")).ConfigValue,
                    smtpPort = Convert.ToInt32(o.FirstOrDefault(e => e.ConfigKey.Equals("SMTP_PORT")).ConfigValue),
                    useSSL = o.FirstOrDefault(e => e.ConfigKey.Equals("SMTP_USESSL")).ConfigValue.Equals("Y"),
                    emailAddress = o.FirstOrDefault(e => e.ConfigKey.Equals("EMAIL_USER")).ConfigValue,
                    emailPassword = o.FirstOrDefault(e => e.ConfigKey.Equals("EMAIL_PASSWORD")).ConfigValue
                };
            }
            return msg;
        }
    }
    public class MailMessage
    {
        public string emailAddress { get; set; }
        public string emailPassword { get; set; }
        public int smtpPort { get; set; }
        public string smtpHost { get; set; }
        public string pop3Host { get; set; }
        public int pop3Port { get; set; }
        public bool useSSL { get; set; }
        public string fromPerson { get; set; }
        public string toPerson { get; set; }
        public string fromEmail { get; set; }
        public string toEmail { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
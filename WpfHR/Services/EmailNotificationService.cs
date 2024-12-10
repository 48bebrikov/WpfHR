using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using WpfHR.Services;
using WpfHR.Models;
using dotenv.net;
using System.IO;

namespace WpfHR.Services
{
    public class NotificationService
    {
        private readonly PdfService _pdfService;

        public NotificationService(PdfService pdfService)
        {
            _pdfService = pdfService;
            DotEnv.Load();
        }

        public void SendNotification(Module module, string message)
        {
            if (module == null)
            {
                Console.WriteLine("Module cannot be null.");
                return;
            }

            try
            {
                var smtpClient = new SmtpClient(Environment.GetEnvironmentVariable("SMTP_SERVER"))
                {
                    Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")),
                    Credentials = new NetworkCredential(
                        Environment.GetEnvironmentVariable("SMTP_EMAIL"),
                        Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                    ),
                    EnableSsl = true,
                };

                foreach (var recipient in module.Developers.Concat(module.Approvers))
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_EMAIL")),
                        Subject = $"Новый адаптационный модуль: {module.CodeName}",
                        Body = message,
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(recipient);

                    var tempPdfPath = Path.Combine(Path.GetTempPath(), $"{module.CodeName}_Order.pdf");

                    _pdfService.GeneratePdfOrder(module, tempPdfPath);

                    if (File.Exists(tempPdfPath))
                    {
                        mailMessage.Attachments.Add(new Attachment(tempPdfPath));
                    }

                    smtpClient.Send(mailMessage);

                    if (File.Exists(tempPdfPath))
                    {
                        File.Delete(tempPdfPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке уведомления: {ex.Message}");
            }
        }
    }
}

using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using WpfHR.Services;
using WpfHR.Models;
using System.IO;

namespace WpfHR.Services
{
    public class NotificationService
    {
        private readonly PdfService _pdfService;

        public NotificationService(PdfService pdfService)
        {
            _pdfService = pdfService;
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
                var smtpClient = new SmtpClient("smtp.your-email-provider.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
                    EnableSsl = true,
                };

                foreach (var recipient in module.Developers.Concat(module.Approvers))
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("your-email@example.com"),
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

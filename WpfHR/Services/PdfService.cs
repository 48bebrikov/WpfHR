using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WpfHR.Models;

namespace WpfHR.Services
{
    public class PdfService
    {
        public void GeneratePdfOrder(Module module, string filePath)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module), "Module cannot be null.");
            }

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var document = new Document();
                    // skip exception
                    using (var writer = PdfWriter.GetInstance(document, fs))
                    {
                        document.Open();

                        document.Add(new Paragraph($"Order for creating module {module.Name}"));
                        document.Add(new Paragraph($"Code name: {module.CodeName}"));
                        document.Add(new Paragraph($"Developers: {string.Join(", ", module.Developers)}"));
                        document.Add(new Paragraph($"Approvers: {string.Join(", ", module.Approvers)}"));
                        document.Add(new Paragraph($"Main approver: {module.MainApprover}"));
                        document.Add(new Paragraph($"Deadline: {module.Deadline?.ToShortDateString() ?? "Not specified"}"));
                        document.Add(new Paragraph($"Text: {module.CustomMessage}"));

                        // skip exception
                        document.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании PDF: {ex.Message}", ex);
            }
        }
    }
}

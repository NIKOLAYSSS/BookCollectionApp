using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Aspose.Words;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BookCollectionApp
{
    public class BookConverter
    {
        // Конвертировать PDF в DOCX
        public void ConvertPdfToDocx(string pdfPath, string docxPath)
        {
            try
            {
                // Извлекаем текст из PDF с помощью iTextSharp
                string extractedText = ExtractTextFromPdf(pdfPath);

                // Создаем новый документ для Aspose.Words
                Aspose.Words.Document doc = new Aspose.Words.Document();
                Aspose.Words.Body body = doc.FirstSection.Body;

                // Добавляем извлеченный текст в документ Word
                Aspose.Words.Paragraph paragraph = new Aspose.Words.Paragraph(doc);
                paragraph.AppendChild(new Aspose.Words.Run(doc, extractedText));
                body.AppendChild(paragraph);

                // Сохраняем результат в файл DOCX
                doc.Save(docxPath);
                MessageBox.Show("Конвертация PDF в DOCX завершена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при конвертации PDF в DOCX: {ex.Message}");
            }
        }

        // Извлекаем текст из PDF с помощью iTextSharp
        private string ExtractTextFromPdf(string pdfPath)
        {
            StringBuilder text = new StringBuilder();

            try
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfPath));
                for (int pageIndex = 1; pageIndex <= pdfDoc.GetNumberOfPages(); pageIndex++)
                {
                    var page = pdfDoc.GetPage(pageIndex);
                    var strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy();
                    iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page, strategy);
                    text.Append(strategy.GetResultantText());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при извлечении текста из PDF: {ex.Message}");
            }

            return text.ToString();
        }

        // Конвертировать DOCX в PDF
        public void ConvertDocxToPdf(string docxPath, string pdfPath)
        {
            try
            {
                // Открываем DOCX файл с помощью Aspose.Words
                Aspose.Words.Document doc = new Aspose.Words.Document(docxPath);

                // Конвертируем в PDF с помощью Aspose.Words
                doc.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
                MessageBox.Show("Конвертация DOCX в PDF завершена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при конвертации DOCX в PDF: {ex.Message}");
            }
        }
    }
}


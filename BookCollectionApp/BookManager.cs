using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Npgsql;
using Newtonsoft.Json;
using Aspose.Pdf;
using Aspose.Words;
using System.Linq;
using System.Windows.Forms;

namespace BookCollectionApp
{
    public class BookManager
    {
        public static string connectionString = "Host=localhost;Username=postgres;Password=1337;Database=bookmanager_db;";


        public bool IsPdfFile(Book book)
        {
            return book.FileData != null && book.FileData.Length > 4 && book.FileData.Take(4).SequenceEqual(new byte[] { 0x25, 0x50, 0x44, 0x46 }); // PDF Header: "%PDF"
        }

        public bool IsDocxFile(Book book)
        {
            return book.FileData != null && book.FileData.Length > 4 && book.FileData.Take(4).SequenceEqual(new byte[] { 0x50, 0x4B, 0x03, 0x04 }); // DOCX Header: "PK.."
        }

        public void ConvertFile(Book selectedBook, string targetFormat, string outputFilePath)
        {
            // Сохраняем временный файл
            string tempFilePath = Path.GetTempFileName();

            // Записываем данные файла во временный файл
            File.WriteAllBytes(tempFilePath, selectedBook.FileData);

            // Если пользователь выбрал PDF
            if (targetFormat == ".pdf" && IsDocxFile(selectedBook))
            {
                // Конвертируем DOCX в PDF с помощью Aspose.Words
                Aspose.Words.Document doc = new Aspose.Words.Document(tempFilePath);
                doc.Save(outputFilePath, Aspose.Words.SaveFormat.Pdf);
            }
            // Если пользователь выбрал DOCX
            else if (targetFormat == ".docx" && IsPdfFile(selectedBook))
            {
                // Конвертируем PDF в DOCX с помощью Aspose.PDF и Aspose.Words
                Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(tempFilePath);
                pdfDoc.Save(tempFilePath + ".docx", Aspose.Pdf.SaveFormat.DocX);

                // Открываем полученный DOCX и сохраняем в файл
                Aspose.Words.Document wordDoc = new Aspose.Words.Document(tempFilePath + ".docx");
                wordDoc.Save(outputFilePath, Aspose.Words.SaveFormat.Docx);

                // Удаляем временные файлы
                File.Delete(tempFilePath + ".docx");
            }

            // Удаляем временный файл
            File.Delete(tempFilePath);

            MessageBox.Show("Конвертация завершена!");
        }
        public Book GetBookById(Guid bookId)
        {
            Book book = null;
            string query = "SELECT id, title, author, year, file_data FROM books WHERE id = @id";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", bookId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            book = new Book
                            {
                                Id = reader.GetGuid(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                Year = reader.GetInt32(3),
                                FileData = (byte[])reader["file_data"]
                            };
                        }
                    }
                }
            }

            return book;
        }

    }
}

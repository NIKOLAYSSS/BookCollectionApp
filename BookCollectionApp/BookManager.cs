using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace BookCollectionApp
{
    public class BookManager
    {
        // Коллекция для хранения книг. Это приватное поле, доступное только внутри этого класса.
        private readonly List<Book> books = new List<Book>();

        // Метод для добавления новой книги в коллекцию.
        // Добавление новой книги
        public void AddBook(string title, string author, int year, string filePath = "")
        {
            var book = new Book(title, author, year, filePath);
            books.Add(book);
        }

        // Импорт книг из JSON-файла.
        public void ImportBooksFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var importedBooks = JsonSerializer.Deserialize<List<Book>>(json);
                if (importedBooks != null)
                {
                    books.AddRange(importedBooks);
                }
            }
        }

        // Экспорт книг в JSON-файл.
        public void ExportBooksToFile(string filePath)
        {
            // Обновляем путь к файлу для каждой книги перед экспортом
            foreach (var book in books)
            {
                if (string.IsNullOrEmpty(book.FilePath))  // Если путь не установлен
                {
                    book.FilePath = filePath;  // Используем путь сохранения файла как путь книги
                }
            }
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Импорт из другой коллекции книг.
        public void ImportBooksFromCollection(IEnumerable<Book> otherBooks)
        {
            books.AddRange(otherBooks);
        }

        // Экспорт в другую коллекцию книг.
        public List<Book> ExportBooksToCollection()
        {
            return new List<Book>(books);
        }

        // Метод для удаления книги из коллекции по уникальному идентификатору.
        public bool RemoveBook(Guid id)
        {
            // Ищем книгу по идентификатору. Если книга не найдена, возвращаем null.
            var book = books.FirstOrDefault(b => b.Id == id);
            // Если книга найдена, удаляем её из коллекции и возвращаем true.
            if (book != null)
            {
                books.Remove(book);
                return true;
            }
            // Если книга не найдена, возвращаем false.
            return false;
        }

        // Метод для поиска книг по названию. Используется регистронезависимый поиск.
        public List<Book> FindBookByName(string title)
        {
            // Используем LINQ для поиска книг, название которых содержит искомое значение.
            return books.Where(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Метод для поиска книг по автору. Также используется регистронезависимый поиск.
        public List<Book> FindBookByAuthor(string author)
        {
            // Используем LINQ для поиска книг, автор которых содержит искомое значение.
            return books.Where(b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Метод для получения всех книг из коллекции.
        public List<Book> GetAllBooks()
        {
            // Возвращаем список всех книг.
            return books;
        }
    }
}



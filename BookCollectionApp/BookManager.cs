using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollectionApp
{
    public class BookManager
    {
        // Коллекция для хранения книг. Это приватное поле, доступное только внутри этого класса.
        private readonly List<Book> books = new List<Book>();

        // Метод для добавления новой книги в коллекцию.
        public void AddBook(string title, string author, int year)
        {
            // Создание новой книги с переданными параметрами.
            var book = new Book(title, author, year);
            // Добавление книги в коллекцию.
            books.Add(book);
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



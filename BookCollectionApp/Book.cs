using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollectionApp
{
    public class Book
    {
        // Свойство для уникального идентификатора книги (Guid).
        // Оно доступно только для чтения (private set), чтобы можно было установить значение только внутри класса.
        public Guid Id { get; private set; }

        // Свойство для названия книги.
        public string Title { get; set; }

        // Свойство для автора книги.
        public string Author { get; set; }

        // Свойство для года издания книги.
        public int Year { get; set; }

        // Конструктор класса Book, который инициализирует книгу с переданными параметрами: название, автор и год издания.
        public Book(string title, string author, int year)
        {
            // Генерация уникального идентификатора (GUID) для каждой книги при ее создании.
            Id = Guid.NewGuid();

            // Инициализация других свойств книги: название, автор и год издания.
            Title = title;
            Author = author;
            Year = year;
        }

        // Переопределение метода ToString(), который будет возвращать строковое представление книги.
        // Это полезно для вывода информации о книге в консоль или другие элементы интерфейса.
        public override string ToString()
        {
            // Формирование строки с информацией о книге в виде: Id, Название, Автор и Год издания.
            return $"Id: {Id}; Название: {Title}; Автор: {Author}; Год издания: {Year}";
        }
    }
}

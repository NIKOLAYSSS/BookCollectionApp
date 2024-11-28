using System;
using System.IO;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string FilePath { get; set; } // Путь к файлу книги.



    public Book(string title, string author, int year, string filePath)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Year = year;
        FilePath = filePath;
    }

    public override string ToString()
    {
        return $"Id: {Id}; Название: {Title}; Автор: {Author}; Год издания: {Year}";
    }

}

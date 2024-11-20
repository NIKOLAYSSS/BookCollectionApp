using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookCollectionApp
{
    public partial class Form1 : Form
    {
        // Создание экземпляра менеджера книг для управления коллекцией книг.
        private BookManager bookManager = new BookManager();

        // Создание источника данных для связывания данных с элементами управления (например, DataGridView).
        private BindingSource bindingSource = new BindingSource();

        // Конструктор формы, который выполняет инициализацию компонентов и связывает DataGridView с BindingSource.
        public Form1()
        {
            InitializeComponent();  // Инициализация компонентов формы (автоматически генерируемый код)

            // Устанавливаем BindingSource в качестве источника данных для DataGridView.
            dataGridBooks.DataSource = bindingSource;

            // Автоматически генерировать столбцы в DataGridView на основе свойств объекта книги.
            dataGridBooks.AutoGenerateColumns = true;
        }

        // Обработчик события для добавления новой книги.
        private void btnAddBook_Click(object sender, EventArgs e)
        {
            // Получение данных из текстовых полей для названия, автора и года.
            string title = txtTitle.Text;
            string author = txtAuthor.Text;

            // Проверка, является ли введенное значение года допустимым числом.
            if (int.TryParse(txtYear.Text, out int year))
            {
                // Добавление новой книги в коллекцию через менеджер книг.
                bookManager.AddBook(title, author, year);

                // Показать сообщение об успешном добавлении книги.
                MessageBox.Show("Книга успешно добавлена.");

                // Очистить текстовые поля после добавления книги.
                ClearInputs();

                // Обновить отображение всех книг в DataGridView.
                DisplayBooks(bookManager.GetAllBooks());
            }
            else
            {
                // Если год некорректный, показать сообщение об ошибке.
                MessageBox.Show("Введен некорректный год.");
            }
        }

        // Обработчик события для удаления книги.
        private void btnRemoveBook_Click(object sender, EventArgs e)
        {
            // Получение выбранной строки в DataGridView.
            var selectedBook = dataGridBooks.SelectedRows[0].DataBoundItem as Book;

            // Проверка, была ли выбрана книга.
            if (selectedBook != null)
            {
                // Удаление книги из коллекции по уникальному идентификатору.
                bookManager.RemoveBook(selectedBook.Id);

                // Показать сообщение об успешном удалении книги.
                MessageBox.Show("Книга успешно удалена.");

                // Обновить отображение всех книг в DataGridView.
                DisplayBooks(bookManager.GetAllBooks());
            }
            else
            {
                // Если книга не выбрана, показать сообщение о необходимости выбора книги.
                MessageBox.Show("Выберите книгу для удаления.");
            }
        }

        // Обработчик события для поиска книги по названию.
        private void btnSearchByTitle_Click(object sender, EventArgs e)
        {
            // Получение текста для поиска по названию.
            string title = txtTitle.Text;

            // Получение списка книг, соответствующих введенному названию.
            var results = bookManager.FindBookByName(title);

            // Отображение найденных книг в DataGridView.
            DisplayBooks(results);
        }

        // Обработчик события для поиска книги по автору.
        private void btnSearchByAuthor_Click(object sender, EventArgs e)
        {
            // Получение текста для поиска по автору.
            string author = txtAuthor.Text;

            // Получение списка книг, соответствующих введенному автору.
            var results = bookManager.FindBookByAuthor(author);

            // Отображение найденных книг в DataGridView.
            DisplayBooks(results);
        }

        // Обработчик события для отображения всех книг.
        private void btnShowAllBooks_Click(object sender, EventArgs e)
        {
            // Отображение всех книг в DataGridView.
            DisplayBooks(bookManager.GetAllBooks());
        }

        // Метод для отображения списка книг в DataGridView.
        private void DisplayBooks(List<Book> books)
        {
            bindingSource.Clear(); // Очищаем текущие данные в BindingSource

            // Перебираем все книги в списке и добавляем их в BindingSource.
            foreach (var book in books)
            {
                bindingSource.Add(book); // Добавляем каждую книгу в источник данных.
            }
        }

        // Метод для очистки текстовых полей на форме.
        private void ClearInputs()
        {
            txtTitle.Clear(); // Очищаем поле для ввода названия книги.
            txtAuthor.Clear(); // Очищаем поле для ввода автора книги.
            txtYear.Clear(); // Очищаем поле для ввода года книги.
        }
    }
}





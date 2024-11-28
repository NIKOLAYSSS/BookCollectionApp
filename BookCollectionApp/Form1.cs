using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

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
                // Открытие диалога выбора файла
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Получаем путь к выбранному файлу
                        string filePath = openFileDialog.FileName;

                        // Добавляем книгу в коллекцию
                        bookManager.AddBook(title, author, year, filePath);

                        // Показать сообщение об успешном добавлении книги
                        MessageBox.Show("Книга успешно добавлена.");

                        // Очистить текстовые поля
                        ClearInputs();

                        // Обновить отображение всех книг в DataGridView
                        DisplayBooks(bookManager.GetAllBooks());
                    }
                }
            }
            else
            {
                // Если год некорректный, показать сообщение об ошибке
                MessageBox.Show("Введен некорректный год.");
            }
        }

        // Обработчик события для удаления книги.
        private void btnRemoveBook_Click(object sender, EventArgs e)
        {
            // Получение выбранной строки в DataGridView.
            var selectedBook = dataGridBooks.CurrentRow.DataBoundItem as Book;

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

        private void btnImportBooks_Click(object sender, EventArgs e)
        {
            //if (authManager.GetLoggedInUser() == null)
            //{
            //    MessageBox.Show("Пожалуйста, войдите в систему.");
            //    return;
            //}

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Выберите файл для импорта книг"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                bookManager.ImportBooksFromFile(filePath);
                DisplayBooks(bookManager.GetAllBooks());
            }
        }

        // Кнопка для экспорта книг в файл (JSON)
        private void btnExportBooks_Click(object sender, EventArgs e)
        {
            //if (authManager.GetLoggedInUser() == null)
            //{
            //    MessageBox.Show("Пожалуйста, войдите в систему.");
            //    return;
            //}

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Выберите место для сохранения списка книг"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                bookManager.ExportBooksToFile(filePath);
                MessageBox.Show("Экспорт завершен!");
            }
        }

        private void btnDownloadBook_Click(object sender, EventArgs e)
        {
            // Проверка, выбрана ли строка в DataGridView
            if (dataGridBooks.CurrentRow != null)
            {
                // Получение объекта книги из текущей строки
                var selectedBook = dataGridBooks.CurrentRow.DataBoundItem as Book;

                // Проверка, что объект действительно является книгой
                if (selectedBook != null)
                {
                    // Проверка, существует ли файл для книги
                    if (File.Exists(selectedBook.FilePath))
                    {
                        try
                        {
                            // Попытка открыть файл с помощью соответствующей программы
                            Process.Start(selectedBook.FilePath);
                        }
                        catch (Exception ex)
                        {
                            // Обработка ошибок при попытке открыть файл
                            MessageBox.Show("Ошибка при попытке открыть файл: " + ex.Message);
                        }
                    }
                    else
                    {
                        // Если файл не существует
                        MessageBox.Show("Файл книги не найден.");
                    }
                }
                else
                {
                    // Если выбранный элемент не является книгой
                    MessageBox.Show("Ошибка: выбранный элемент не является книгой.");
                }
            }
            else
            {
                // Если нет выбранной строки
                MessageBox.Show("Выберите книгу для загрузки.");
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли книга
            if (dataGridBooks.CurrentRow != null)
            {
                var selectedBook = dataGridBooks.CurrentRow.DataBoundItem as Book;
                if (selectedBook != null)
                {
                    // Открываем диалоговое окно для сохранения нового файла
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    // Проверяем формат файла книги и настраиваем диалог для сохранения
                    if (selectedBook.FilePath.EndsWith(".pdf"))
                    {
                        saveFileDialog.Filter = "Word Documents (*.docx)|*.docx"; // PDF -> DOCX
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Конвертируем из PDF в DOCX
                            BookConverter converter = new BookConverter();
                            converter.ConvertPdfToDocx(selectedBook.FilePath, saveFileDialog.FileName);
                        }
                    }
                    else if (selectedBook.FilePath.EndsWith(".docx"))
                    {
                        saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf"; // DOCX -> PDF
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Конвертируем из DOCX в PDF
                            BookConverter converter = new BookConverter();
                            converter.ConvertDocxToPdf(selectedBook.FilePath, saveFileDialog.FileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите книгу с форматом PDF или DOCX для конвертации.");
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось выбрать книгу для конвертации.");
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для конвертации.");
            }
        }
    }
}





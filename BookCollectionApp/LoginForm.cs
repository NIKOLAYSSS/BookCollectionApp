using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using Npgsql;
using BCrypt.Net;

namespace BookCollectionApp
{
    public partial class LoginForm : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=1337;Database=bookmanager_db;";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Имя пользователя и пароль не могут быть пустыми.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Хэшируем пароль
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                    // Вставляем пользователя в БД
                    string query = "INSERT INTO users (username, password_hash) VALUES (@username, @passwordHash)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("username", username);
                        command.Parameters.AddWithValue("passwordHash", passwordHash);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Регистрация успешна!");
                    }
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Проверка уникальности имени
            {
                MessageBox.Show("Такой пользователь уже существует.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        // Обработчик кнопки входа
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Имя пользователя и пароль не могут быть пустыми.");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Извлекаем хэш пароля из БД
                    string query = "SELECT password_hash FROM users WHERE username = @username";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("username", username);
                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedHash = result.ToString();

                            // Проверяем пароль
                            if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                            {
                                MessageBox.Show("Авторизация успешна!");
                                Form1 mainForm = new Form1(); // Создаём объект главной формы
                                mainForm.Show(); // Открываем главную форму
                                this.Hide(); // Скрываем текущую форму (Form1)
                            }
                            else
                            {
                                MessageBox.Show("Неправильный пароль.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}

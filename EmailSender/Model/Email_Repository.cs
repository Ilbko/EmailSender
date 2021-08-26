using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;

namespace EmailSender.Model
{
    public class Email
    {
        public int Email_Id { get; set; }
        public string Email_Address { get; set; }
    }

    public static class Email_Repository
    {
        public static readonly string connStr = ConfigurationManager.ConnectionStrings["EmailDb"].ConnectionString;

        //Метод для выборки всех записей в таблице
        public static async Task<List<Email>> Select()
        {
            const string procedure = "SELECT * FROM [Email]";

            using (SQLiteConnection db = new SQLiteConnection(connStr))
            {
                db.Open();
                List<Email> emails = new List<Email>();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        emails = (List<Email>)await db.QueryAsync<Email>(procedure, transaction).ConfigureAwait(false);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                return emails;
            }
        }

        //Метод для удаления записи с таблицы
        public static async void Delete(Email value)
        {
            const string procedure = "DELETE FROM [Email] WHERE [Email_Id] = @Email_Id";
            var values = new { Email_Id = value.Email_Id };

            using (SQLiteConnection db = new SQLiteConnection(connStr))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        await db.QueryAsync(procedure, values, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //Метод для добавления записи в таблицу
        public static async void Insert(Email value)
        {
            const string procedure = "INSERT INTO [Email] ([Email_Address]) VALUES (@Email_Address)";
            var values = new { Email_Address = value.Email_Address };

            using (SQLiteConnection db = new SQLiteConnection(connStr))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        await db.QueryAsync(procedure, values, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}

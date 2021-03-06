using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace EmailSender.Model
{
    public static class Logic
    {
        //Название файла базы данных
        public static readonly string dbName = "EmailDB.sqlite3";

        //Метод добавления пути к прикрепляемому файлу
        internal static string AddFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        //Метод отправки сообщения адресатам
        internal static bool StartSend(string addressString, string password, string titleString, string bodyString, ObservableCollection<string> files)
        {
            try
            {
                MailAddress fromAddress = new MailAddress(addressString);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(fromAddress.Address, password),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage()
                {
                    Subject = titleString,
                    Body = bodyString,
                    IsBodyHtml = false,
                    From = fromAddress,
                };

                Email_Repository.Select().Result.ForEach(x => mailMessage.To.Add(new MailAddress(x.Email_Address)));
                files.ToList().ForEach(x => mailMessage.Attachments.Add(new Attachment(x)));

                //При попытке отправить письма асинхронно виснет. Microsoft рекомендует использовать библиотеку третьего лица MailKit вместо System.Net.Mail.
                //smtpClient.SendMailAsync(mailMessage);
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {               
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }

        //Метод инициализации базы данных, если её нет
        internal static async void InitDB(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);

            const string procedure = "CREATE TABLE Email (" +
                "Email_Id      INTEGER PRIMARY KEY AUTOINCREMENT" +
                "                      UNIQUE" +
                "                      NOT NULL," +
                "Email_Address VARCHAR UNIQUE" +
                "                      NOT NULL" +
                "                      CHECK ( (Email_Address LIKE '%@%' AND" +
                "                               Email_Address LIKE '%.%') )" +
                ");";

            using (SQLiteConnection db = new SQLiteConnection(Email_Repository.connStr))
            {
                db.Open();

                //Невозможно выполнить с транзакцией.
                //using (var transaction = db.BeginTransaction())
                //{
                //    try
                //    {
                //        db.Execute(procedure, transaction);
                //        transaction.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        transaction.Rollback();
                //        throw ex;
                //    }
                //}
                try
                {
                    await db.ExecuteAsync(procedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

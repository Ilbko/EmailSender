using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

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
        public static List<Email> Select()
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
                        emails = db.Query<Email>(procedure, transaction).ToList();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

                return emails;
            }
        }

        public static void Delete(Email value)
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
                        db.Query(procedure, values, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}

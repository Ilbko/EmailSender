using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace EmailSender.Model
{
    public static class Logic
    {
        internal static string AddFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

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

                Email_Repository.Select().ForEach(x => mailMessage.To.Add(new MailAddress(x.Email_Address)));
                files.ToList().ForEach(x => mailMessage.Attachments.Add(new Attachment(x)));

                smtpClient.SendAsync(mailMessage, null);

                return true;
            }
            catch (System.Exception ex)
            {               
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }
    }
}

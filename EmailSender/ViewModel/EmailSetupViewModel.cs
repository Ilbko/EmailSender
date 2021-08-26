using EmailSender.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace EmailSender.ViewModel
{
    public class EmailSetupViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Email> Emails { get; set; }

        private Button deleteEmailButton;

        private Email selectedEmail;
        public Email SelectedEmail
        {
            get { return selectedEmail; }
            set 
            { 
                selectedEmail = value; 
                OnPropertyChanged("SelectedEmail");

                if (SelectedEmail != null)
                    deleteEmailButton.IsEnabled = true;
                else
                    deleteEmailButton.IsEnabled = false;
            }
        }

        private string addressString;
        public string AddressString
        {
            get { return addressString; }
            set { addressString = value; OnPropertyChanged("AddressString"); }
        }

        private RelayCommand<TextBox> addEmailCommand;
        public RelayCommand<TextBox> AddEmailCommand
        {
            get 
            {
                return addEmailCommand ?? new RelayCommand<TextBox>(act => 
                {
                    Email_Repository.Insert(new Email() { Email_Address = this.AddressString });
                    UpdateEmails();
                    act.Text = string.Empty;
                });
            }
        }

        private RelayCommand deleteEmailCommand;
        public RelayCommand DeleteEmailCommand
        {
            get
            {
                return deleteEmailCommand ?? new RelayCommand(() =>
                {
                    Email_Repository.Delete(this.SelectedEmail);
                    UpdateEmails();
                });
            }
        }

        private void UpdateEmails()
        {
            this.Emails = new ObservableCollection<Email>(Email_Repository.Select());
            OnPropertyChanged("Emails");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public EmailSetupViewModel(ref Button deleteEmailButton)
        {
            this.deleteEmailButton = deleteEmailButton;

            UpdateEmails();
        }
    }
}

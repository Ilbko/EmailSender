using EmailSender.Model;
using EmailSender.View;
using EmailSender.View.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System;
using System.Configuration;
using System.Reflection;

namespace EmailSender.ViewModel
{
    public class EmailSenderViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Files { get; set; }

        private Button deleteFileButton;

        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
            set 
            { 
                selectedFile = value; 
                OnPropertyChanged("SelectedFile");

                if (SelectedFile != null)
                    this.deleteFileButton.IsEnabled = true;
                else
                    this.deleteFileButton.IsEnabled = false;
            }
        }

        private string titleString;
        public string TitleString
        {
            get { return titleString; }
            set { titleString = value; OnPropertyChanged("TitleString"); }
        }

        private string bodyString;
        public string BodyString
        {
            get { return bodyString; }
            set { bodyString = value; OnPropertyChanged("BodyString"); }
        }

        private string addressString;
        public string AddressString
        {
            get { return addressString; }
            set { addressString = value; OnPropertyChanged("AddressString"); }
        }

        private RelayCommand addFileCommand;
        public RelayCommand AddFileCommand
        {
            get 
            {
                return addFileCommand ?? new RelayCommand(act => 
                {
                    string filePath = Logic.AddFile();

                    if (filePath != null)
                        this.Files.Add(filePath);
                });
            }
        }

        private RelayCommand deleteFileCommand;
        public RelayCommand DeleteFileCommand
        {
            get
            {
                return deleteFileCommand ?? new RelayCommand(act =>
                {
                    this.Files.Remove(SelectedFile);
                });
            }
        }

        private RelayCommand emailSetupCommand;
        public RelayCommand EmailSetupCommand
        {
            get
            {
                return emailSetupCommand ?? new RelayCommand(act =>
                {
                    new EmailSetupWindow().ShowDialog();
                });
            }
        }

        private GalaSoft.MvvmLight.Command.RelayCommand<PasswordBox> startCommand;
        public GalaSoft.MvvmLight.Command.RelayCommand<PasswordBox> StartCommand
        {
            get
            {
                return startCommand ?? new GalaSoft.MvvmLight.Command.RelayCommand<PasswordBox>(act =>
                {
                    if (Logic.StartSend(this.AddressString, act.Password, this.TitleString, this.BodyString, this.Files))
                        MessageBox.Show("Отправка успешна!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void SetConnectionString(string dbPath)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["EmailDB"];
            
            FieldInfo fieldInfo = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(settings, false);

            settings.ConnectionString = $"Data Source = {dbPath}; Version = 3;";
        }

        public EmailSenderViewModel(ref Button deleteFileButton)
        {
            this.Files = new ObservableCollection<string>();

            this.deleteFileButton = deleteFileButton;

            this.TitleString = "Title";
            this.BodyString = "Body";

            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Logic.dbName;

            SetConnectionString(dbPath);
            //ConfigurationManager.ConnectionStrings["EmailDB"].ConnectionString = $"Data Source = {dbPath}; Version = 3;";
            
            if (!File.Exists(dbPath))
                Logic.InitDB(dbPath);
        }
    }
}

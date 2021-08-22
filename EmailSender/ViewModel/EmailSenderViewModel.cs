﻿using EmailSender.View.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EmailSender.ViewModel
{
    public class EmailSenderViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Files { get; set; }

        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
            set { selectedFile = value; OnPropertyChanged("SelectedFile"); }
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

                });
            }
        }

        private RelayCommand startCommand;
        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ?? new RelayCommand(act =>
                {

                });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public EmailSenderViewModel()
        {
            this.TitleString = "Title";
            this.BodyString = "Body";
        }
    }
}

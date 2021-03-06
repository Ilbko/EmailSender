using EmailSender.ViewModel;
using System;
using System.Windows;

namespace EmailSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new EmailSenderViewModel(ref deleteFileButton);

            string themeName = "AppTheme";

            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add
                (Application.LoadComponent(new Uri(@"View/" + themeName + ".xaml", UriKind.Relative)) as ResourceDictionary);
        }
    }
}

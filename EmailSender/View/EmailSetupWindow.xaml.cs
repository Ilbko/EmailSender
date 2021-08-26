using EmailSender.ViewModel;
using System;
using System.Windows;

namespace EmailSender.View
{
    /// <summary>
    /// Логика взаимодействия для EmailSetupWindow.xaml
    /// </summary>
    public partial class EmailSetupWindow : Window
    {
        public EmailSetupWindow()
        {
            InitializeComponent();
            this.DataContext = new EmailSetupViewModel(ref deleteEmailButton);

            string themeName = "AppTheme";

            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add
                (Application.LoadComponent(new Uri(@"View/" + themeName + ".xaml", UriKind.Relative)) as ResourceDictionary);
        }
    }
}

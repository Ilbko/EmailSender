using EmailSender.ViewModel;
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
            this.DataContext = new EmailSenderViewModel();
        }
    }
}

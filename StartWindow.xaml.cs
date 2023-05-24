using System.Windows;
using Homework_11.Providers;

namespace Homework_11
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void OnButton_Сonsultant(object sender, RoutedEventArgs e)
        {
            new MainWindow(new User(Users.Consultant)).Show();
            Close();
        }

        private void OnButton_Manager(object sender, RoutedEventArgs e)
        {
            new MainWindow(new User(Users.Manager)).Show();
            Close();
        }
    }
}

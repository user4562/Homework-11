using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Homework_11.Organization;
using Homework_11.Professions;
using Homework_11.Providers;
using Homework_11.Data;

namespace Homework_11
{
    public partial class MainWindow : Window
    {
        DataProvider provider;
        public MainWindow(User user)
        {
            var boss = new Director()
            {
                Name = new Name("Иван", "Бурилов", "Васильевич"),
                PhoneNumber = new PhoneNumber("+0 111 222 33 44"),
                PassportNumber = new PassportNumber("0000 111222"),
                Age = 50
            };

            var company = new Company("Company 1", boss);

            provider = new DataProvider(this, company, user);

            DataContext = provider;

            InitializeComponent();
            SetViewForUser();

            NewWorkerProfession.SelectedIndex = 0;
        }

        public void SetViewForUser()
        {
            if (!provider.CurrentUser.Add)
            {
                ButtonAddWorker.Visibility = Visibility.Collapsed;
                ButtonAddDep.Visibility = Visibility.Collapsed;
            }

            if (!provider.CurrentUser.Remove)
            {
                ButtonRemoveWorker.Visibility = Visibility.Collapsed;
                ButtonRemoveDep.Visibility = Visibility.Collapsed;
            }

            if (provider.CurrentUser.ChangeOnlyPhone)
            {
                NewWorkerLastName.IsEnabled = false;
                NewWorkerFirstName.IsEnabled = false;
                NewWorkerPatronymic.IsEnabled = false;
                NewWorkerAge.IsEnabled = false;
                NewWorkerProfession.IsEnabled = false;

                DepartamentNameInput.IsEnabled = false;
            }

            if (!provider.CurrentUser.WatchPassport)
            {
                Binding binding = new Binding("HidePassportNumber");
                PassportNumberColumn.DisplayMemberBinding = binding;
            }
        }

        private void OnDepartamentSelected(object sender, RoutedEventArgs e)
        {
            if ((sender as TreeView).SelectedItem is Departament departament)
            {
                provider.CurrentDepartament = departament;
            }
        }

        private void OnWorkerSelected(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item && item.Content != null)
            {
                provider.CurrentWorker = item.Content as Worker;
                provider.CurrentWorkerDep = provider.CurrentDepartament;
                provider.ErrorsToOK();
            }
        }

        private void OnCreateDepartament(object sender, RoutedEventArgs e)
        {
            provider.CreateDepartament();
        }

        private void OnChengeDepartament(object sender, RoutedEventArgs e)
        {
            provider.ChangeDepartament();
        }

        private void OnRemoveDepartament(object sender, RoutedEventArgs e)
        {
            provider.RemoveDepartament();
        }

        private void OnDoneDepartament(object sender, RoutedEventArgs e)
        {
            provider.DoneDepartament();
        }

        private void OnCancelDepartament(object sender, RoutedEventArgs e)
        {
            provider.CancelDepartament();
        }

        private void OnCreateWorker(object sender, RoutedEventArgs e)
        {
            provider.CreateWorker();
        }

        private void OnChangeWorker(object sender, RoutedEventArgs e)
        {
            provider.ChangeWorker();
        }

        private void OnRemoveWorker(object sender, RoutedEventArgs e)
        {
            provider.RemoveWorker();
        }
        
        private void OnDoneWorker(object sender, RoutedEventArgs e)
        {
            provider.DoneWorker();
        }

        private void OnCancelWorker(object sender, RoutedEventArgs e)
        {
            provider.CancelWorker();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            provider.InputTextChanged();
        }
    }
}

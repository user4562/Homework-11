using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

using Homework_11.Organization;
using Homework_11.Professions;
using Homework_11.Data;

namespace Homework_11.Providers
{
    internal class DataProvider : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User CurrentUser { get; set; }

        private Company _company;
        public Company Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged();
            }
        }

        private Departament _departament;
        public Departament CurrentDepartament
        {
            get { return _departament; }
            set
            {
                _departament = value;
                OnPropertyChanged();
            }
        }

        public Departament CurrentWorkerDep { get; set; }

        private Worker _worker;
        public Worker CurrentWorker
        {
            get { return _worker; }
            set
            {
                _worker = value;
                OnPropertyChanged();
            }
        }

        private string _errorText;
        public string CurrentErrorText 
        {
            get { return _errorText; }
            set 
            {
                _errorText = value;
                OnPropertyChanged();
            } 
        }

        private string _workerErrorText;
        public string CurrentWorkerErrorText
        { 
            get { return _workerErrorText; }
            set
            {
                _workerErrorText = value;
                OnPropertyChanged();
            }
        }

        private Brush _errorColor;
        public Brush CurrentErrorColor 
        {
            get { return _errorColor; } 
            set
            {
                _errorColor = value;
                OnPropertyChanged();
            }
        }

        private ViewMode _viewMode;
        public ViewMode CurrentViewMode 
        { 
            get { return _viewMode; }
            set
            {
                _viewMode = value;
                OnPropertyChanged();
            }
        }

        private MainWindow WindowContext;
        public DataProvider(MainWindow windowContext, Company company, User user) 
        {
            WindowContext = windowContext;
            Company = company;
            CurrentUser = user;

            CurrentDepartament = Company.MainDepartament;
            CurrentWorker = null;

            CurrentViewMode = new ViewMode();

            CurrentErrorText = Errors.TextOK;
            CurrentErrorColor = Errors.ColorOK;
            CurrentWorkerErrorText = string.Empty;
        }

        public void CreateDepartament()
        {
            CurrentViewMode = new ViewMode(ViewModeOptions.CreateDepartament);
            ErrorsToOK();
        }

        private bool _isChange = false;
        public void ChangeDepartament()
        {
            _isChange = true;
            CurrentViewMode = new ViewMode(ViewModeOptions.CreateDepartament);
            ErrorsToOK();

            WindowContext.DepartamentNameInput.Text = CurrentDepartament.Name;

            if (CurrentDepartament.Name == Company.MainDepartament.Name)
            {
                WindowContext.DepartamentNameInput.IsEnabled = false;
            }

            SetWorkerBox(CurrentDepartament.Boss);
        }

        private void SetWorkerBox(Worker worker)
        {
            WindowContext.NewWorkerLastName.Text = worker.Name.LastName;
            WindowContext.NewWorkerFirstName.Text = worker.Name.FirstName;
            WindowContext.NewWorkerPatronymic.Text = worker.Name.Patronymic;
            WindowContext.NewWorkerAge.Text = worker.Age.ToString();
            WindowContext.NewWorkerPhone.Text = worker.PhoneNumber.Text;

            if (CurrentUser.WatchPassport)
            {
                WindowContext.NewWorkerPassport.Text = worker.PassportNumber.Text;
            }
            else
            {
                WindowContext.NewWorkerPassport.Text = "** ** ******";
                WindowContext.NewWorkerPassport.IsEnabled = false;
            }

            if (CurrentWorker == Company.MainDepartament.Boss)
            {
                WindowContext.NewWorkerProfession.IsEnabled = false;
                WindowContext.NewWorkerProfession.SelectedIndex = -1;
            }
            else
            {
                SetProfessions(worker);
            }
        }

        private void SetProfessions(Worker worker)
        {
            int professionIndex;
            switch (worker.Profession)
            {
                case "Менеджер": professionIndex = 0; break;
                case "Программист": professionIndex = 1; break;
                case "Охранник": professionIndex = 2; break;
                case "Бухгалтер": professionIndex = 3; break;
                case "Аналитик": professionIndex = 4; break;
                default: professionIndex = -1; break;
            }

            WindowContext.NewWorkerProfession.SelectedIndex = professionIndex;
        }

        private bool _doneDepPress = false;
        public void DoneDepartament()
        {
            _doneDepPress = true;

            string errorText = GetErrorText();

            if (errorText != string.Empty)
            {
                ToMarkErrorInputs();
                CurrentErrorText = errorText;
                CurrentErrorColor = Errors.ColorError;
                SetErrorMessageText();        
            }
            else
            {
                Worker boss = CreateNewWorker();
                string name = WindowContext.DepartamentNameInput.Text;

                string message;
                if (_isChange)
                {
                    CurrentDepartament.Boss = boss;
                    CurrentDepartament.Name = name;

                    message = "Департамент изменен";
                }
                else
                {            
                    CurrentDepartament.Add(new Departament(name, boss));
                    message = "Создан новый департамент";
                }

                Refresh();

                _doneDepPress = false;
                ToDefaultState(message);
                Company.Save();
            }
        }

        public void RemoveDepartament()
        {
            if (CurrentDepartament.Name == Company.MainDepartament.Name)
            {
                CurrentErrorText = $"{Company.MainDepartament.Name} не может быть удален";
                CurrentErrorColor = Errors.ColorWarning;
                return;
            }

            if (Company.RemoveDepartament(CurrentDepartament))
            {
                CurrentDepartament = Company.MainDepartament;
                CurrentErrorText = "Департамен удален";
            }

            CurrentErrorColor = Errors.ColorOK;

            Refresh();
            Company.Save();
        }

        public void CancelDepartament()
        {
            ToDefaultState();
        }

        public void CreateWorker()
        {
            CurrentViewMode = new ViewMode(ViewModeOptions.CreateWorker);
            ErrorsToOK();
        }

        public void ChangeWorker()
        {
            if(CurrentWorker == null)
            {
                CurrentErrorText = "Рабочий не выбран";
                CurrentErrorColor = Errors.ColorWarning;
                return;
            }
            else
            {
                CurrentViewMode = new ViewMode(ViewModeOptions.CreateWorker);
                SetWorkerBox(CurrentWorker);
                _isChange = true;
            }
        }

        public void RemoveWorker()
        {
            if (CurrentWorker == null)
            {
                CurrentErrorText = "Рабочий не выбран";
                CurrentErrorColor = Errors.ColorWarning;
                return;
            }

            if (CurrentDepartament.Boss == CurrentWorker)
            {
                CurrentErrorText = "Начальник не может быть удален";
                CurrentErrorColor = Errors.ColorWarning;
                return;
            }

            CurrentDepartament.Remove(CurrentWorker);
            CurrentErrorText = "Рабочий удален";
            CurrentErrorColor = Errors.ColorOK;
            CurrentWorker = null;

            Refresh();
            Company.Save();
        }

        private void Refresh()
        {
            CurrentDepartament = CurrentDepartament;
            Company.MainDepartament = Company.MainDepartament;
            Company = Company;
        }

        private bool _doneWorkerPress = false;
        public void DoneWorker()
        {
            _doneWorkerPress = true;

            string errorText = GetErrorText();

            if (errorText != string.Empty)
            {
                ToMarkErrorInputs();
                CurrentErrorText = errorText;
                CurrentErrorColor = Errors.ColorError;
                SetErrorMessageText();
            }
            else
            {
                Worker newWorker = CreateNewWorker();
                string message;
                if (_isChange)
                {
                    int index = CurrentWorkerDep.Workers.IndexOf(CurrentWorker);

                    CurrentWorkerDep.Workers.RemoveAt(index);
                    CurrentWorker = newWorker;
                    CurrentWorkerDep.Workers.Insert(index, CurrentWorker);

                    message = "Рабочий изменен";
                }
                else
                {
                    CurrentWorkerDep.Add(newWorker);
                    message = "Добавлен новый рабочий";
                }

                _doneWorkerPress = false;
                ToDefaultState(message);

                Refresh();
                Company.Save();
            }
        }

        public void CancelWorker()
        {
            ToDefaultState();
        }

        private void CleanWorkerInput()
        {
            WindowContext.NewWorkerLastName.Text = string.Empty;
            WindowContext.NewWorkerFirstName.Text = string.Empty;
            WindowContext.NewWorkerPatronymic.Text = string.Empty;
            WindowContext.NewWorkerAge.Text = string.Empty;
            WindowContext.NewWorkerPhone.Text = string.Empty;
            WindowContext.NewWorkerPassport.Text = string.Empty;
            WindowContext.NewWorkerProfession.SelectedIndex = 0;
        }

        private string GetErrorText()
        {
            string last = WindowContext.NewWorkerLastName.Text;
            string first = WindowContext.NewWorkerFirstName.Text;
            string patronymic = WindowContext.NewWorkerPatronymic.Text;
            string age = WindowContext.NewWorkerAge.Text;
            string phone = WindowContext.NewWorkerPhone.Text;
            string passport = WindowContext.NewWorkerPassport.Text;

            if (_doneDepPress && WindowContext.DepartamentNameInput.Text == string.Empty)
                return "Название департамента не может быть пустое";

            if (!Name.IsValid(last)) 
                return Errors.GetTextFromError(Name.GetNameError(last));
            if (!Name.IsValid(first))
                return Errors.GetTextFromError(Name.GetNameError(first));
            if (!Name.IsValid(patronymic))
                return Errors.GetTextFromError(Name.GetNameError(patronymic));

            if (!IsAgeValid(age))
                return GetErrorAgeText(age);

            if (!PhoneNumber.IsValid(phone))
                return Errors.GetTextFromError(PhoneNumber.GetPhoneNumberError(phone));

            if (CurrentUser.WatchPassport && !PassportNumber.IsValid(passport))
                return Errors.GetTextFromError(PassportNumber.GetPassportNumberError(passport));

            return string.Empty;
        }

        private bool IsAgeValid(string age)
        {
            return GetErrorAgeText(age) == string.Empty;
        }

        private string GetErrorAgeText(string age)
        {
            int numAge; 

            if (age == null || age.Length == 0) return "Возраст не указан";
            if (!int.TryParse(age, out numAge)) return "Возраст должен быть числом";
            if (numAge < 18) return "Возраст слишком мал";
            if (numAge > 100) return "Возраст слишком большой";

            return string.Empty;
        }

        private void ToMarkErrorInputs()
        {
            string depName = WindowContext.DepartamentNameInput.Text;
            string last = WindowContext.NewWorkerLastName.Text;
            string first = WindowContext.NewWorkerFirstName.Text;
            string patronymic = WindowContext.NewWorkerPatronymic.Text;
            string age = WindowContext.NewWorkerAge.Text;
            string phone = WindowContext.NewWorkerPhone.Text;
            string passport = WindowContext.NewWorkerPassport.Text;

            if (depName.Length == 0) WindowContext.DepartamentNameInput.BorderBrush = Errors.ColorError;
            else WindowContext.DepartamentNameInput.BorderBrush = Errors.ColorInputDefault;

            if (!Name.IsValid(last)) WindowContext.NewWorkerLastName.BorderBrush = Errors.ColorError;
            else WindowContext.NewWorkerLastName.BorderBrush = Errors.ColorInputDefault;

            if (!Name.IsValid(first)) WindowContext.NewWorkerFirstName.BorderBrush = Errors.ColorError;
            else WindowContext.NewWorkerFirstName.BorderBrush = Errors.ColorInputDefault;

            if (!Name.IsValid(patronymic)) WindowContext.NewWorkerPatronymic.BorderBrush = Errors.ColorError;
            else WindowContext.NewWorkerPatronymic.BorderBrush = Errors.ColorInputDefault;

            if (!IsAgeValid(age)) WindowContext.NewWorkerAge.BorderBrush = Errors.ColorError;
            else WindowContext.NewWorkerAge.BorderBrush = Errors.ColorInputDefault;

            if (!PhoneNumber.IsValid(phone)) WindowContext.NewWorkerPhone.BorderBrush = Errors.ColorError;
            else WindowContext.NewWorkerPhone.BorderBrush = Errors.ColorInputDefault;

            if(CurrentUser.WatchPassport)
            {
                if (!PassportNumber.IsValid(passport)) WindowContext.NewWorkerPassport.BorderBrush = Errors.ColorError;
                else WindowContext.NewWorkerPassport.BorderBrush = Errors.ColorInputDefault;
            }

        }

        private void UnMarkErrorInputs()
        {
            WindowContext.DepartamentNameInput.BorderBrush = Errors.ColorInputDefault;

            WindowContext.NewWorkerLastName.BorderBrush = Errors.ColorInputDefault;
            WindowContext.NewWorkerFirstName.BorderBrush = Errors.ColorInputDefault;
            WindowContext.NewWorkerPatronymic.BorderBrush = Errors.ColorInputDefault;
            WindowContext.NewWorkerAge.BorderBrush = Errors.ColorInputDefault;
            WindowContext.NewWorkerPhone.BorderBrush = Errors.ColorInputDefault;

            if (CurrentUser.WatchPassport)
            {
                WindowContext.NewWorkerPassport.BorderBrush = Errors.ColorInputDefault;
            }
        }

        private Worker CreateNewWorker()
        {
            Name name = new Name(
                WindowContext.NewWorkerFirstName.Text,
                WindowContext.NewWorkerLastName.Text,
                WindowContext.NewWorkerPatronymic.Text
            );

            PhoneNumber phone = new PhoneNumber(WindowContext.NewWorkerPhone.Text);

            PassportNumber passport;
            if (CurrentUser.WatchPassport) passport = new PassportNumber(WindowContext.NewWorkerPassport.Text);
            else passport = CurrentWorker.PassportNumber;

            uint age = uint.Parse(WindowContext.NewWorkerAge.Text);

            switch(WindowContext.NewWorkerProfession.SelectedIndex)
            {
                case 0: return new Manager(name, phone, passport, age);
                case 1: return new Programmer(name, phone, passport, age);
                case 2: return new Security(name, phone, passport, age);
                case 3: return new Accountant(name, phone, passport, age);
                case 4: return new Analyst(name, phone, passport, age);
                default: return null;
            }
        }

        private void SetErrorMessageText()
        {
            if (SetErrorMessageText(Name.GetNameError(WindowContext.NewWorkerLastName.Text))) return;
            if (SetErrorMessageText(Name.GetNameError(WindowContext.NewWorkerFirstName.Text))) return;
            if (SetErrorMessageText(Name.GetNameError(WindowContext.NewWorkerPatronymic.Text))) return;

            if (!IsAgeValid(WindowContext.NewWorkerAge.Text))
            {
                CurrentWorkerErrorText = "Возраст не должен быть\nменьше 18 или больше 100";
                return;
            }

            if (SetErrorMessageText(PhoneNumber.GetPhoneNumberError(WindowContext.NewWorkerPhone.Text))) return;
            if (SetErrorMessageText(PassportNumber.GetPassportNumberError(WindowContext.NewWorkerPassport.Text))) return;
            CurrentWorkerErrorText = string.Empty; 
        }

        private bool SetErrorMessageText(Enum error)
        {
            string message = Errors.GetMessageFromError(error);
            if (message == string.Empty) return false;
            else CurrentWorkerErrorText = message;
            return true;
        }
            
        private void CleanErrorMessageText()
        {
            CurrentWorkerErrorText = string.Empty;
        }

        private void ToDefaultState()
        {
            ToDefaultState(Errors.TextOK);
        }

        private void ToDefaultState(string message)
        {
            CurrentViewMode = new ViewMode();

            CurrentErrorColor = Errors.ColorOK;
            CurrentErrorText = message;

            WindowContext.DepartamentNameInput.Text = string.Empty;

            CleanWorkerInput();
            CleanErrorMessageText();
            UnMarkErrorInputs();

            if (!CurrentUser.ChangeOnlyPhone)
            {
                WindowContext.DepartamentNameInput.IsEnabled = true;
            }

            WindowContext.NewWorkerProfession.IsEnabled = true;
            WindowContext.NewWorkerProfession.SelectedIndex = 0;

            _isChange = false;
            _doneDepPress = false;
            _doneWorkerPress = false;
        }

        public void InputTextChanged()
        {
            InputTextChanged(false);
        }

        public void InputTextChanged(bool forWorker)
        {
            if(_doneDepPress || _doneWorkerPress)
            {
                string errorText = GetErrorText();

                if (errorText != string.Empty)
                {
                    CurrentErrorText = errorText;
                    CurrentErrorColor = Errors.ColorError;          
                }
                else
                {
                    ErrorsToOK();
                }

                ToMarkErrorInputs();
                SetErrorMessageText();
            }
        }

        public void ErrorsToOK()
        {
            CurrentErrorColor = Errors.ColorOK;
            CurrentErrorText = Errors.TextOK;
        }
    }
}

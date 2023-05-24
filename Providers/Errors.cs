using System.Windows.Media;
using Homework_11.Data;

namespace Homework_11.Providers
{
    internal static class Errors
    {
        public static Brush ColorError { get; }
        public static Brush ColorWarning { get; }
        public static Brush ColorOK { get; }

        public static Brush ColorInputDefault { get; }

        public static string TextOK { get; }

        static Errors ()
        {
            ColorOK = Brushes.GreenYellow;
            ColorError = Brushes.Red;
            ColorWarning = Brushes.Yellow;

            ColorInputDefault = new SolidColorBrush(Color.FromRgb(171, 173, 179));

            TextOK = "OK";
        }

        public static string GetTextFromError<T>(T error) where T : System.Enum
        {
            if(error is Name.Errors nameError)
            {
                switch(nameError)
                {
                    case Name.Errors.EmptyNameError: return "Пустое имя/фамилия/очество";
                    case Name.Errors.MinSymbolError: return "Имя/фамилия/очество слишком короткое";
                    case Name.Errors.MaxSymbolError: return "Имя/фамилия/очество слишком длинное";
                    case Name.Errors.WrongSymbolError: return "Недопустимые симолы в имя/фамилия/очество";

                    default: return "";
                }
            }

            if (error is PhoneNumber.Errors phoneError)
            {
                switch (phoneError)
                {
                    case PhoneNumber.Errors.EmptyError: return "Номер телефона пустой";
                    case PhoneNumber.Errors.MinLenghtError: return "Телефон слишком короткий";
                    case PhoneNumber.Errors.MaxLenghtError: return "Телефон слишком длинный";
                    case PhoneNumber.Errors.CountryCodeError: return "Неправильно указан код страны";
                    case PhoneNumber.Errors.OperatorCodeError: return "Неправильно указан код оператора";
                    case PhoneNumber.Errors.BaseNumberError: return "Неправильно указана оснавная часть телефона";
                    case PhoneNumber.Errors.WrongFormatError: return "Телефон имеет не верный формат";

                    default: return "";
                }
            }

            if (error is PassportNumber.Errors passportError)
            {
                switch (passportError)
                {
                    case PassportNumber.Errors.EmptyError: return "Номер паспорта пустой";
                    case PassportNumber.Errors.MaxLenghtError: return "Номер паспорта слишком длинный";
                    case PassportNumber.Errors.MinLenghtError: return "Номер паспорта слишком короткий";
                    case PassportNumber.Errors.WrongSymbolsError: return "Недопустимые символы в номере паспорта";
                    case PassportNumber.Errors.WrongFormatError: return "Неправильный формат номера паспорта";

                    default: return "";
                }
            }

            return "";
        }

        public static string GetMessageFromError<T>(T error) where T : System.Enum
        {
            if (error is Name.Errors nameError)
                if (nameError != Name.Errors.None)
                {
                    return "Фармат имени:\n" +
                       "1. Длина имени не меньше 2 символов\n" +
                       "2. Длина имени не больше 20 символов\n" +
                       "3. Символы могут быть только буквы";
                }


            if (error is PhoneNumber.Errors phoneError)
                if (phoneError != PhoneNumber.Errors.None)
                {
                    return "Формат номера телефона:\n" +
                           "1. Код страны не может \n   превышать 3 символа\n" +
                           "2. Код оператора может \n   быть заключен в круглые кавычки\n" +
                           "3. Основная часть номера \n   может разделяться дефисами\n" +
                           "4. Допускаются пробелы\n\n" +
                           "+0 (000) 000-00-00 или\n" +
                           "0 000 000 00 00 или\n" +
                           "00000000000";
                }

            if (error is PassportNumber.Errors passportError)
                if (passportError != PassportNumber.Errors.None)
                {
                    return "Формат номера паспорта:\n" +
                           "** ** ****** или\n" +
                           "**** ****** или\n" +
                           "**********";
                }

            return "";
        }
    }
}

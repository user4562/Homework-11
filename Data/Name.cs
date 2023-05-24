using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Homework_11.Data
{
    internal class Name
    {
        private string _firstName; 
        private string _lastName;
        private string _patronymic;

        [JsonPropertyName("FirstName")]
        public string FirstName
        {
            get => _firstName;
            set => _firstName = TryName(value);
        }

        [JsonPropertyName("LastName")]
        public string LastName
        {
            get => _lastName;
            set => _lastName = TryName(value);
            
        }

        [JsonPropertyName("Patronymic")]
        public string Patronymic
        {
            get => _patronymic;
            set => _patronymic = TryName(value);
        }

        public Name() { }

        public Name(string name)
        {
            name = TryName(name, true);
            
            var nameParts = name.Split(' ');

            _lastName = nameParts[0];
            _firstName = nameParts[1];
            _patronymic = nameParts[2];
        }

        public Name(string firstName, string lastName, string patronymic) 
        {
            _firstName = TryName(firstName);
            _lastName = TryName(lastName);
            _patronymic = TryName(patronymic);
        }

        [JsonIgnore]
        public static Name Empty
        {
            get => new Name("Пусто", "Пусто", "Пусто");
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {Patronymic}";
        }

        public static bool IsValid(string name, bool isFull = false)
        {
            return GetNameError(name, isFull) == Errors.None;
        }

        private static string TryName(string name, bool isFull = false)
        {
            Errors error = GetNameError(name, isFull);
            if (error == Errors.None) return name;
                throw new AggregateException(error.ToString());     
        }

        public static Errors GetNameError(string name, bool isFull = false)
        {
            name = name.Trim();

            if(isFull)
            {
                if (name.Contains(" ") && name.Count(s => s == ' ') != 2) return Errors.WrongFormatError;
            }

            if (name == null || name.Length == 0) return Errors.EmptyNameError;

            if (!name.All(symbol => char.IsLetter(symbol))) return Errors.WrongSymbolError;

            if (name.Length < (isFull ? 8 : 2)) return Errors.MinSymbolError;
            if (name.Length > (isFull ? 62 : 20)) return Errors.MaxSymbolError;

            return Errors.None;
        }

        public enum Errors
        {
            EmptyNameError,
            MinSymbolError,
            MaxSymbolError,
            WrongSymbolError,
            WrongFormatError,
            None
        }
    }
}

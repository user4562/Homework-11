using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Homework_11.Data
{
    internal class PhoneNumber : Number
    {
        [JsonIgnore]
        public string CountryCode { get; private set; }

        [JsonIgnore]
        public string OperatorCode { get; private set; }

        [JsonIgnore]
        public string BaseNumber { get; private set; }

        [JsonPropertyName("Text")]
        public override string Text
        {
            get { return ToString(); }
            set
            {
                PhoneNumber phone = new PhoneNumber(value);
                CountryCode = phone.CountryCode;
                OperatorCode = phone.OperatorCode;
                BaseNumber = phone.BaseNumber;
            }
        }

        public PhoneNumber() { }

        public PhoneNumber(string text)
        {
            Errors error = GetPhoneNumberError(text);
            if (error != Errors.None)
                throw new ArgumentException(error.ToString());

            text = GetOnlyDigit(text);

            CountryCode = text.Substring(0, text.Length - 10);
            OperatorCode = text.Substring(CountryCode.Length, 3);
            BaseNumber = text.Substring(CountryCode.Length + 3);
        }

        public override string ToString()
        {
            return $"{CountryCode} ({OperatorCode}) " +
                   $"{BaseNumber.Substring(0, 3)}-" +
                   $"{BaseNumber.Substring(3, 2)}-" +
                   $"{BaseNumber.Substring(5, 2)}";
        }

        [JsonIgnore]
        public static PhoneNumber Empty
        {
            get => new PhoneNumber("00000000000");
        }

        public static bool IsValid(string phoneNumber)
        {
            return GetPhoneNumberError(phoneNumber) == Errors.None;
        }

        public static Errors GetPhoneNumberError(string phoneNumber)
        {
            return new PhoneNumberChecker().GetPhoneNumberError(phoneNumber);
        }

        private static string GetOnlyDigit(string phoneNumber)
        {
            var onlyDigit = phoneNumber.Where(s => char.IsDigit(s) || s == '+');
            return string.Join("", onlyDigit);
        }

        public enum Errors
        {
            EmptyError,
            MinLenghtError,
            MaxLenghtError,
            CountryCodeError,
            OperatorCodeError,
            BaseNumberError,
            WrongFormatError,
            None
        }

        private class PhoneNumberChecker
        {
            private readonly char _charPlus, _charBracketLeft, _charBracketRight, _charHyphen;
            private readonly int _maxLenght, _minLenght;

            private bool _bracketsInclude;
            private int _bracketLeftIndex, _bracketRightIndex;

            private string _baseNumber, _operatorCode;

            public PhoneNumberChecker()
            {
                _charPlus = '+';
                _charBracketLeft = '(';
                _charBracketRight = ')';
                _charHyphen = '-';

                _maxLenght = 13;
                _minLenght = 11;

                _bracketsInclude = false;
                _bracketLeftIndex = -1;
                _bracketRightIndex = -1;

                _baseNumber = string.Empty;
                _operatorCode = string.Empty;
            }

            public Errors GetPhoneNumberError(string phoneNumber)
            {
                if (phoneNumber == null || phoneNumber.Length == 0) return Errors.EmptyError;

                phoneNumber = phoneNumber.Replace(" ", "");
                if (!CheckFormat(phoneNumber)) return Errors.WrongFormatError;

                string onlyDigit = GetOnlyDigit(phoneNumber);
                if (onlyDigit.Length > _maxLenght) return Errors.MaxLenghtError;
                if (onlyDigit.Length < _minLenght) return Errors.MinLenghtError;

                Errors error;

                error = GetErrorForBaseNumber(GetBaseNumber(phoneNumber));
                if (error != Errors.None) return error;

                error = GetErrorForOperatorCode(GetOperatorCode(phoneNumber));
                if (error != Errors.None) return error;

                error = GetErrorForCountryCode(GetCountryCode(phoneNumber));
                if (error != Errors.None) return error;

                return Errors.None;
            }

            private bool CheckFormat(string phoneNumber)
            {
                char[] allowedChars = new char[]
                {
                    _charPlus, _charBracketLeft, _charBracketRight, _charHyphen, ' '
                };

                if (!IncludeOnlyDigit(phoneNumber, allowedChars)) return false;

                if (phoneNumber.IndexOf("  ") != -1) return false;
                if (phoneNumber.IndexOf("--") != -1) return false;
                if (phoneNumber.LastIndexOf(_charPlus) > 0) return false;

                if (!BracketsCheck(phoneNumber)) return false;

                return true;
            }

            private bool BracketsCheck(string phoneNumber)
            {
                if (phoneNumber.Contains(_charBracketLeft) || phoneNumber.Contains(_charBracketRight))
                {
                    _bracketsInclude = true;

                    if (phoneNumber.Count(s => s == _charBracketLeft) != 1 ||
                        phoneNumber.Count(s => s == _charBracketRight) != 1) return false;

                    _bracketLeftIndex = phoneNumber.IndexOf(_charBracketLeft);
                    _bracketRightIndex = phoneNumber.IndexOf(_charBracketRight);

                    if (_bracketLeftIndex > _bracketRightIndex) return false;
                }

                return true;
            }

            private Errors GetErrorForBaseNumber(string baseNumber)
            {
                var baseDigit = baseNumber.Where(s => char.IsDigit(s));
                if (baseDigit.Count() != 7) return Errors.BaseNumberError;

                return Errors.None;
            }

            private Errors GetErrorForOperatorCode(string operatorCode)
            {
                if (!IncludeOnlyDigit(operatorCode)) return Errors.WrongFormatError;
                if (operatorCode.Length != 3) return Errors.OperatorCodeError;

                return Errors.None;
            }

            private Errors GetErrorForCountryCode(string countryCode)
            {
                if (!IncludeOnlyDigit(countryCode, _charPlus)) return Errors.WrongFormatError;

                if (countryCode.StartsWith(_charPlus.ToString()) && countryCode.Length < 2) return Errors.CountryCodeError;
                if (countryCode.Length < 1 || countryCode.Length > 3) return Errors.CountryCodeError;

                return Errors.None;
            }

            private string GetCountryCode(string phoneNumber)
            {
                int countryCodeLenght;

                if (_bracketsInclude) countryCodeLenght = _bracketLeftIndex;
                else countryCodeLenght = phoneNumber.LastIndexOf(_operatorCode + _baseNumber);

                return phoneNumber.Substring(0, countryCodeLenght);
            }

            private string GetOperatorCode(string phoneNumber)
            {
                string operatorCode;

                if (_bracketsInclude)
                {
                    int operatorCodeLenght = _bracketRightIndex - _bracketLeftIndex - 1;
                    operatorCode = phoneNumber.Substring(_bracketLeftIndex + 1, operatorCodeLenght);
                }
                else
                {
                    int operatorCodeIndex = phoneNumber.LastIndexOf(_baseNumber) - 3;
                    operatorCode = phoneNumber.Substring(operatorCodeIndex, 3);
                }

                _operatorCode = operatorCode;
                return operatorCode;
            }

            private string GetBaseNumber(string phoneNumber)
            {
                int baseIndex = phoneNumber.Length - 1;

                if (_bracketsInclude)
                {
                    baseIndex = _bracketRightIndex + 1;
                }
                else
                {
                    int baseLenght = 7;
                    for (int i = phoneNumber.Length - 1; i >= 0; i--)
                    {
                        if (char.IsDigit(phoneNumber[i])) baseLenght--;
                        if (baseLenght == 0)
                        {
                            baseIndex = i;
                            break;
                        }
                    }
                }

                _baseNumber = phoneNumber.Substring(baseIndex);
                return _baseNumber;
            }
        }
    }
}

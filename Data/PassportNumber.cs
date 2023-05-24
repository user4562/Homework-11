using System;
using System.Linq;
using System.Text.Json.Serialization;



namespace Homework_11.Data
{
    internal class PassportNumber : Number
    {
        private string _seriesFirst;
        private string _seriesLast;
        private string _serial;

        [JsonIgnore]
        public string SeriesFirst
        {
            get => _seriesFirst;
            set => _seriesFirst = TrySeries(value);
        }

        [JsonIgnore]
        public string SeriesLast
        {
            get => _seriesLast;
            set => _seriesLast = TrySeries(value);
        }

        [JsonIgnore]
        public string Serial
        {
            get => _serial;
            set => _serial = TrySerial(value);
        }

        public PassportNumber() { }

        public PassportNumber(string text)
        {
            text = text.Replace(" ", "");

            if (!IsValid(text))
                throw new ArgumentException("Wrong pasport number format.");

            _seriesFirst = text.Substring(0, 2);
            _seriesLast = text.Substring(2, 2);
            _serial = text.Substring(4);
        }

        public PassportNumber(string seriesFirst, string seriesLast, string serialNumber)
        {
            _seriesFirst = TrySeries(seriesFirst);
            _seriesLast = TrySeries(seriesLast);
            _serial = TrySerial(serialNumber);
        }

        [JsonIgnore]
        public static PassportNumber Empty
        {
            get => new PassportNumber("0000000000");
        }

        public static bool IsValid(string number)
        {
            return GetPassportNumberError(number) == Errors.None;
        }

        [JsonPropertyName("Text")]
        public override string Text
        {
            get { return ToString(); }
            set {
                PassportNumber number = new PassportNumber(value);

                _seriesFirst = number.SeriesFirst;
                _seriesLast = number.SeriesLast;
                _serial = number.Serial;
            }
        }

        public override string ToString()
        {
            return $"{_seriesFirst} {_seriesLast} {_serial}";
        }

        public static Errors GetPassportNumberError(string number)
        {

            if (number == null || number.Length == 0) return Errors.EmptyError;
            if (!IncludeOnlyDigit(number, ' ')) return Errors.WrongSymbolsError;

            if(number.Contains(' '))
            {
                if (!(number.IndexOf(' ') == 2 && number.LastIndexOf(' ') == 5 ||
                      number.IndexOf(' ') == 4 && number.LastIndexOf(' ') == 4)) 
                      return Errors.WrongFormatError;
            }

            number = number.Replace(" ", "");

            if (number.Length > 10) return Errors.MaxLenghtError;
            if (number.Length < 10) return Errors.MinLenghtError;

            return Errors.None;
        }

        private static string TrySeries(string series)
        {
            if (series.Length != 2 || !IncludeOnlyDigit(series))
                throw new ArgumentException("Wrong series number.");

            return series;
        }
        private static string TrySerial(string serial)
        {
            if (serial.Length != 6 || !IncludeOnlyDigit(serial))
                throw new ArgumentException("Wrong serial number.");

            return serial;
        }

        public enum Errors
        {
            EmptyError,
            MaxLenghtError,
            MinLenghtError,
            WrongSymbolsError,
            WrongFormatError,
            None
        }
    }
}

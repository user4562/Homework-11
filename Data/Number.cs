using System;
using System.Linq;

namespace Homework_11.Data
{
    internal abstract class Number
    {
        public abstract string Text { get; set; }

        protected static bool IncludeOnlyDigit(string number)
        {
            return IncludeOnlyDigit(number, new char[0]);
        }

        protected static bool IncludeOnlyDigit(string number, char allowedChar)
        {
            return IncludeOnlyDigit(number, new char[] { allowedChar });
        }

        protected static bool IncludeOnlyDigit(string number, char[] allowedChars)
        {
            return number.All(symbol =>
            {
                return char.IsDigit(symbol) || allowedChars.Contains(symbol);
            });
        }
    }
}

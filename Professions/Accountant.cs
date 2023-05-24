using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Accountant : Worker
    {
        public Accountant() : base() 
        {
            Profession = "Бухгалтер";
        }

        public Accountant(Name name, PhoneNumber phone, PassportNumber passport, uint age)
            : base(name, phone, passport, age)
        {
            Profession = "Бухгалтер";
        }
    }
}

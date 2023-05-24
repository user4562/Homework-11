using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Analyst : Worker
    {
        public Analyst() : base() 
        {
            Profession = "Аналитик";
        }

        public Analyst(Name name, PhoneNumber phone, PassportNumber passport, uint age)
            : base(name, phone, passport, age)
        {
            Profession = "Аналитик";
        }
    }
}

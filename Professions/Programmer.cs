using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Programmer : Worker
    {
        public Programmer() : base()
        {
            Profession = "Программист";
        }

        public Programmer(Name name, PhoneNumber phone, PassportNumber passport, uint age)
            : base(name, phone, passport, age)
        {
            Profession = "Программист";
        }
    }
}

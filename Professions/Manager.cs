using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Manager : Worker
    {
        public Manager() : base() 
        {
            Profession = "Менеджер";
        }

        public Manager(Name name, PhoneNumber phone, PassportNumber passport, uint age)
            : base(name, phone, passport, age)
        {
            Profession = "Менеджер";
        }
    }
}

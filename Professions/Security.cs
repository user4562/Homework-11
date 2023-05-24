using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Security : Worker
    {
        public Security() : base()
        {
            Profession = "Охранник";
        }

        public Security(Name name, PhoneNumber phone, PassportNumber passport, uint age)
            : base(name, phone, passport, age)
        {
            Profession = "Охранник";
        }
    }
}

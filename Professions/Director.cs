using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Director : Worker
    {
        public Director() : base() 
        {
            Profession = "Директор";
        }
        
        public Director(Name name, PhoneNumber phone, PassportNumber passport, uint age)
            : base(name, phone, passport, age)
        {
            Profession = "Директор";
        }
    }
}

using System.Windows;


namespace Homework_11.Providers
{
    public class User
    {
        public bool Add { get; }
        public bool Remove { get; }
        public bool WatchPassport { get; }
        public bool ChangeOnlyPhone { get; }

        public User(Users user)
        {
            if(user == Users.Manager)
            {
                Add = true;
                Remove = true;
                WatchPassport = true;
                ChangeOnlyPhone = false;
            }

            if(user == Users.Consultant)
            {
                Add = false;
                Remove = false;
                WatchPassport = false;
                ChangeOnlyPhone = true;
            }
        }
    }

    public enum Users
    {
        Manager,
        Consultant
    }
}

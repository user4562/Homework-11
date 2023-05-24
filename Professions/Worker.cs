using System.Text.Json.Serialization;
using Homework_11.Data;

namespace Homework_11.Professions
{
    internal class Worker
    {
        [JsonPropertyName("Name")]
        public Name Name { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public PhoneNumber PhoneNumber { get; set; }

        [JsonPropertyName("PassportNumber")]
        public PassportNumber PassportNumber { get; set; }

        [JsonPropertyName("Age")]
        public uint Age { get; set; }

        [JsonPropertyName("Profession")]
        public virtual string Profession { get; set; }

        [JsonIgnore]
        public string HidePassportNumber { get; }

        public Worker() : this(Name.Empty, PhoneNumber.Empty, PassportNumber.Empty, 0) { }

        public Worker(Name name, PhoneNumber phone, PassportNumber passport, uint age) 
        {
            HidePassportNumber = "** ** ******";
            Name = name;
            PhoneNumber = phone;
            PassportNumber = passport;
            Age = age;
            Profession = null;
        }
    }
}

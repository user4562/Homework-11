using System.IO;
using System.Text.Json;

using Homework_11.Organization;

namespace Homework_11.Providers
{
    internal static class DBProvider
    {
        public static Departament Load(string dbName)
        {
            if (File.Exists(dbName))
            {
                string jsonString = File.ReadAllText(dbName);
                return JsonSerializer.Deserialize<Departament>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public static void Save(string dbName, Departament departament)
        {
            string jsonString = JsonSerializer.Serialize(departament);
            File.WriteAllText(dbName, jsonString);
        }
    }
}

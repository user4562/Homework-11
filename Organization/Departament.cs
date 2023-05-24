using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

using Homework_11.Professions;

namespace Homework_11.Organization
{
    internal class Departament
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Departaments")]
        public ObservableCollection<Departament> Departaments { get; set; }

        [JsonIgnore]
        public Worker Boss 
        {
            get { return Workers[0]; }
            set
            {
                if (value != null)
                {
                    if(Workers.Count != 0) Workers.RemoveAt(0);
                    Workers.Insert(0, value);
                }
            }
        }

        [JsonPropertyName("Workers")]
        public ObservableCollection<Worker> Workers { get; set; }

        public Departament() { }

        public Departament(string name, Worker boss)
        {
            Departaments = new ObservableCollection<Departament>();
            Workers = new ObservableCollection<Worker>() { boss };

            Name = name;
            Boss = boss;
        }

        public Departament(string name, Worker boss, Worker[] workers) : this(name, boss)
        {
            Workers = new ObservableCollection<Worker>(workers);
            Workers.Insert(0, boss);
        }

        public Departament(string name, Worker boss, Worker[] workers, Departament[] departaments) 
            : this(name, boss, workers)
        {
            Departaments = new ObservableCollection<Departament>(departaments);
        }

        public void Add(Departament dep) => Departaments.Add(dep);
        public void Add(Worker worker) => Workers.Add(worker);

        public bool Remove(Departament dep)
        {
            if (Departaments.Remove(dep)) return true;

            foreach (Departament iDep in Departaments)
            {
                if(iDep.Remove(dep)) return true;
            }

            return false;
        }
        public void Remove(Worker worker) => Workers.Remove(worker);

        [JsonIgnore]
        public int CountDepartaments => Departaments.Count;

        [JsonIgnore]
        public int CountWorkers => Workers.Count;

        [JsonIgnore]
        public int CountDepartamentsInclude
        {
            get
            {
                int count = CountDepartaments;
                foreach (Departament dep in Departaments)
                {
                    count += dep.CountDepartamentsInclude;
                }
                return count;
            }
        }

        [JsonIgnore]
        public int CountWorkersInclude
        {
            get
            {
                int count = CountWorkers;
                foreach (Departament dep in Departaments)
                {
                    count += dep.CountWorkersInclude;
                }
                return count;
            }
        }
    }
}

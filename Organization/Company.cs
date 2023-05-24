using System.Collections.ObjectModel;

using Homework_11.Professions;
using Homework_11.Providers;
using Homework_11.Data;

namespace Homework_11.Organization 
{
    internal class Company
    {
        public string Name { get; set; }
        public Director Boss { get; set; }

        private Departament _mainDepartament;     
        public Departament MainDepartament {
            get { return _mainDepartament; } 
            set
            {
                if(_mainDepartament != null) Departaments.Remove(_mainDepartament);
                Departaments.Add(value);
                _mainDepartament = value;
            }
        }

        public int CountWorkersInclude { get => MainDepartament.CountWorkersInclude; }
        public int CountDepartamentsInclude { get => MainDepartament.CountDepartamentsInclude; }

        public ObservableCollection<Departament> Departaments { get; }

        public readonly string _dbName;

        public Company(string name, Director boss)
        {
            Boss = boss;
            Name = name;
            _dbName = $"DB{name}.json";

            Departaments = new ObservableCollection<Departament>();

            Load();
            Departaments.Add(_mainDepartament);
        }

        public bool RemoveDepartament(Departament dep)
        {
            return MainDepartament.Remove(dep);
        }

        public void Save() => DBProvider.Save(_dbName, MainDepartament);

        public void Load()
        {
            _mainDepartament = DBProvider.Load(_dbName);

            if (_mainDepartament == null)
            {
                _mainDepartament = new Departament("Главный департамет", Boss);
            }
        }
    }
}

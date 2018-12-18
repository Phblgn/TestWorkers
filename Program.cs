using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [Serializable]
    internal abstract class Worker : IComparable<Worker>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Salary { get; protected set; }
        public abstract void CalculateSalary();

        public Worker(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int CompareTo(Worker other)
        {
            var res = -Salary.CompareTo(other.Salary);
            if (res == 0)
                res = Name.CompareTo(other.Name);

            return res;
        }

        public override string ToString()
        {
            return ID + " " + Salary + " " + Name;
        }
    }

    [Serializable]
    internal class HourlyPayWorker : Worker
    {
        public double HourlySalary { get; set; }

        public HourlyPayWorker(int id, string name, double hourlySalary) : base(id, name)
        {
            HourlySalary = hourlySalary;
        }

        public override void CalculateSalary()
        {
            Salary = 20.8 * 8 * HourlySalary;
        }
    }

    [Serializable]
    internal class FixedPayWorker : Worker
    {
        public FixedPayWorker(int id, string name, double salary) : base(id, name)
        {
            Salary = salary;
        }

        public override void CalculateSalary()
        {
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {

            var workers = new List<Worker>();
            workers.Add(new HourlyPayWorker(43, "Андрей", 80));
            workers.Add(new FixedPayWorker(28, "Николай", 15000));
            workers.Add(new HourlyPayWorker(01, "Василий", 150));
            workers.Add(new FixedPayWorker(152, "Максим", 28000));
            workers.Add(new HourlyPayWorker(96, "Игорь", 110));
            workers.Add(new HourlyPayWorker(3, "Алексей", 150));
            workers.Add(new FixedPayWorker(8769, "Елена", 90000));
            workers.Add(new FixedPayWorker(61, "Ирина", 22000));
            workers.Add(new FixedPayWorker(82, "Антон", 90000));
            workers.Add(new HourlyPayWorker(88, "Артём", 150));
            foreach (Worker worker in workers)
                worker.CalculateSalary();

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Запись в файл ...");
            using (var fs = new FileStream("temp.bin", FileMode.Create))
                new BinaryFormatter().Serialize(fs, workers);

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Чтение из файла ...");
            using (var fs = new FileStream("temp.bin", FileMode.Open))
                try
                {
                    workers = (List<Worker>)new BinaryFormatter().Deserialize(fs);
                }
                catch 
                {
                    Console.WriteLine("Incorrect file format or file does not exist");
                    Console.ReadLine();
                    return;
                }

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Работники по убыванию среднемесячного заработка:");

            //workers.Sort();
            workers.Sort();

            // foreach (Worker worker in workers)
            //   Console.WriteLine(worker.ID,worker.Name,worker.Salary);
            foreach (Worker worker in workers)
                   Console.WriteLine(worker);

            Console.WriteLine("--------------------------------------------");
            for (int x = 0; x < 5; ++x) Console.WriteLine(workers[x].Name);
            Console.WriteLine("--------------------------------------------");
            for (int x = workers.Count-1; x >= workers.Count - 3 ; --x) Console.WriteLine(workers[x].ID);
            Console.ReadKey();
        }
    }
}
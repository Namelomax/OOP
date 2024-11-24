using System;
using System.Collections.Generic;
using System.IO;

namespace BusManagement
{
    public class Bus
    {
        public int Number { get; }
        public string DriverName { get; }
        public int RouteNumber { get; }

        public Bus(int number, string driverName, int routeNumber)
        {
            Number = number;
            DriverName = driverName;
            RouteNumber = routeNumber;
        }

        public override string ToString()
        {
            return $"Bus Number: {Number}, Driver: {DriverName}, Route: {RouteNumber}";
        }
    }

    // Узел однонаправленного списка
    public class Node
    {
        public Bus Data { get; set; }
        public Node Next { get; set; }

        public Node(Bus data)
        {
            Data = data;
            Next = null;
        }
    }

    // Однонаправленный список
    public class LinkedList
    {
        private Node head;

        public void Add(Bus bus)
        {
            Node newNode = new Node(bus);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public bool Remove(int busNumber, out Bus removedBus)
        {
            removedBus = null;
            if (head == null)
                return false;

            if (head.Data.Number == busNumber)
            {
                removedBus = head.Data;
                head = head.Next;
                return true;
            }

            Node current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Number == busNumber)
                {
                    removedBus = current.Next.Data;
                    current.Next = current.Next.Next;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public void Print()
        {
            if (head == null)
            {
                Console.WriteLine("No buses in the list.");
                return;
            }

            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data);
                current = current.Next;
            }
        }

        public List<Bus> ToList()
        {
            List<Bus> buses = new List<Bus>();
            Node current = head;
            while (current != null)
            {
                buses.Add(current.Data);
                current = current.Next;
            }
            return buses;
        }
    }

    // Управляющий класс
    public class BusManager
    {
        private LinkedList park = new LinkedList();
        private LinkedList route = new LinkedList();

        public void AddBusToPark(Bus bus)
        {
            park.Add(bus);
        }

        public void MoveBusToRoute(int busNumber)
        {
            if (park.Remove(busNumber, out Bus removedBus))
            {
                route.Add(removedBus);
                Console.WriteLine("Bus moved to route.");
            }
            else
            {
                Console.WriteLine("Bus not found in the park.");
            }
        }

        public void MoveBusToPark(int busNumber)
        {
            if (route.Remove(busNumber, out Bus removedBus))
            {
                park.Add(removedBus);
                Console.WriteLine("Bus moved to park.");
            }
            else
            {
                Console.WriteLine("Bus not found on the route.");
            }
        }
public void PrintList(bool isPark)
        {
            Console.WriteLine(isPark ? "Buses in the park:" : "Buses on the route:");
            if (isPark)
                park.Print();
            else
                route.Print();
        }

        public void SaveToFile(string parkFile, string routeFile)
        {
            File.WriteAllLines(parkFile, park.ToList().ConvertAll(b => $"{b.Number},{b.DriverName},{b.RouteNumber}"));
            File.WriteAllLines(routeFile, route.ToList().ConvertAll(b => $"{b.Number},{b.DriverName},{b.RouteNumber}"));
        }

        public void LoadFromFile(string parkFile, string routeFile)
        {
            if (File.Exists(parkFile))
            {
                foreach (var line in File.ReadAllLines(parkFile))
                {
                    var parts = line.Split(',');
                    AddBusToPark(new Bus(int.Parse(parts[0]), parts[1], int.Parse(parts[2])));
                }
            }

            if (File.Exists(routeFile))
            {
                foreach (var line in File.ReadAllLines(routeFile))
                {
                    var parts = line.Split(',');
                    route.Add(new Bus(int.Parse(parts[0]), parts[1], int.Parse(parts[2])));
                }
            }
        }
    }

    // Основной класс программы
    class Program
    {
        private const string BusNumberFile = "bus_number.txt";

        static void Main(string[] args)
        {
            BusManager manager = new BusManager();

            // Файлы для хранения данных
            string parkFile = "park.txt";
            string routeFile = "route.txt";

            // Загрузка данных из файлов
            manager.LoadFromFile(parkFile, routeFile);

            // Словарь команд
            var commands = new Dictionary<string, Action>
            {
                { "add", () => AddBus(manager) },
                { "move-to-route", () => MoveBusToRoute(manager) },
                { "move-to-park", () => MoveBusToPark(manager) },
                { "show-park", () => manager.PrintList(true) },
                { "show-route", () => manager.PrintList(false) },
                { "help", ShowHelp },
                { "exit", () => Exit(manager, parkFile, routeFile) }
            };

            ShowHelp();

            while (true)
            {
                Console.Write("\nEnter command: ");
                string command = Console.ReadLine()?.Trim().ToLower();

                if (commands.ContainsKey(command))
                {
                    commands[command].Invoke();
                }
                else
                {
                    Console.WriteLine("Invalid command. Type 'help' to see the list of available commands.");
                }
            }
        }

        // Методы для выполнения команд
        static void AddBus(BusManager manager)
        {
            int busNumber = GetNextBusNumber();

            Console.Write("Enter driver name: ");
            string driverName = Console.ReadLine();

            Console.Write("Enter route number: ");
            if (!int.TryParse(Console.ReadLine(), out int routeNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for the route.");
                return;
            }

            manager.AddBusToPark(new Bus(busNumber, driverName, routeNumber));
            Console.WriteLine($"Bus added successfully with number {busNumber}.");
        }

        static void MoveBusToRoute(BusManager manager)
        {
            Console.Write("Enter bus number: ");
            if (!int.TryParse(Console.ReadLine(), out int busNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            manager.MoveBusToRoute(busNumber);
        }
static void MoveBusToPark(BusManager manager)
        {
            Console.Write("Enter bus number: ");
            if (!int.TryParse(Console.ReadLine(), out int busNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            manager.MoveBusToPark(busNumber);
        }

        static void Exit(BusManager manager, string parkFile, string routeFile)
        {
            manager.SaveToFile(parkFile, routeFile);
            Console.WriteLine("Data saved. Exiting program...");
            Environment.Exit(0);
        }

        static void ShowHelp()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("add           - Add a new bus to the park");
            Console.WriteLine("move-to-route - Move a bus from the park to the route");
            Console.WriteLine("move-to-park  - Move a bus from the route to the park");
            Console.WriteLine("show-park     - Show all buses currently in the park");
            Console.WriteLine("show-route    - Show all buses currently on the route");
            Console.WriteLine("help          - Show this list of commands");
            Console.WriteLine("exit          - Save data and exit the program");
        }

        // Метод для получения следующего уникального номера автобуса
        static int GetNextBusNumber()
        {
            int busNumber = 1;

            if (File.Exists(BusNumberFile))
            {
                string content = File.ReadAllText(BusNumberFile);
                if (int.TryParse(content, out int lastNumber))
                {
                    busNumber = lastNumber + 1;
                }
            }

            File.WriteAllText(BusNumberFile, busNumber.ToString());
            return busNumber;
        }
    }
}
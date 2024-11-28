using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BusManagement
{
    public class Bus
    {
        public int Number { get; set; }
        public string DriverName { get; set; }
        public bool IsOnRoute { get; set; }

        public override string ToString()
        {
            return $"Bus {Number}: Driver {DriverName}, " + (IsOnRoute ? "On Route" : "In Park");
        }
    }

    public class BusManager
    {
        private const string DataFile = "busData.json";

        public List<Bus> Buses { get; private set; }

        public BusManager()
        {
            Buses = LoadFromFile();
        }

        public void AddBus(int number, string driverName)
        {
            Buses.Add(new Bus { Number = number, DriverName = driverName, IsOnRoute = false });
        }

        public bool MoveToRoute(int number)
        {
            var bus = Buses.Find(b => b.Number == number && !b.IsOnRoute);
            if (bus != null)
            {
                bus.IsOnRoute = true;
                return true;
            }
            return false;
        }

        public bool MoveToPark(int number)
        {
            var bus = Buses.Find(b => b.Number == number && b.IsOnRoute);
            if (bus != null)
            {
                bus.IsOnRoute = false;
                return true;
            }
            return false;
        }

        public void ShowPark()
        {
            foreach (var bus in Buses.FindAll(b => !b.IsOnRoute))
            {
                Console.WriteLine(bus);
            }
        }

        public void ShowRoute()
        {
            foreach (var bus in Buses.FindAll(b => b.IsOnRoute))
            {
                Console.WriteLine(bus);
            }
        }

        public void SaveToFile()
        {
            var json = JsonSerializer.Serialize(Buses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataFile, json);
        }

        private List<Bus> LoadFromFile()
        {
            if (File.Exists(DataFile))
            {
                var json = File.ReadAllText(DataFile);
                return JsonSerializer.Deserialize<List<Bus>>(json) ?? new List<Bus>();
            }
            return new List<Bus>();
        }
    }

    class Program
    {
        private const string BusNumberFile = "busNumber.txt";

        static void Main(string[] args)
        {
            var manager = new BusManager();

            var commands = new Dictionary<string, Action>
            {
                { "add", () => AddBus(manager) },
                { "move-to-route", () => MoveToRoute(manager) },
                { "move-to-park", () => MoveToPark(manager) },
                { "show-park", () => { Console.WriteLine("Buses in park:"); manager.ShowPark(); } },
                { "show-route", () => { Console.WriteLine("Buses on route:"); manager.ShowRoute(); } },
                { "help", ShowHelp },
                { "exit", () => Exit(manager) }
            };

            while (true)
            {
                Console.Write("Enter command (help for list of commands): ");
                string command = Console.ReadLine()?.ToLower();

                if (commands.ContainsKey(command))
                {
                    commands[command].Invoke();
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help' for list of commands.");
                }
            }
        }

        static void AddBus(BusManager manager)
        {
            Console.Write("Enter driver name: ");
            string driverName = Console.ReadLine();
            int busNumber = GetNextBusNumber();
            manager.AddBus(busNumber, driverName);
            Console.WriteLine($"Bus {busNumber} added.");
        }

static void MoveToRoute(BusManager manager)
        {
            Console.Write("Enter bus number to move to route: ");
            if (int.TryParse(Console.ReadLine(), out int routeBusNumber) &&
                manager.MoveToRoute(routeBusNumber))
            {
                Console.WriteLine($"Bus {routeBusNumber} moved to route.");
            }
            else
            {
                Console.WriteLine("Bus not found or already on route.");
            }
        }

        static void MoveToPark(BusManager manager)
        {
            Console.Write("Enter bus number to move to park: ");
            if (int.TryParse(Console.ReadLine(), out int parkBusNumber) &&
                manager.MoveToPark(parkBusNumber))
            {
                Console.WriteLine($"Bus {parkBusNumber} moved to park.");
            }
            else
            {
                Console.WriteLine("Bus not found or already in park.");
            }
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

        static void Exit(BusManager manager)
        {
            manager.SaveToFile();
            Console.WriteLine("Data saved. Exiting program...");
            Environment.Exit(0);
        }

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
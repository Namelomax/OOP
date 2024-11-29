using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MicrofinanceOrganization
{
    // Класс клиента
    public class Client
    {
        private int id;
        private string name;
        private string phone;

        public int Id => id;
        public string Name => name;
        public string Phone => phone;

        public Client(int id, string name, string phone)
        {
            this.id = id;
            this.name = name;
            this.phone = phone;
        }

        public override string ToString()
        {
            return $"Client ID: {id}, Name: {name}, Phone: {phone}";
        }
    }

    // Класс кредита
    public class Credit
    {
        private int id;
        private double amount;
        private double interestRate;
        private DateTime repaymentDate;
        private string comments;

        public int Id => id;
        public double Amount => amount;
        public double InterestRate => interestRate;
        public DateTime RepaymentDate => repaymentDate;
        public string Comments => comments;

        public Credit(int id, double amount, double interestRate, DateTime repaymentDate, string comments)
        {
            this.id = id;
            this.amount = amount;
            this.interestRate = interestRate;
            this.repaymentDate = repaymentDate;
            this.comments = comments;
        }

        public override string ToString()
        {
            return $"Credit ID: {id}, Amount: {amount:C}, Interest Rate: {interestRate}%, Repayment Date: {repaymentDate.ToShortDateString()}, Comments: {comments}";
        }
    }

    // Статический класс для обработки данных
    public static class DataHandler
    {
        public static void SaveToFile<T>(string fileName, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        public static List<T> LoadFromFile<T>(string fileName)
        {
            if (!File.Exists(fileName)) return new List<T>();
            var json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }

    class Program
    {
        private const string ClientsFile = "clients.json";
        private const string CreditsFile = "credits.json";

        static void Main(string[] args)
        {
            List<Client> clients = DataHandler.LoadFromFile<Client>(ClientsFile);
            List<Credit> credits = DataHandler.LoadFromFile<Credit>(CreditsFile);
            ShowHelp();

            var commands = new Dictionary<string, Action>
            {
                { "add-client", () => AddClient(clients) },
                { "add-loan", () => AddCredit(credits) },
                { "show-clients", () => ShowClients(clients) },
                { "show-loans", () => ShowCredits(credits) },
                { "help", ShowHelp },
                { "exit", () => Exit(clients, credits) }
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

        static void AddClient(List<Client> clients)
        {
            string name;
            do
            {
                Console.Write("Enter client name (letters only): ");
                name = Console.ReadLine();
                if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
                {
                    Console.WriteLine("Invalid name. It should only contain letters and spaces.");
                    name = null;
                }
            } while (name == null);

            string phone;
            do
            {
                Console.Write("Enter client phone (digits only): ");
                phone = Console.ReadLine();
                if (!Regex.IsMatch(phone, @"^\d+$"))
                {
                    Console.WriteLine("Invalid phone number. It should only contain digits.");
                    phone = null;
                }
            } while (phone == null);

            int id = clients.Count > 0 ? clients[^1].Id + 1 : 1; // Уникальный ID
            clients.Add(new Client(id, name, phone));
            Console.WriteLine("Client added successfully.");
        }

        static void AddCredit(List<Credit> credits)
        {
            double amount = GetValidatedInput<double>("Enter credit amount: ", "Invalid amount. Please enter a valid number.");
            double interestRate = GetValidatedInput<double>("Enter interest rate: ", "Invalid interest rate. Please enter a valid number.");
            DateTime repaymentDate = GetValidatedInput<DateTime>("Enter repayment date (YYYY-MM-DD): ", "Invalid date. Please enter in format YYYY-MM-DD.");
            
            Console.Write("Enter comments: ");
            string comments = Console.ReadLine();

            int id = credits.Count > 0 ? credits[^1].Id + 1 : 1; // Уникальный ID
            credits.Add(new Credit(id, amount, interestRate, repaymentDate, comments));
            Console.WriteLine("Credit added successfully.");
        }

        static T GetValidatedInput<T>(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                try
                {
                    return (T)Convert.ChangeType(input, typeof(T));
                }
                catch
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }

        static void ShowClients(List<Client> clients)
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("No clients available.");
            }
            else
            {
                foreach (var client in clients)
                {
                    Console.WriteLine(client);
                }
            }
        }

        static void ShowCredits(List<Credit> credits)
        {
            if (credits.Count == 0)
            {
                Console.WriteLine("No credits available.");
            }
            else
            {
                foreach (var credit in credits)
                {
                    Console.WriteLine(credit);
                }
            }
        }

        static void Exit(List<Client> clients, List<Credit> credits)
        {
            DataHandler.SaveToFile(ClientsFile, clients);
            DataHandler.SaveToFile(CreditsFile, credits);
            Console.WriteLine("Data saved. Exiting program...");
            Environment.Exit(0);
        }

        static void ShowHelp()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("add-client   - Add a new client to the system");
            Console.WriteLine("add-loan     - Add a new loan");
            Console.WriteLine("show-clients - Show all registered clients");
            Console.WriteLine("show-loans   - Show all credits and their statuses");
            Console.WriteLine("help         - Show this list of commands");
            Console.WriteLine("exit         - Save data and exit the program");
        }
    }
}
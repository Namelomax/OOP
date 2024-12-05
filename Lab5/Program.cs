using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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

     public static class DataHandler
    {
        public static void SaveToFile<T>(string fileName, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        public static List<T> LoadFromFile<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            return new List<T>();
        }
    }

    // Класс авторизации
    public class AuthManager
    {
        private Dictionary<string, string> userDatabase;

        public AuthManager()
        {
            userDatabase = LoadUsers();
        }

        public bool Register(string username, string password)
        {
            if (userDatabase.ContainsKey(username))
            {
                Console.WriteLine("Username already exists.");
                return false;
            }

            string hashedPassword = HashPassword(password);
            userDatabase[username] = hashedPassword;
            SaveUsers();
            Console.WriteLine("User registered successfully.");
            return true;
        }

        public bool Login(string username, string password)
        {
            if (!userDatabase.ContainsKey(username))
            {
                Console.WriteLine("User not found.");
                return false;
            }

            string hashedPassword = userDatabase[username];
            if (VerifyPassword(password, hashedPassword))
            {
                Console.WriteLine("Login successful.");
                return true;
            }

            Console.WriteLine("Invalid password.");
            return false;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        private void SaveUsers()
        {
            string json = JsonSerializer.Serialize(userDatabase, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("users.json", json);
        }
private Dictionary<string, string> LoadUsers()
        {
            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();
        }
    }

    // Основная программа
    class Program
    {
        private const string ClientsFile = "clients.json";
        private const string CreditsFile = "credits.json";

        static void Main(string[] args)
        {
            AuthManager authManager = new AuthManager();

            Console.WriteLine("Welcome to the Microfinance System!");
            while (true)
            {
                Console.Write("Enter 'register', 'login', or 'exit': ");
                string authCommand = Console.ReadLine()?.ToLower();

                if (authCommand == "register")
                {
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    authManager.Register(username, password);
                }
                else if (authCommand == "login")
                {
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    if (authManager.Login(username, password)) break;
                }
                else if (authCommand == "exit")
                {
                    Console.WriteLine("Exiting system.");
                    return;
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }

            List<Client> clients = DataHandler.LoadFromFile<Client>(ClientsFile);
            List<Credit> credits = DataHandler.LoadFromFile<Credit>(CreditsFile);

            ShowHelp();

            var commands = new Dictionary<string, Action>
            {
                { "add-client", () => AddClient(clients) },
                { "add-credit", () => AddCredit(credits) },
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

        static void AddClient(List<Client> clients) { /*...*/ }
        static void AddCredit(List<Credit> credits) { /*...*/ }
        static void ShowClients(List<Client> clients) { /*...*/ }
        static void ShowCredits(List<Credit> credits) { /*...*/ }
        static void ShowHelp() { /*...*/ }
        static void Exit(List<Client> clients, List<Credit> credits) { /*...*/ }
    }
}
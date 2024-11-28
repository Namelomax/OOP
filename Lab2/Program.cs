using System;

namespace University
{
    // Интерфейс для описания личных данных
    public interface IPersonalInfo
    {
        string GetFullName();
        int GetAge();
    }

    // Интерфейс для деятельности
    public interface IActivity
    {
        void PerformActivity();
    }

    // Интерфейс для взаимодействия с системой
    public interface ISystemInteraction
    {
        void LogInteraction();
    }

    // Абстрактный класс Persona
    public abstract class Persona : IPersonalInfo, IActivity, ISystemInteraction
    {
        public string FullName { get; private set; }
        public int Age { get; private set; }

        public Persona(string fullName, int age)
        {
            FullName = fullName;
            Age = age;
        }

        // Реализация интерфейса IPersonalInfo
        public string GetFullName() => FullName;
        public int GetAge() => Age;

        // Абстрактный метод для деятельности
        public abstract void PerformActivity();

        // Общий метод взаимодействия с системой
        public void LogInteraction()
        {
            Console.WriteLine($"{GetFullName()} взаимодействует с системой.");
        }

        // Виртуальный метод для отображения информации
        public virtual void PrintInfo()
        {
            Console.WriteLine($"Имя: {FullName}, Возраст: {Age}");
        }
    }

    // Класс Student
    public class Student : Persona
    {
        public string Group { get; private set; }

        public Student(string fullName, int age, string group)
            : base(fullName, age)
        {
            Group = group;
        }

        public override void PerformActivity()
        {
            Console.WriteLine($"{GetFullName()} учится в группе {Group}.");
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Группа: {Group}");
        }
    }

    // Класс Teacher
    public class Teacher : Persona
    {
        public string Subject { get; private set; }

        public Teacher(string fullName, int age, string subject)
            : base(fullName, age)
        {
            Subject = subject;
        }

        public override void PerformActivity()
        {
            Console.WriteLine($"{GetFullName()} преподает предмет: {Subject}.");
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Преподаваемый предмет: {Subject}");
        }
    }

    // Класс DepartmentHead
    public class DepartmentHead : Teacher
    {
        public string Department { get; private set; }

        public DepartmentHead(string fullName, int age, string subject, string department)
            : base(fullName, age, subject)
        {
            Department = department;
        }

        public override void PerformActivity()
        {
            Console.WriteLine($"{GetFullName()} управляет кафедрой: {Department}.");
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Кафедра: {Department}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создаем объекты
            Persona student = new Student("Иван Иванов", 20, "ИС-21");
            Persona teacher = new Teacher("Петр Петров", 45, "Математика");
            Persona departmentHead = new DepartmentHead("Анна Смирнова", 50, "Программирование", "Информатика");

            // Демонстрация полиморфизма
            Persona[] personas = { student, teacher, departmentHead };
            foreach (var persona in personas)
            {
                persona.PrintInfo();
                persona.PerformActivity();
                persona.LogInteraction();
                Console.WriteLine();
            }
        }
    }
}
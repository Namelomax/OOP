using System;

namespace University
{
    // Базовый класс Persona
    public class Persona
    {
        public string FullName { get; set; }
        public int Age { get; set; }

        public Persona(string fullName, int age)
        {
            FullName = fullName;
            Age = age;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"Персона: {FullName}, Возраст: {Age}");
        }
    }

    // Класс Student, наследуется от Persona
    public class Student : Persona
    {
        public string Group { get; set; }
        public double GPA { get; set; } // Средняя оценка

        public Student(string fullName, int age, string group, double gpa) 
            : base(fullName, age)
        {
            Group = group;
            GPA = gpa;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Студент группы: {Group}, Средний балл: {GPA:F2}");
        }
    }

    // Класс Teacher наследуется от Persona
    public class Teacher : Persona
    {
        public string Subject { get; set; }
        public int Experience { get; set; } // Опыт работы

        public Teacher(string fullName, int age, string subject, int experience) 
            : base(fullName, age)
        {
            Subject = subject;
            Experience = experience;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Преподаватель предмета: {Subject}, Опыт: {Experience} лет");
        }
    }

    // Класс DepartmentHead, наследуется от Teacher
    public class DepartmentHead : Teacher
    {
        public string Department { get; set; }

        public DepartmentHead(string fullName, int age, string subject, int experience, string department) 
            : base(fullName, age, subject, experience)
        {
            Department = department;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Заведующий кафедрой: {Department}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Пример создания объектов
            Student student = new Student("Иван Иванов", 20, "ИС-23-1", 4.5);
            Teacher teacher = new Teacher("Петр Петров", 45, "Математика", 20);
            DepartmentHead departmentHead = new DepartmentHead("Анна Смирнова", 50, "Программирование", 25, "Кафедра Информатики");

            // Вывод
            student.PrintInfo();
            Console.WriteLine();
            teacher.PrintInfo();
            Console.WriteLine();
            departmentHead.PrintInfo();
        }
    }
}
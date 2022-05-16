using System;
using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.LocalDAL
{
    internal class LocalStorage
    {

        internal static Settings Settings = new Settings("DENDUKA", 1);

        internal static Dictionary<string, Account> Accounts = new Dictionary<string, Account>()
        {
            ["DENDUKA"] = new Account("DENDUKA", "DENDUKA", "123", "admin", "test@test.test", "PCNAME1"),
            ["Account1"] = new Account("Account1", "Account1", "123", "user", "test2@test.test", "PCNAME1")
        };

        internal static Dictionary<int, Status> Statuses = new Dictionary<int, Status>()
        {
            [1] = new Status(1, "Отложена"),
            [2] = new Status(2, "В работе"),
            [3] = new Status(3, "Завершена"),
            [4] = new Status(4, "Удалена")
        };


        internal static Dictionary<int, ProjectType> ProjectTypes = new Dictionary<int, ProjectType>()
        {
            [1] = new ProjectType(1, "Геолого-разведочные работы"),
            [2] = new ProjectType(2, "Сейсморазведка"),
            [3] = new ProjectType(3, "Подсчет запасов"),
            [4] = new ProjectType(4, "Разработка"),
            [5] = new ProjectType(5, "Другие проекты"),
        };

        internal static Dictionary<int, Project> Projects = new Dictionary<int, Project>()
        {
            [1] = new Project(1, "Project1", Accounts["DENDUKA"], ProjectTypes[1], Statuses[1],"", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), DateTime.MinValue, "12345678990", 9000000, 1, 50,false),
            [2] = new Project(2, "Project2", Accounts["DENDUKA"], ProjectTypes[1], Statuses[2],"", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), DateTime.MinValue, "12345678990", 9000000, 2, 50,false),
        };

        internal static Dictionary<int, Task> Tasks = new Dictionary<int, Task>()
        {
            [0] = new Task(0, "task1", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), Statuses[1], 1, Projects[1].Name, Accounts["DENDUKA"], "description", Accounts["DENDUKA"], 1),
            [1] = new Task(1, "task2", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), Statuses[2], 1, Projects[1].Name, Accounts["Account1"], "description", Accounts["DENDUKA"], 2),
            [2] = new Task(2, "task3", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), Statuses[3], 1, Projects[1].Name, Accounts["DENDUKA"], "description", Accounts["DENDUKA"], 5),
            [3] = new Task(3, "task4", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), Statuses[4], 2, Projects[2].Name, Accounts["Account1"], "description", Accounts["DENDUKA"], 10),
            [4] = new Task(4, "task5", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), Statuses[1], 2, Projects[2].Name, Accounts["DENDUKA"], "description", Accounts["DENDUKA"], 2),
            [5] = new Task(5, "task6", new DateTime(2019, 10, 10), new DateTime(2019, 10, 13), Statuses[2], 2, Projects[2].Name, Accounts["Account1"], "description", Accounts["DENDUKA"], 3),
        };

        internal static Dictionary<int, Company> Companies = new Dictionary<int, Company>()
        {
            [0] = new Company(0, "company1", "email@email.email","0000000000", "ООО"),
            [1] = new Company(1, "company2", "email@email.email", "0000000000", "ОАО"),
            [2] = new Company(2, "company3", "email@email.email", "0000000000", "ЗАО"),
        };

        internal static Dictionary<int, Employee> Employees = new Dictionary<int, Employee>()
        {
            [0] = new Employee(0, "name0", "9271110088", 1234, "email@email.email", 3),
            [1] = new Employee(1, "name1", "9271110088", 1234, "email@email.email", 1),
            [2] = new Employee(2, "name2", "9271110088", 1234, "email@email.email", 1),
            [3] = new Employee(3, "name3", "9271110088", 1234, "email@email.email", 1),
            [4] = new Employee(4, "name4", "9271110088", 1234, "email@email.email", 1),
            [5] = new Employee(5, "name5", "9271110088", 1234, "email@email.email", 2),
            [6] = new Employee(6, "name6", "9271110088", 1234, "email@email.email", 2),
            [7] = new Employee(7, "name7", "9271110088", 1234, "email@email.email", 2),
            [8] = new Employee(8, "name8", "9271110088", 1234, "email@email.email", 2),
            [9] = new Employee(9, "name9", "9271110088", 1234, "email@email.email", 3),
            [10] = new Employee(10, "name10", "9271110088", 1234, "email@email.email", 3),
            [11] = new Employee(11, "name11", "9271110088", 1234, "email@email.email", 3),
            [12] = new Employee(2, "name12", "9271110088", 1234, "email@email.email", 3),
        };
    }
}

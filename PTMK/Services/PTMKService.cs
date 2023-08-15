using PTMK.Controllers;
using PTMK.Models;
using System.Text.RegularExpressions;
using System;
using PTMK.Data;

namespace PTMK.Services
{
    internal class PTMKService
    {
        private const int RECORDS_CNT = 1000000;
        private const int F_RECORDS_CNT = 100;
        private const int STR_SIZE_MIN = 2;
        private const int STR_SIZE_MAX = 8;
        private readonly PersonsRepository _repo;
        private readonly Random _prng;

        public PTMKService(PersonsRepository repo)
        {
            _repo = repo;
            _prng = new Random();
        }

        public void HandleConsole()
        {
            string[]? command;
            DateTime startTime;
            while (true)
            {
                Console.WriteLine("\nWaiting for command.");
                command = Console.ReadLine()?.Split();
                if (command != null && command.Length > 1)
                {
                    try
                    {
                        switch (command[0] + command[1])
                        {
                            case "ptmk1":
                                PTMK1();
                                PrintMessage("Created successfully Table \"Person\".");
                                break;
                            case "ptmk2":
                                if (command.Length == 7)
                                {
                                    PTMK2(new[] { command[2], command[3], command[4], command[5], command[6] });
                                    PrintMessage($"Added successfully\t\"{command[2]}\"\trecord.");
                                }
                                else
                                {
                                    throw new FormatException();
                                }
                                break;
                            case "ptmk3":
                                PrintMessage($"Records distinct by the key \"Surname+Name+Patronymic+DoB\":\n{PTMK3()}\nSequence end.");
                                break;
                            case "ptmk4":
                                PrintMessage("Request begin");
                                if (PTMK4(RECORDS_CNT, 2))
                                {
                                    PrintMessage($"Added successfully\t{RECORDS_CNT}\trecords.");
                                }
                                if (PTMK4(F_RECORDS_CNT, 1))
                                {
                                    PrintMessage($"Added successfully\t{F_RECORDS_CNT}\trecords.");
                                }
                                break;
                            case "ptmk5":
                                startTime = DateTime.Now;
                                PrintMessage($"Records by the key \"Males with the first \"F\" in Surname\":\n{PTMK5()}\nAnd took {(DateTime.Now - startTime).TotalMilliseconds} ms.");
                                break;
                            case "ptmk6A":
                                startTime = DateTime.Now;
                                PrintMessage($"Optimized query by LINQ-Query-Method:\n{PTMK6A()}\nAnd took {(DateTime.Now - startTime).TotalMilliseconds} ms.");
                                break;
                            case "ptmk6B":
                                startTime = DateTime.Now;
                                PrintMessage($"Optimized query by SelDataReader:\n{ParseToView(new PTMKDbContext().GetBySqlDataReader())}\nAnd took {(DateTime.Now - startTime).TotalMilliseconds} ms.");
                                break;
                            default:
                                Console.WriteLine(GetHelp());
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("\n\n\nSayonara!\a");
                    return;
                }
            }
        }

        private void PTMK1()
            => _repo.GetAll();// Will provoke "PTMKDb" Database and "Person" Table creation.

        private void PTMK2(string[] credentials)
            => _repo.Add(credentials);

        private string PTMK3()
            => ParseToView(_repo.GetAll()
                                    .DistinctBy(p => p.Surname + p.Name + p.Patronymic + p.DoB)// Get the Intersection of values.
                                    .OrderBy(p => p.Surname + p.Name + p.Patronymic));

        private bool PTMK4(int cntToCreate, int f_mode)
        {
            var regex = new Regex("^.");
            string[] credentials;
            int recordsCnt = _repo.GetAll().Count();
            int strSize;
            for (int i = 0; i < cntToCreate; i++)
            {
                credentials = new string[typeof(Person).GetProperties().Length - 1];
                for (int j = 0; j < credentials.Length; j++)
                {
                    switch (j)
                    {
                        case 3:
                            credentials[j] = $"{_prng.Next(DateTime.Now.Year - 1900) + 1900}/{_prng.Next(12) + 1}/{_prng.Next(28) + 1}";
                            break;
                        case 4:
                            credentials[j] = _prng.Next(f_mode) == 0 ? "Male" : "Female";
                            break;
                        default:
                            strSize = _prng.Next(STR_SIZE_MIN, STR_SIZE_MAX);
                            for (int k = 0; k < strSize; k++)
                            {
                                credentials[j] += (char)_prng.Next('a', 'z');
                            }
                            credentials[j] = regex.Replace(credentials[j], (f_mode == 1 && j == 0 ? 'F' : (char)(credentials[j][0] - 32)).ToString());
                            break;
                    }
                }
                _repo.Add(credentials);
            }
            return _repo.GetAll().Count() == recordsCnt + cntToCreate;
        }

        private string PTMK5()
            => ParseToView(_repo.GetAll()
                                .Where(p => p.Sex && p.Surname[0] == 'F'));

        private string PTMK6A()
            => ParseToView(_repo.GetByLinqQuery());

        private string ParseToView(IEnumerable<Person> persons)
        {
            string res = "\nSurname\t\tName\t\tPatronymic\tDate of Birth\t\tSex\t\tAge";
            int age;
            foreach (var person in persons)
            {
                age = DateTime.Now.Year - person.DoB.Year;
                if (DateTime.Now.Month < person.DoB.Month || (DateTime.Now.Month == person.DoB.Month && DateTime.Now.Day <= person.DoB.Day))
                {
                    ++age;
                }
                res += $"\n{person.Surname}\t\t{person.Name}\t\t{person.Patronymic}\t\t{person.DoB:dd.MM.yyyy}\t\t{(person.Sex ? "Male" : "Female")}\t\t{age}";
            }
            return $"{res}\n\n{persons.Count()}\tRecords were printed out.";
        }

        private void PrintMessage(string message)
            => Console.WriteLine($"{DateTime.Now:HH:mm:ss}\t{message}.\a");

        private string GetHelp()
            => "Unknown command." +
                "\nHelp:" +
                "\nptmk 1 - Create \"Person\" table;" +
                "\nptmk 2 [string] [string] [string] [yyyy/MM/dd] [Female/Male] - Create record with given Surname, Name, Patronymic, DoB, and Sex." +
                "\n[Local Date Format Required];" +
                "\nptmk 3 - Print Persons data by distinct Name and DoB;" +
                "\nptmk 4 - Fill 1,000,000 records AND 100 records of the Males with the first \"F\" character in Surname;" +
                "\nptmk 5 - Select Males with the first \"F\" in Surname;" +
                "\nptmk 6A - Execute optimized query by LINQ-Query-Method;" +
                "\nptmk 6B - Execute optimized query by SqlDataReader.";
    }
}
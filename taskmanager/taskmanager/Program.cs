using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace taskmanager
{
    public static class ConsoleEx
    {
        public static void WriteLine(string message, ConsoleColor color)
        {
            ConsoleColor orgColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"{message}");
            Console.ForegroundColor = orgColor;
        }

        public static void Write(string message, ConsoleColor color)
        {
            ConsoleColor orgColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"{message}");
            Console.ForegroundColor = orgColor;
        }
    }

    public class TaskModel
    {
        private string _name = ""; //Pole wymagane - krótki tytuł zadania
        private string _description = "";//Opis - Wymagany.
        public DateTime dateStart; //Datę Rozpoczęcia - Wymagana.
        public DateTime? dateEnd;// Datę Zakończenia - Niewymagana, jeśli zadanie jest całodniowe.
        public bool? fulldayFlag = false;//Zadanie Całodniowe - Niewymagana, domyślnie zadanie nie jest całodniowe.
        private bool _correctData = true;//checking data correction
        public importantFlag prioryty;

        public enum importantFlag
        {
            //Zadanie Ważne - Niewymagana, domyślnie zadanie nie jest ważne.
            Normal,
            Important,
            Very_Important
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value.Length > 2)
                {
                    _description = value;
                }
                else
                {
                    ConsoleEx.WriteLine("Opis musi zawierać co najmniej 3 znaki", ConsoleColor.Red);
                    CorrectData = false;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length < 11 && value.Length > 0)
                {
                    _name = value;
                }
                else
                {
                    ConsoleEx.WriteLine("Tytuł musi zawierać od 3 do 10 znaków", ConsoleColor.Red);
                    CorrectData = false;
                }
            }
        }

        public bool CorrectData { get; private set; }

        public TaskModel(string name, string description, DateTime start, int prior)
        {
            Name = name;
            Description = description;
            dateStart = start;
            prioryty = (importantFlag)prior;
            fulldayFlag = true;
        }

        public TaskModel(string name, string description, DateTime start, int prior, DateTime end)
        {
            Name = name;
            Description = description;
            dateStart = start;
            dateEnd = end;
            prioryty = (importantFlag)prior;
        }

    }

    class Program
    {
        public string[] listOdCommands = new string[4]
        {
            "add (dodawanie zadania)", "rem (usuwanie zadania)", "show (listowanie zapisanych zadań)", ""
        };

        public static List<TaskModel> tasks = new List<TaskModel>();

        static void Main(string[] args)
        {
            void AddTask()
            {
                var taskString = "";
                Console.WriteLine("Podaj dane dotyczące zadania - oddzielając je znakiem '|'");
                Console.WriteLine("(tytuł(3-10 znaków)|opis|data rozpoczęcia [xxxx-xx-xx xx:xx:xx]|całodniowe[tak/nie]|");
                Console.WriteLine("priorytet[0-Normal, 1-Important, 2-Very_Important](podaj nr)");
                taskString = Console.ReadLine();
                if (taskString != "")
                {
                    var taskStringArray = taskString.Split("|");
                    var taskOK = false;
                    var taskOKv = 1;
                    var finalDate = "";
                    var priority = 0;
                    DateTime checkedDT = DateTime.Now;
                    DateTime checkedDTF = DateTime.MinValue;
                    if (taskStringArray.Length == 5)
                    {
                        taskOK = true;
                        int.TryParse(taskStringArray[4], out priority);
                        /*if (taskStringArray[0].Length > 10 || taskStringArray[0].Length < 3)
                        {
                            taskOK = false;
                            Console.WriteLine("Tytuł musi mieć 3-10 znaków");
                        }
                        else */
                        if (taskStringArray[2].Length != 19 || !DateTime.TryParse(taskStringArray[2], out checkedDT))
                        {
                            taskOK = false;
                            ConsoleEx.WriteLine(
                                $"Data rozpoczęcia musi być w formacie YYYY-MM-DD HH:mm:ss; {taskStringArray[2].Length}; {checkedDT}",
                                ConsoleColor.Red);
                        }
                        else if (priority < 0 || priority > 2)
                        {
                            taskOK = false;
                            ConsoleEx.WriteLine("Priorytet to liczby 0-2", ConsoleColor.Red);
                        }
                        else if (taskStringArray[3].ToLower() != "tak")
                        {
                            Console.WriteLine("Podaj datę zakończenia w formacie YYYY-MM-DD HH:mm:ss");
                            finalDate = Console.ReadLine();
                            if (finalDate.Length != 19 || !DateTime.TryParse(finalDate, out checkedDTF))
                            {
                                taskOK = false;
                                ConsoleEx.WriteLine("Data rozpoczęcia musi być w formacie YYYY-MM-DD HH:mm:ss", ConsoleColor.Red);
                            }
                            else
                            {
                                taskOKv = 2;
                            }
                        }
                    }
                    else
                    {

                    }


                    if (taskOK)
                    {
                        if (taskOKv == 2)
                        {
                            var newTask = new TaskModel(taskStringArray[0], taskStringArray[1], checkedDT, priority, checkedDTF);
                            if (newTask.CorrectData)
                                tasks.Add(newTask);
                        }
                        else
                        {
                            var newTask = new TaskModel(taskStringArray[0], taskStringArray[1], checkedDT, priority);
                            if (newTask.CorrectData)
                                tasks.Add(newTask);
                        }
                    }

                    
                }
            }

            void RemoveTask()
            {
                Console.WriteLine("Podaj tytuł usuwanego zadania");
                var remove = Console.ReadLine();
                var tmpTasks = tasks;
                var i = 0;
                foreach (var t in tmpTasks)
                {
                    if(t.Name == remove)
                        tasks.RemoveAt(i);
                    i++;
                }
            }

            void ShowTasks()
            {
                List<TaskModel> VeryImportants = new List<TaskModel>();
                List<TaskModel> Importants = new List<TaskModel>();
                List<TaskModel> Normals = new List<TaskModel>();
                foreach (var t in tasks)
                {
                    Console.WriteLine($"{t.Name}; {t.dateStart}");
                }
            }
            Console.WriteLine("Witaj w Menadżerze zadań");
            Console.WriteLine("Lista dostępnych komend - wpisz '-help'");
            var command = "";
            
            do
            {
                Console.Write("Podaj komendę: ");
                command = Console.ReadLine();
                if (command.ToLower() == "add")
                {
                    AddTask();
                }

                if (command.ToLower() == "rem")
                {
                    RemoveTask();
                }

                if (command.ToLower() == "show")
                {
                    ShowTasks();
                }
            }
            while (command != "exit");

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            CorrectData = true;
            Name = name;
            Description = description;
            dateStart = start;
            prioryty = (importantFlag)prior;
            fulldayFlag = true;
        }

        public TaskModel(string name, string description, DateTime start, int prior, DateTime end)
        {
            CorrectData = true;
            Name = name;
            Description = description;
            dateStart = start;
            dateEnd = end;
            prioryty = (importantFlag)prior;
        }

        public string Export()
        {
            var csv = new StringBuilder();
            csv.Append($"{Name}|{Description}|{dateStart}|");
            if (fulldayFlag != null)
            {
                csv.Append("tak");
            }
            else
            {
                csv.Append(dateEnd);
            }

            csv.Append($"|{prioryty}");
            return csv.ToString();
        }

    }

    class Program
    {
        public string[] listOdCommands = new string[6]
        {
            "add (dodawanie zadania)", "rem (usuwanie zadania)", "show (listowanie zapisanych zadań)",
            "save (zapisywanie zadań do pliku)", "load (wczytywanie zadań z pliku",
            "close (listowanie zadań w najbliższych 5 dniach)"
        };

        public static List<TaskModel> tasks = new List<TaskModel>();

        static void Main(string[] args)
        {
            void AddTaskFromString(string taskString)
            {
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
                            if (taskStringArray[3].ToLower() == "nie")
                            {
                                Console.WriteLine("Podaj datę zakończenia w formacie YYYY-MM-DD HH:mm:ss");
                                finalDate = Console.ReadLine();
                            }
                            else
                            {
                                finalDate = taskStringArray[3];
                            }

                            if (finalDate != null && (finalDate.Length != 19 || !DateTime.TryParse(finalDate, out checkedDTF)))
                            {
                                taskOK = false;
                                ConsoleEx.WriteLine("Data rozpoczęcia musi być w formacie YYYY-MM-DD HH:mm:ss",
                                    ConsoleColor.Red);
                            }
                            else
                            {
                                taskOKv = 2;
                            }
                        }
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Zbyt mało zmiennych", ConsoleColor.Red);
                    }


                    if (taskOK)
                    {
                        if (taskOKv == 2)
                        {
                            var newTask = new TaskModel(taskStringArray[0], taskStringArray[1], checkedDT, priority, checkedDTF);
                            if (newTask.CorrectData)
                            {
                                tasks.Add(newTask);
                                ConsoleEx.WriteLine("Zadanie dodano", ConsoleColor.Green);
                            }
                            else
                            {
                                ConsoleEx.WriteLine("Nie dodano zadania2", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            var newTask = new TaskModel(taskStringArray[0], taskStringArray[1], checkedDT, priority);
                            if (newTask.CorrectData)
                            {
                                tasks.Add(newTask);
                                ConsoleEx.WriteLine("Zadanie dodano", ConsoleColor.Green);
                            }
                            else
                            {
                                ConsoleEx.WriteLine("Nie dodano zadania3", ConsoleColor.Red);
                            }
                        }
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Nie dodano zadania1", ConsoleColor.Red);
                    }
                }
            }

            void AddTask()
            {
                var taskString = "";
                Console.WriteLine("Podaj dane dotyczące zadania - oddzielając je znakiem '|'");
                Console.WriteLine("(tytuł(3-10 znaków)|opis|data rozpoczęcia [xxxx-xx-xx xx:xx:xx]|całodniowe[tak/nie]|");
                Console.WriteLine("priorytet[0-Normal, 1-Important, 2-Very_Important](podaj nr)");
                taskString = Console.ReadLine();
                AddTaskFromString(taskString);
            }

            void RemoveTask()
            {
                Console.WriteLine("Podaj tytuł usuwanego zadania");
                var remove = Console.ReadLine();
                var tmpTasks = tasks;
                var i = 0;
                foreach (var t in tmpTasks)
                {
                    if (t.Name == remove)
                    {
                        tasks.RemoveAt(i);
                        ConsoleEx.WriteLine("Zadanie usunięto", ConsoleColor.Yellow);
                    }
                    i++;
                }
            }

            void ShowTasks(int close = 0)
            {
                var tmpConsoleBackground = Console.BackgroundColor;
                var tmpForegroundColor = Console.ForegroundColor;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                var today = DateTime.Today;
                var closeDate = today.AddDays(5);
                List<TaskModel> VItasks = new List<TaskModel>();
                List<TaskModel> Itasks = new List<TaskModel>();
                List<TaskModel> Ntasks = new List<TaskModel>();
                Console.Write($"{"nazwa".PadRight(12)}| {"opis".PadRight(20)}| ");
                Console.Write($"{"Data rozpoczęcia".PadRight(21)}| {"data zakończenia".PadRight(21)}| ");
                Console.WriteLine($"{"Ważność".PadRight(15)}");
                foreach (var t in tasks)
                {
                    if ((t.fulldayFlag != null && t.dateStart >= today && t.dateStart < closeDate)
                        || (t.dateEnd >= today && t.dateEnd < closeDate) || close == 0) { 
                        if (t.prioryty.ToString() == "Very_Important")
                            VItasks.Add(t);
                        else if (t.prioryty.ToString() == "Important")
                            Itasks.Add(t);
                        else
                            Ntasks.Add(t);
                    }
            }

                for (var i = 0; i < 3; i++)
                {
                    var tmpTask = VItasks;
                    if (i == 1)
                        tmpTask = Itasks;
                    if (i == 2)
                        tmpTask = Ntasks;

                    foreach (var t in tmpTask)
                    {
                        Console.Write($"{t.Name.PadRight(12)}| {t.Description.PadRight(20)}| ");
                        Console.Write($"{t.dateStart.ToString().PadRight(21)}| ");
                        if (t.fulldayFlag != null && t.fulldayFlag == true)
                        {
                            Console.Write($"{"całodniowe".ToString().PadRight(21)}");
                        }
                        else
                        {
                            Console.Write($"{t.dateEnd.ToString().PadRight(21)}");
                        }
                        Console.WriteLine($"| {t.prioryty.ToString().PadRight(15)}");
                    }

                }
                
                Console.BackgroundColor = tmpConsoleBackground;
                Console.ForegroundColor = tmpForegroundColor;
            }

            void SaveTask()
            {
                var tasksToSave = new StringBuilder();
                foreach (var t in tasks)
                {
                    tasksToSave.Append(t.Export());
                    tasksToSave.Append("\n");
                }

                var filePath = @"E:\Data.csv";
                File.WriteAllText(filePath, tasksToSave.ToString());

                Console.WriteLine("zapisano");
            }

            void LoadTasks()
            {
                var filePath = @"E:\Data.csv";
                var textFromFile = File.ReadAllLines(filePath);
                foreach (var txt in textFromFile)
                {
                    var csv = txt.Split('|');
                    var addTask = true;
                    foreach (var t in tasks)
                    {
                        if (t.Name == csv[0])
                        {
                            addTask = false;
                        }
                    }

                    if (addTask)
                    {
                        var priority = "0";
                        if (csv[csv.Length - 1] == "Important")
                            priority = "1";
                        if (csv[csv.Length - 1] == "Very_Important")
                            priority = "2";
                        csv[csv.Length - 1] = priority;
                        AddTaskFromString(string.Join('|', csv));
                        ConsoleEx.WriteLine("Wczytano", ConsoleColor.Green);
                    }
                    else
                    {
                        ConsoleEx.WriteLine($"Nie dopisano zadania o tytule '{csv[0]}'", ConsoleColor.Red);
                        ConsoleEx.WriteLine("Zadanie o tym tytule już istnieje w liście", ConsoleColor.Red);
                    }
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

                if (command.ToLower() == "close")
                {
                    ShowTasks(5);
                }

                if (command.ToLower() == "save")
                {
                    SaveTask();
                }

                if (command.ToLower() == "load")
                {
                    LoadTasks();
                }

            }
            while (command != "exit");

        }
    }
}

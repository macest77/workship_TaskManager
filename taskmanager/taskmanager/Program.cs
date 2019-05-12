using System;
using System.Collections.Generic;

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
        public string[] listOdCommands = new string[4]
        {
            "", "", "", ""
        };
        private string _description = "";//Opis - Wymagany.
        public DateTime dateStart; //Datę Rozpoczęcia - Wymagana.
        public DateTime? dateEnd;// Datę Zakończenia - Niewymagana, jeśli zadanie jest całodniowe.
        public bool? fulldayFlag = false;//Zadanie Całodniowe - Niewymagana, domyślnie zadanie nie jest całodniowe.

        public enum importantFlag
        {
            //Zadanie Ważne - Niewymagana, domyślnie zadanie nie jest ważne.
            Normal,
            Important,
            Very_Important
        }

        public string Description
        {
            set
            {
                if (value.Length > 2)
                {
                    _description = value;
                }
                else
                {
                    ConsoleEx.WriteLine("Opis musi zawierać co najmniej 3 znaki", ConsoleColor.Red);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Witaj w Menadżerze zadań");
            Console.WriteLine("Lista dostępnych komend - wpisz '-help'");
            var command = "";
            do
            {
                Console.Write("Podaj komendę: ");
                command = Console.ReadLine();
            }
            while (command != "exit");

        }
    }
}

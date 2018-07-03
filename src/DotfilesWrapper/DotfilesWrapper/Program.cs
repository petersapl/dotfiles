using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace DotfilesWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args.Distinct())
            {
                if (File.Exists(arg) && Regex.IsMatch(Path.GetExtension(arg.ToLower()) , "^.ya?ml$"))
                {
                    switch (Path.GetFileNameWithoutExtension(arg))
                    {
                        case "commands":
                            ExecTask(new Command(arg));
                            break;
                        case "choco":
                            ExecTask(new Choco(arg));
                            break;
                    }
                }
            }

            void ExecTask(TaskBase task)
            {
                if (task.Tasks > 0)
                {
                    task.Exec();
                }
                else
                {
                    Console.WriteLine($"\"{task.FileName}\" has no value to process.");
                    Console.WriteLine($"Check if the commands in \"{task.FileName}\" are valid!");
                }
            }

            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DotfilesWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
           var _taskQueue = new Queue<TaskFactory>();

            foreach (var arg in args)
            {
                if (File.Exists(arg) && Regex.IsMatch(Path.GetExtension(arg.ToLower()) , "^.ya?ml$"))
                {
                    switch (Path.GetFileNameWithoutExtension(arg))
                    {
                        case "commands":
                            _taskQueue.Enqueue(new Command(arg));
                            break;
                        case "choco":
                            _taskQueue.Enqueue(new Choco(arg));
                            break;
                        default:
                            _taskQueue.Enqueue(new Command(arg));
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid File {arg}!");
                }
            }

            void Dequeue()
            {
                if (_taskQueue.Count > 0)
                {
                    if (_taskQueue.Peek().Tasks > 0)
                    {
                        _taskQueue.Dequeue().Exec();
                    }
                    else
                    {
                        Console.WriteLine("No value to process.");
                        Console.WriteLine("Check if the commands in the respective files are valid!");
                    }
                }
            }

            Dequeue();

            TaskFactory.OnTasksFinished += sender =>
            {
                Dequeue();
            };

            Console.ReadLine();
        }
    }
}

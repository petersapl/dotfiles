using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DotfilesWrapper
{
    class Program
    {
        private static Queue<TaskFactory> _taskQueue;
        static void Main(string[] args)
        {
            _taskQueue = new Queue<TaskFactory>();

            foreach (var arg in args)
            {
                if (Regex.IsMatch(Path.GetExtension(arg.ToLower()) , "^.ya?ml$"))
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
            }

            void Dequeue()
            {
                if (_taskQueue.Count > 0)
                {
                    _taskQueue.Dequeue().Exec();
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

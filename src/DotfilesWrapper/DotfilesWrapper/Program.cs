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
            var _taskList = new List<TaskBase>();

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
                            _taskList.Add(new Choco(arg));
                            break;
                        case "choco_dependency":
                            _taskList.Add(new ChocoDependency(arg));
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

            TaskBase.OnTasksFinished += (sender, type) =>
            {
                var task = _taskList.Where(x => x.CmdType == type).FirstOrDefault();

                if (task != null)
                    ExecTask(task);
            };

            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace DotfilesWrapper
{
    class Choco : TaskFactory
    {
        private ConcurrentQueue<string> _chocoQueue;

        public Choco(string file)
        {
            var deserialize = Serial.Deserialize<ChocoWrapper, string>(file);

            deserialize.IfPresent(val =>
            {
                _chocoQueue = new ConcurrentQueue<string>(val.Commands);
                Tasks = val.Commands.Count;
            });
        }
        public override void Exec()
        {
            if(Environment.GetCommandLineArgs().Skip(1).Contains("-csync"))
            {
                Task.Run(async () =>
                {
                    foreach (var task in _chocoQueue.Select(cmd => ExecCommand($"choco install {cmd} -y", cmd)))
                    {
                        Console.WriteLine(await task);
                        Console.WriteLine($"Task {++_status} of {Tasks} finished. {Environment.NewLine}");
                    }
                });
            }
            else
            {
                for (var i = 0; Check(i, _chocoQueue); i++)
                    ExecTask();
            }

        }

        protected override void ExecTask()
        {
            if (Check(_currentProcesses, _chocoQueue))
            {
                _chocoQueue.TryDequeue(out var cmd);

                Task.Run(async () =>
                {
                    Interlocked.Increment(ref _currentProcesses);
                    Console.WriteLine(await ExecCommand($"choco install {cmd} -y", cmd));
                    Interlocked.Increment(ref _status);
                    Console.WriteLine($"Task {_status} of {Tasks} finished. {Environment.NewLine}");
                }).ContinueWith(x =>
                {
                    Interlocked.Decrement(ref _currentProcesses);
                    ExecTask();
                });
            }
        }

        internal class ChocoWrapper : ICommandable<string>
        {
            [YamlMember(Alias = "choco")]
            public List<string> Commands { get; set; }
        }
    }
}

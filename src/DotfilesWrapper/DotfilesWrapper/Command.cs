using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace DotfilesWrapper
{
    class Command : TaskFactory
    {
        private ConcurrentQueue<CommandWrapper.CommandTuple> _commandQueue;
        public Command(string file)
        {
            var deserialize = Serial.Deserialize<CommandWrapper>(file);

            deserialize.IfPresent(val =>
            {
                _commandQueue = new ConcurrentQueue<CommandWrapper.CommandTuple>(val.Commands);
                Tasks = val.Commands.Count;
            });
        }

        public override void Exec()
        {
            for (var i = 0; Check(i, _commandQueue); i++)
                ExecTask();
        }

        protected override void ExecTask()
        {
            if (Check(_currentProcesses, _commandQueue))
            {
                _commandQueue.TryDequeue(out var cmd);
                Task.Run(async () =>
                {
                    Interlocked.Increment(ref _currentProcesses);
                    Console.WriteLine(await ExecCommand(string.Join(" && ", cmd.Cmd ?? (new[] { "" })), cmd.Desc, cmd.Path));
                    Interlocked.Increment(ref _status);
                    Console.WriteLine($"Task {_status} of {Tasks} finished. {Environment.NewLine}");
                }).ContinueWith(x =>
                {
                    Interlocked.Decrement(ref _currentProcesses);
                    ExecTask();
                });
            }
        }

        internal class CommandWrapper
        {
            internal class CommandTuple
            {
                [YamlMember(Alias = "cmd")]
                public string[] Cmd { get; set; }
                [YamlMember(Alias = "path")]
                public string Path { get; set; }
                [YamlMember(Alias = "desc")]
                public string Desc { get; set; }
            }

            [YamlMember(Alias = "commands")]
            public List<CommandTuple> Commands { get; set; }
        }
    }
}

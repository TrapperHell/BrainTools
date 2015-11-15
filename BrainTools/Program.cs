using ManyConsole;
using System;
using System.Collections.Generic;

namespace BrainTools
{
    static class Program
    {
        static int Main(string[] args)
        {
#if DEBUG
            Tests.RunTests();
#endif

            List<ConsoleCommand> commands = new List<ConsoleCommand>()
            {
                new RunCommand(),
                new EncodeCommand(),
                new DecodeCommand(),
                new EnlargeCommand(),
                new ReduceCommand()
            };

            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}
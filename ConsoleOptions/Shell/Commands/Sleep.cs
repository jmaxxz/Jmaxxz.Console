using ConsoleOptions;
using System.Threading;

namespace Jmaxxz.ShellHelpers
{
    public class Sleep : ICommand
    {
        public int Run(Cli console,string[] args)
        {
            int time =0;
            Options opts = new Options("Waits for a period of time")
            {
                new Option((int t)=>time =t,"WaitTime","Time in milliseconds to sleep for")
            };
            opts.Parse(args);
            Thread.Sleep(time);
            return 0;
        }
    }
}


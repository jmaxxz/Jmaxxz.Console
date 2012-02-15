using System;

namespace Jmaxxz.ShellHelpers
{
    public class Beep: ICommand
    {
        public int Run(Cli console,string[] args)
        {
            Console.Beep();
            return 0;
        }
    }
}


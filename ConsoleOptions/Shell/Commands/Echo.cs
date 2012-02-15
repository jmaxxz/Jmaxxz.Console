using System;

namespace Jmaxxz.ShellHelpers
{
    public class Echo : ICommand
    {
        public int Run(Cli console,string[] args)
        {
            console.Out.WriteLine(string.Join(" ",args));
            return 0;
        }
    }
}


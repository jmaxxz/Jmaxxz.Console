using System;

namespace Jmaxxz.ShellHelpers
{
    public interface ICommand
    {
        int Run(Cli console,string[] args);
    }
}


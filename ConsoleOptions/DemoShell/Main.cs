using System;
using Jmaxxz.ShellHelpers;

namespace DemoShell
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var s = new Shell(()=>"jmaxxzshell> ")
            {
                {"echo", new Echo()},
                {"sleep", new Sleep()},
                {"pause", new Pause("Press any key to continue...")},
                {"beep", new Beep()},
                {"wget", new Download()},
            };
            s.Add("exit", new Exit(s));


            s.Start(new Cli(Console.In, Console.Out));
        }
    }
}

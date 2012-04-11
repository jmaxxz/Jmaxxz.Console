namespace Jmaxxz.ShellHelpers
{
    public class Exit: ICommand
    {
        private readonly Shell _shell;
        public Exit(Shell s)
        {
            _shell =s;
        }
        public int Run(Cli console,string[] args)
        {
            _shell.Stop();
            return 0;
        }
    }
}


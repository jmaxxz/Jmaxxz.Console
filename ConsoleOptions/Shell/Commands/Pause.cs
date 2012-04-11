namespace Jmaxxz.ShellHelpers
{
    public class Pause: ICommand
    {
        private readonly string _waitText;
        public Pause(string waitText)
        {
            _waitText =waitText;
        }
        public int Run(Cli console,string[] args)
        {
            console.Out.Write(_waitText);
            console.In.Read();
            console.Out.WriteLine();
            return 0;
        }
    }
}


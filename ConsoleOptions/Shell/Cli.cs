using System.IO;

namespace Jmaxxz.ShellHelpers
{ 
    public class Cli
    {
        public TextWriter Out { get; private set; }
        public TextReader In { get;  private set; }

        public Cli(TextReader input, TextWriter output)
        {
            Out = output;
            In = input;
        }
    }
}


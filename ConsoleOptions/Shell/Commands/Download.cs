using System;
using ConsoleOptions;

namespace Jmaxxz.ShellHelpers
{
    public class Download: ICommand
    {
        public int Run(Cli console,string[] args)
        {
            string source = null;
            string local  = null;
            Options opts = new Options("Downloads a specified file")
            {
                new Option((string s)=>source =s,"address","The source address of the file to be downloaded"),
                new Option((string l)=>local =l,"localFile","Save the remote file as this name"),
            };
            opts.Parse(args);
            if(source == null)
            {
                return 1;
            }
            using(var client = new System.Net.WebClient())
            {
                if(local !=null)
                {
                    client.DownloadFile(new Uri(source),local);
                }
                else
                {
                    var result = client.DownloadString(new Uri(source));
                    console.Out.WriteLine(result);
                }
            }
            return 0;
        }
    }
}


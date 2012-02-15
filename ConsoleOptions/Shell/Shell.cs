using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Jmaxxz.ShellHelpers
{
    public class Shell : IEnumerable<KeyValuePair<string,ICommand>>
    {
        private IDictionary<string,ICommand> _commands;
        private Func<string> _prompt;

        //This should be moved out into a state object
        private bool _running = false;
        public Shell (Func<string> prompt)
        {
            _prompt = prompt;
            _commands = new Dictionary<string,ICommand>();
        }

        public void Add(string key, ICommand command)
        {
            _commands.Add(key, command);
        }


        public void Stop ()
        {
            _running = false;
            //Will kill after next line is hit...this is non-ideal. Should use a thread based model
        }
        public void Start(Cli cli)
        {
            var input = cli.In;
            var output = cli.Out;
            _running = true;
            while(_running)
            {
                cli.Out.Write(_prompt.Invoke());
                var nextLine = input.ReadLine();
                var parsedInput = SlitCliInput(nextLine);
                if(!parsedInput.Any())
                {
                    continue; //No commands here
                }

                string command = parsedInput.First();
                string[] args = parsedInput.Skip(1).ToArray();
                if(_commands.ContainsKey(command))
                {
                    try
                    {
                        _commands[command].Run(cli,args);
                    }
                    catch(Exception ex)
                    {
                        output.WriteLine(ex);
                    }
                }
                else
                {
                    output.WriteLine("Unknown command");
                }


            }
        }

        private static String[] SlitCliInput(string line)
        {
            return line.Split(new []{' '},StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerator<KeyValuePair<string,ICommand>> GetEnumerator ()
        {
            return _commands.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return (IEnumerator)_commands.GetEnumerator ();
        }
    }
}


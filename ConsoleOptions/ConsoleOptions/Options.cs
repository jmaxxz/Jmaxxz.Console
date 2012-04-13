using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace ConsoleOptions
{
    public class Options : IEnumerable<Option>
    {
        private readonly IList<Option> _options;
        private readonly string _description;
        private readonly string _name;
        private readonly Option _showUsage;

        public Options (): this("")
        {
        }

        public Options (string description)
        {
            _description = description;
            _name = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            _options = new List<Option> ();
            
            _showUsage = new Option(new[] {"?", "help"}, () => PrintUsage(Console.Out, Console.WindowWidth),"Prints usage for " + _name);
            _options.Add(_showUsage);
        }

        public void Add (Option o)
        {
            //Eventually should scan existing options for collisions, or use a dictionary for options
            _options.Add (o);
        }
        public bool Parse( string[] args)
        {
            return Parse(Console.Error, args);
        }

        public bool Parse(TextWriter stderr, string[] args)
        {
            var copyOfOptions = _options.ToList();
            //Return false if something goes wrong (for now nothing can go wrong)
            //This loop is naive, the array should be flattened and then a regex should be used to extract the parameters
            for (int i = 0; i < args.Length; i++)
            {
                string nextArg = i + 1 < args.Length ? args[i + 1] : null;
                string arg = args[i];
                bool usedNextArg = false;
                bool parsedSuccessful;
                //Check for flag indicator (-,--,/)
                //This is the simplest of all cases the entire arg is taken as a literal 
                if (arg.StartsWith ("--") && arg.Length >= 3)
                {
                    parsedSuccessful = HandleLiteralFlag(stderr, arg.Substring(2), nextArg, copyOfOptions, out usedNextArg);
                //We need to decend into this string to handle all the possible switches. (only the last switch can take a value)
                }
                else if (arg.StartsWith("-") && arg.Length >= 2 && !arg.StartsWith("--"))
                {
                    parsedSuccessful = HandleQuickFlags(stderr, arg.Substring(1), nextArg, copyOfOptions, out usedNextArg);

                // This is a data arg
                } 
                else
                {
                    parsedSuccessful = HandleParameter(arg, copyOfOptions);
                }
                if (!parsedSuccessful)
                {
                    return false;
                }
                if (usedNextArg)
                {
                    i++;
                    //Advance and extra index
                }
            }
            return true;
        }

        public void PrintUsage()
        {
            PrintUsage(Console.Out, Console.WindowWidth);
        }
        public void PrintUsage(TextWriter stdOut, int consoleWidth)
        {
            //Write out highlevel description
            var sortedOpts = _options.Where(x=>!x.IsFlagless).Concat(_options.Where(x=>x.IsFlagless));
            var syntax = string.Join(" ",sortedOpts.Select(x=>x.GetUsageSyntax()).ToArray());
            stdOut.WriteLine("Usage: {0} {1}", _name,syntax);
            if(_description != "")stdOut.WriteLine("Description: {0}", _description);

            stdOut.WriteLine(new string('=',consoleWidth));
            stdOut.Write("Flags");
            stdOut.Write(new string(' ',30));
            stdOut.Write("|  ");
            stdOut.WriteLine("Descriptions");
            stdOut.WriteLine(new string('_',consoleWidth));

            foreach(var opt in sortedOpts)
            {
                opt.PrintUsage(Console.Out,35, consoleWidth);
                stdOut.WriteLine(new string('-',consoleWidth));
            }

        }

        private bool HandleQuickFlags (TextWriter stderr,string flags, string nextVal,ICollection<Option> opts, out bool usedNextArg)
        {
            foreach (char flag in flags.Substring (0, flags.Length - 1))
            {
                bool junk;
                if (!HandleLiteralFlag (stderr,flag.ToString (), null,opts, out junk))
                {
                    usedNextArg = false;
                    return false;
                }
            }
            return HandleLiteralFlag (stderr, flags[flags.Length - 1].ToString (), nextVal,opts, out usedNextArg);
        }

        private bool HandleLiteralFlag (TextWriter stderr, string flag, string nextVal,ICollection<Option> opts, out bool usedNextArg)
        {
            foreach (var opt in opts)
            {
                if (opt.IsMatch (flag))
                {
                    usedNextArg = opt.UsesValue;
                    opts.Remove(opt);
                    return (usedNextArg ? opt.Invoke(flag, nextVal) : opt.Invoke(flag, "")) && opt != _showUsage;
                }
            }
            usedNextArg = false;
            return false;
        }
        
        private bool HandleParameter(string data, IList<Option> opts)
        {
            foreach (var opt in opts)
            {
                if (opt.IsFlagless)
                {
                    opts.Remove(opt);
                    return opt.Invoke("NoFlag", data) && opt != _showUsage;
                }
            }
            return false;
        }

        public IEnumerator<Option> GetEnumerator ()
        {
            return _options.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _options.GetEnumerator ();
        }
    }
}


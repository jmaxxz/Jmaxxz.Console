using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace ConsoleOptions
{
    public class Options : IEnumerable<Option>
    {
        private IList<Option> _options;

        public Options ()
        {
            _options = new List<Option> ();
        }

        public void Add (Option o)
        {
            //Eventually should scan existing options for collisions, or use a dictionary for options
            _options.Add (o);
        }
        
        //public bool Parse(string[] args)
        //{
        //            string rawString = string.Join(" ",args);
        //}
        //private bool InnerParse (string[] args)
        //{
        public bool Parse(string[] args)
        {
            var copyOfOptions = _options.ToList();
            //Return false if something goes wrong (for now nothing can go wrong)
            //This loop is naive, the array should be flattened and then a regex should be used to extract the parameters
            for (int i = 0; i < args.Length; i++)
            {
                string nextArg = i + 1 < args.Length ? args[i + 1] : null;
                string arg = args[i];
                bool usedNextArg = false;
                //Check for flag indicator (-,--,/)
                //This is the simplest of all cases the entire arg is taken as a literal 
                if (arg.StartsWith ("--"))
                {
                    HandleLiteralFlag (arg.Substring (2), nextArg,copyOfOptions, out usedNextArg);
                //We need to decend into this string to handle all the possible switches. (only the last switch can take a value)
                } 
                else if (arg.StartsWith ("-") || arg.StartsWith ("/"))
                {
                    HandleQuickFlags (arg.Substring (1), nextArg,copyOfOptions, out usedNextArg);
                // This is a data arg
                } 
                else
                {
                    HandleParameter(arg, copyOfOptions);
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
            Console.WriteLine(new string('=',Console.WindowWidth-Console.CursorLeft));
            Console.Write("Flags");
            Console.Write(new string(' ',30));
            Console.Write("|  ");
            Console.WriteLine("Descriptions");
            Console.WriteLine(new string('_',Console.WindowWidth));

            foreach(var opt in _options)
            {
                opt.PrintUsage(35);
                Console.WriteLine(new string('-',Console.WindowWidth));
            }

        }

        private bool HandleQuickFlags (string flags, string nextVal,IList<Option> opts, out bool usedNextArg)
        {
            foreach (char flag in flags.Substring (0, flags.Length - 1))
            {
                bool junk;
                if (!HandleLiteralFlag (flag.ToString (), null,opts, out junk))
                {
                    usedNextArg = false;
                    return false;
                }
            }
            return HandleLiteralFlag (flags[flags.Length - 1].ToString (), nextVal,opts, out usedNextArg);
        }

        private bool HandleLiteralFlag (string flag, string nextVal,IList<Option> opts, out bool usedNextArg)
        {
            foreach (var opt in opts)
            {
                if (opt.IsMatch (flag))
                {
                    usedNextArg = opt.UsesValue;
                    opts.Remove(opt);
                    return usedNextArg ? opt.Invoke (flag, nextVal) : opt.Invoke (flag, "");
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
                    return opt.Invoke ("NoFlag", data);
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
            return (IEnumerator)_options.GetEnumerator ();
        }
    }
}


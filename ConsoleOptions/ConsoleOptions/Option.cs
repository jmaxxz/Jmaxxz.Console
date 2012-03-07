using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;


namespace ConsoleOptions
{
    public class Option
    {
        private readonly Func<string, string,TextWriter, bool> command;
        private delegate bool TryParse<T> (string s, out T val);
        private delegate void ConvertFailedHandler (TextWriter stdErr,string val);
        private readonly string[] flags;
        public bool UsesValue { get; private set; }
        public bool IsFlagless { get; private set; }
        public readonly string _description;
        public readonly string _valueName;

        //Start of flagless params
        public Option (Action<int> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<double> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<string> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<DateTime> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<TimeSpan> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<long> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<byte> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<short> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }

        public Option (Action<bool> a,string valueName, string description) : this(Enumerable.Empty<string>(), a,valueName, description)
        {
        }
        //END of flagless params

        //This is somewhat of a special case because no input is used.
        public Option (IEnumerable<string> flags, Action a, string description) : this(flags, GetHandler ((string s) => a ()), false,"",description)
        {
        }

        public Option (IEnumerable<string> flags, Action<int> a, string valueName, string description) : this(flags, GetHandler (int.TryParse, a, (e,v) => e.WriteLine ("'{0}' could not be converted to an interger.", v)), valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<double> a, string valueName, string description) : this(flags, GetHandler (double.TryParse, a, (e,v) => e.WriteLine ("'{0}' could not be converted to a double.", v)),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<string> a, string valueName, string description) : this(flags, GetHandler (a),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<DateTime> a, string valueName, string description) : this(flags, GetHandler (DateTime.TryParse, a,  (e,v) => e.WriteLine ("'{0}' could not be converted to a date/time.", v)),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<TimeSpan> a, string valueName, string description) : this(flags, GetHandler (TimeSpan.TryParse, a, (e,v) => e.WriteLine ("'{0}' could not be converted to a timespan.", v)),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<long> a, string valueName, string description) : this(flags, GetHandler (long.TryParse, a, (e,v) => e.WriteLine ("'{0}' could not be converted to a long.", v)),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<byte> a, string valueName, string description) : this(flags, GetHandler (byte.TryParse, a, (e,v) => e.WriteLine ("'{0}' could not be converted to a long.", v)),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<short> a, string valueName, string description) : this(flags, GetHandler (short.TryParse, a, (e,v) => e.WriteLine ("'{0}' could note be converted to a short.", v)),valueName,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<bool> a, string valueName, string description) : this(flags, GetHandler (bool.TryParse, a, (e,v) => e.WriteLine ("'{0}' was invalid, expected 'True' or 'False'.", v)),valueName,description)
        {
        }

        public bool IsMatch (string flag)
        {
            return flags.Contains (flag);
        }

        public bool Invoke (string flag, string val)
        {
            return Invoke(flag, val, Console.Error);
        }

        public bool Invoke (string flag, string val, TextWriter stdErr)
        {
            return command (flag, val, stdErr);
        }

        public string GetUsageSyntax()
        {
            string valueText ="";
            if(this.UsesValue)valueText=" "+_valueName;

            if(!IsFlagless)
            {
                var primaryFlag = flags.First();
                var qualifier = primaryFlag.Length >1? "--" : "-";

                return string.Format("[{0}{1}{2}]",qualifier, primaryFlag,valueText);
            }

            return string.Format("<{0}>", valueText.Trim());
        }

        //This really does not belong here, I think this is really part Options
        public void PrintUsage(TextWriter stdout,int dividerColumn, int consoleWidth)
        {
            var winWidth = consoleWidth;

            int descStart = dividerColumn+3;
            if(winWidth <= descStart)
            {
                //Can not even fit a single char on the console....
                //Give up and just pretend the console is twice the size of the descStart
                winWidth = descStart *2;
            }
            var offset= 0;
            var flagsOffset= 0;

            while(offset < _description.Length || flagsOffset < flags.Length)
            {
                string qualifiedFlag = "";
                int charsUsedInFirstColumn =0;
                if(flagsOffset < flags.Length)
                {
                    string currentFlag = flags[flagsOffset];
                    qualifiedFlag = (currentFlag.Length>1 ? "--": "-")+currentFlag;
                    stdout.Write(qualifiedFlag);
                    flagsOffset++;
                    charsUsedInFirstColumn = qualifiedFlag.Length;

                }else if(IsFlagless && offset ==0)
                {
                    stdout.Write("<{0}>",_valueName);
                    charsUsedInFirstColumn = _valueName.Length+2;
                }
                stdout.Write(new string(' ', dividerColumn-charsUsedInFirstColumn));
                stdout.Write("|  ");
                var nextRow = new string(_description.Skip(offset).Take(winWidth- descStart).ToArray());
                stdout.WriteLine(nextRow);
                offset += nextRow.Length;
            }
        }


        /// <summary>
        /// Gets an option handler for a option that takes a non string type as an input
        /// </summary>
        /// <param name="parser">
        /// A <see cref="TryParse<T>"/>
        /// </param>
        /// <param name="a">
        /// A <see cref="Action<T>"/>
        /// </param>
        /// <returns>
        /// A <see cref="Func<System.String, System.String, System.Boolean>"/>
        /// </returns>
        private static Func<string, string,TextWriter, bool> GetHandler<T> (TryParse<T> parser, Action<T> a, ConvertFailedHandler convertFailed)
        {
            Func<string,TextWriter, bool> parsedAction = (string s, TextWriter stdErr) =>
            {
                // convert to input type from string and then invoke
                T val;
                if (parser (s, out val))
                {
                    a (val);
                    return true;
                }
                convertFailed (stdErr,s);
                return false;
            };
            return GetHandler (parsedAction);
        }

        /// <summary>
        /// Gets an option handler for a option that takes a string as an input.
        /// </summary>
        /// <param name="a">
        /// A <see cref="Action<System.String>"/>
        /// </param>
        /// <returns>
        /// A <see cref="Func<System.String, System.String, System.Boolean>"/>
        /// </returns>
        private static Func<string, string,TextWriter, bool> GetHandler (Action<string> a)
        {
            Func<string,TextWriter, bool> parsedAction = (string s,TextWriter stdErr) =>
            {
                a (s);
                return true;
            };
            return GetHandler (parsedAction);
        }

        private static Func<string, string,TextWriter, bool> GetHandler (Func<string,TextWriter, bool> valueHandler)
        {
            return (string f, string v, TextWriter stdError) =>
            {
                //Try for a flag
                try
                {
                    return valueHandler (v,stdError);
                }
                catch
                {
                    return false;
                }
            };
        }

        private Option (IEnumerable<string> flags, Func<string, string,TextWriter, bool> command, string valueName, string description) : this(flags, command, true,valueName, description)
        {
        }

        private Option (IEnumerable<string> flags, Func<string, string,TextWriter, bool> command, bool usesValue,string valueName, string description)
        {
            _description = description;
            _valueName = valueName;
            this.command = command;
            this.flags = flags.ToArray ();
            this.IsFlagless = !flags.Any();
            this.UsesValue = usesValue;
        }
    }
}


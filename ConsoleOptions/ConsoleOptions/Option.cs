using System;
using System.Linq;
using System.Collections.Generic;


namespace ConsoleOptions
{
    public class Option
    {
        private readonly Func<string, string, bool> command;
        private delegate bool TryParse<T> (string s, out T val);
        private delegate void ConvertFailedHandler (string val);
        private readonly string[] flags;
        public bool UsesValue { get; private set; }
        public bool IsFlagless { get; private set; }
        public readonly string _description;

        //Start of flagless params
        public Option (Action a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<int> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<double> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<string> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<DateTime> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<TimeSpan> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<long> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<byte> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<short> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        public Option (Action<bool> a, string description) : this(Enumerable.Empty<string>(), a, description)
        {
        }

        //END of flagless params

        //This is somewhat of a special case because no input is used.
        public Option (IEnumerable<string> flags, Action a, string description) : this(flags, GetHandler ((string s) => a ()), false,description)
        {
        }

        public Option (IEnumerable<string> flags, Action<int> a, string description) : this(flags, GetHandler (int.TryParse, a, v => Console.Error.WriteLine ("'{0}' could not be converted to an interger.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<double> a, string description) : this(flags, GetHandler (double.TryParse, a, v => Console.Error.WriteLine ("'{0}' could not be converted to a double.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<string> a, string description) : this(flags, GetHandler (a),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<DateTime> a, string description) : this(flags, GetHandler (DateTime.TryParse, a, v => Console.Error.WriteLine ("'{0}' could not be converted to a date/time.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<TimeSpan> a, string description) : this(flags, GetHandler (TimeSpan.TryParse, a, v => Console.Error.WriteLine ("'{0}' could not be converted to a timespan.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<long> a, string description) : this(flags, GetHandler (long.TryParse, a, v => Console.Error.WriteLine ("'{0}' could not be converted to a long.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<byte> a, string description) : this(flags, GetHandler (byte.TryParse, a, v => Console.Error.WriteLine ("'{0}' could not be converted to a long.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<short> a, string description) : this(flags, GetHandler (short.TryParse, a, v => Console.Error.WriteLine ("'{0}' could note be converted to a short.", v)),description)
        {
        }

        public Option (IEnumerable<string> flags, Action<bool> a, string description) : this(flags, GetHandler (bool.TryParse, a, v => Console.Error.WriteLine ("'{0}' was invalid, expected 'True' or 'False'.", v)),description)
        {
        }

        public bool IsMatch (string flag)
        {
            return flags.Contains (flag);
        }

        public bool Invoke (string flag, string val)
        {
            return command (flag, val);
        }

        public void PrintUsage(int dividerColumn)
        {
            var winWidth = Console.WindowWidth;
            int flagSpace = dividerColumn-2;
            int descStart = dividerColumn+3;

            var offset= 0;
            var flagsOffset= 0;
            while(offset < _description.Length || flagsOffset < flags.Length)
            {
                var y = Console.CursorTop;
                string qualifiedFlag = "";
                if(flagsOffset < flags.Length)
                {
                    string currentFlag = flags[flagsOffset];
                    qualifiedFlag = (currentFlag.Length>1 ? "--": "-")+currentFlag;
                    Console.Write(qualifiedFlag);
                    flagsOffset++;

                }
                Console.Write(new string(' ', dividerColumn-qualifiedFlag.Length));
                Console.Write("|  ");
                var nextRow = new string(_description.Skip(offset).Take(winWidth- descStart).ToArray());
                Console.WriteLine(nextRow);
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
        private static Func<string, string, bool> GetHandler<T> (TryParse<T> parser, Action<T> a, ConvertFailedHandler convertFailed)
        {
            Func<string, bool> parsedAction = (string s) =>
            {
                // convert to input type from string and then invoke
                T val;
                if (parser (s, out val))
                {
                    a (val);
                    return true;
                }
                convertFailed (s);
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
        private static Func<string, string, bool> GetHandler (Action<string> a)
        {
            Func<string, bool> parsedAction = (string s) =>
            {
                a (s);
                return true;
            };
            return GetHandler (parsedAction);
        }

        private static Func<string, string, bool> GetHandler (Func<string, bool> valueHandler)
        {
            return (string f, string v) =>
            {
                //Try for a flag
                try
                {
                    return valueHandler (v);
                }
                catch
                {
                    return false;
                }
            };
        }

        private Option (IEnumerable<string> flags, Func<string, string, bool> command, string description) : this(flags, command, true, description)
        {
        }

        private Option (IEnumerable<string> flags, Func<string, string, bool> command, bool usesValue, string description)
        {
            _description = description;
            this.command = command;
            this.flags = flags.ToArray ();
            this.IsFlagless = !flags.Any();
            this.UsesValue = usesValue;
        }
    }
}


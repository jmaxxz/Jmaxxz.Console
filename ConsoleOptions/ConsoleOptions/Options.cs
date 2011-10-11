using System;
using System.Collections;
using System.Collections.Generic;
namespace ConsoleOptions
{
	public class Options : IEnumerable<Option>
	{
		private IList<Option> _options;

		/*
	     * The following cases eventually need to be handled, but for now they will be ignored:
	     * 1) Flagless options (a.k.a. nonoptional data parameters)
	     * 2) Values with whitespace
	     * 3) Do not run options more than once if they are specified more than once
		 */
		public Options ()
		{
			_options = new List<Option> ();
		}

		public void Add (Option o)
		{
			//Eventually should scan existing options for collisions, or use a dictionary for options
			_options.Add (o);
		}

		public bool Parse (string[] args)
		{
			//Return false if something goes wrong (for now nothing can go wrong)
			//This loop is naive, the array should be flattened and then a regex should be used to extract the parameters
			for (int i = 0; i < args.Length; i++)
			{
				string nextArg = i + 1 < args.Length ? args[i + 1] : null;
				string arg = args[i];
				bool usedNextArg = false;
				//Check for flag indicator (-,--,/,\)
				//This is the simplest of all cases the entire arg is taken as a literal 
				if (arg.StartsWith ("--"))
				{
					HandleLiteralFlag (arg.Substring (2), nextArg, out usedNextArg);
				//We need to decend into this string to handle all the possible switches. (only the last switch can take a value)
				} else if (arg.StartsWith ("-") || arg.StartsWith ("/"))
				{
					HandleQuickFlags (arg.Substring (1), nextArg, out usedNextArg);
				// This is a data arg
				} else
				{
					
				}
				
				if (usedNextArg)
				{
					i++;
					//Advance and extra index
				}
			}
			return true;
		}

		private bool HandleQuickFlags (string flags, string nextVal, out bool usedNextArg)
		{
			foreach (char flag in flags.Substring (0, flags.Length - 1))
			{
				bool junk;
				if (!HandleLiteralFlag (flag.ToString (), null, out junk))
				{
					usedNextArg = false;
					return false;
				}
			}
			return HandleLiteralFlag (flags[flags.Length - 1].ToString (), nextVal, out usedNextArg);
		}

		private bool HandleLiteralFlag (string flag, string nextVal, out bool usedNextArg)
		{
			foreach (var opt in _options)
			{
				if (opt.IsMatch (flag))
				{
					usedNextArg = opt.UsesValue;
					return usedNextArg ? opt.Invoke (flag, nextVal) : opt.Invoke (flag, "");
				}
			}
			usedNextArg = false;
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


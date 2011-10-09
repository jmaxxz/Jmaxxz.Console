using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleOptions
{
	public class Option
	{
		private Func<string, string, bool> command;
		private delegate bool TryParse<T>(string s,out T val);
		private string[] flags;
		public bool UsesValue{get; private set;}
		
		public Option (IEnumerable<string> flags, Action a) : this(flags, GetHandler((string s)=>a()), false)//This is somewhat of a special case because no input is used.
		{}
		
		public Option(IEnumerable<string> flags, Action<int> a): this(flags, GetHandler(int.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<double> a): this(flags, GetHandler(double.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<string> a): this(flags, GetHandler(a))
		{}
		
		public Option(IEnumerable<string> flags, Action<DateTime> a): this(flags, GetHandler(DateTime.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<TimeSpan> a): this(flags, GetHandler(TimeSpan.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<long> a): this(flags, GetHandler(long.TryParse, a))
		{}		
		
		public Option(IEnumerable<string> flags, Action<byte> a): this(flags, GetHandler(byte.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<short> a): this(flags, GetHandler(short.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<bool> a): this(flags, GetHandler(bool.TryParse, a))
		{}
		
		
		public bool IsMatch(string flag)
		{
			return flags.Contains(flag);	
		}
		
		public bool Invoke(string flag, string val)
		{
			return command(flag, val);	
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
		private static Func<string,string, bool> GetHandler<T>(TryParse<T>parser, Action<T> a)
		{
			Func<string, bool> parsedAction= (string s) => { // convert to input type from string and then invoke
										T val;
										if(parser(s, out val))
										{
											a(val);
											return true;
										}
										return false;
									};
			return GetHandler(parsedAction);
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
		private static Func<string,string, bool> GetHandler(Action<string> a)
		{
			Func<string, bool> parsedAction= (string s) => {
											a(s);
											return true;
									};
			return GetHandler(parsedAction);
		}
		
		private static Func<string, string, bool> GetHandler(Func<string, bool> valueHandler)
		{
			return (string f, string v) =>{ //Try for a flag
												try 
												{
													return valueHandler(v);
												} 
												catch (Exception ex) 
												{
													return false;
												}
											};
		}

		private Option (IEnumerable<string> flags, Func<string, string, bool> command) : this(flags, command, true)
		{}
		
		private Option (IEnumerable<string> flags, Func<string, string, bool> command, bool usesValue)
		{
			this.command = command;
			this.flags = flags.ToArray();
			this.UsesValue = usesValue;
		}
	}
}


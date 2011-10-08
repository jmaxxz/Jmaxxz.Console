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
		
		public Option (IEnumerable<string> flags, Action a) : this(flags, GetHandler((string s)=>a()))
		{}
		
		public Option(IEnumerable<string> flags, Action<int> a): this(flags, GetHandler(int.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<double> a): this(flags, GetHandler(double.TryParse, a))
		{}
		
		public Option(IEnumerable<string> flags, Action<Guid> a): this(flags, GetHandler(Guid.TryParse, a))
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
		
		
		
		
		public bool Invoke(string flag, string val)
		{
			return command(flag, val);	
		}
		
		
		
		
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
		
		private static Func<string,string, bool> GetHandler(Action<string> a)
		{
			Func<string, bool> parsedAction= (string s) => { // convert to input type from string and then invoke
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
		
		private Option (IEnumerable<string> flags, Func<string, string, bool> command)
		{
			this.command = command;
			this.flags = flags.ToArray();
		}
	}
}


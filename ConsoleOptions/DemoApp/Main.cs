using System;
using ConsoleOptions;

namespace DemoApp
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var opt = new Option(new []{"h","talk"},()=>Console.WriteLine("hello world"));
			opt.Invoke("h",null);
					
			var opt2 = new Option(new []{"a","add3"},(int i)=>Console.WriteLine(i+3));
			opt2.Invoke("h","10000");
			
			Console.ReadLine();
		}
	}
}


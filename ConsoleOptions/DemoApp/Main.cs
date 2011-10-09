using System;
using ConsoleOptions;

namespace DemoApp
{
	class MainClass
	{
		//this is the driver method that I am using to test the options class as it is being written, needs to be replaced with a good unit test suite
		public static void Main (string[] args)
		{
			var opt = new Option(new []{"h","talk"},()=>Console.WriteLine("hello world"));
			opt.Invoke("h",null);
					
			var opt2 = new Option(new []{"a","add3"},(int i)=>Console.WriteLine(i+3));
			opt2.Invoke("a","10000");
			
			Options opts = new Options
			{
				opt,
				opt2,
			};
			opts.Parse(new []{"--h","--add3","6"});
			opts.Parse(new []{"-ha","7"});
			Console.ReadLine();
		}
	}
}


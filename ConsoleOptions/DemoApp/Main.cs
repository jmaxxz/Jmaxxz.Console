using System;
using ConsoleOptions;

namespace DemoApp
{
    class MainClass
    {
        //this is the driver method that I am using to test the options class as it is being written, needs to be replaced with a good unit test suite
        public static void Main (string[] args)
        {
            var opt = new Option(new []{"h","talk"},()=>Console.WriteLine("hello world"),"Prints greeting");
            var opt2 = new Option(new []{"a","add3"},(int i)=>Console.WriteLine(i+3),"Adds 3 to value");
            var opt3 = new Option(new []{"x","so","dsdsdsajdlasdsjkdjask","ThisIsAnotherOne","YetAnotherOne","ItGoesOn", "d"},()=>Console.WriteLine("Goodbye World"),"This is a test of word wrapping if everything goes ok this shoudl wrap around on to several lines while maintaining an offset for other text. If this is not wrapped correctly something is wrong...very wrong.");

            Options opts = new Options
            {
                opt,
                opt2,
                opt3
            };
            opts.PrintUsage();
            opt.Invoke("h",null);
            opt2.Invoke("a","10000");
            opts.Parse(new []{"--h","--add3","6"});
            opts.Parse(new []{"-ha","7"});
            opts.Parse(new []{"-a","this is fail"});
            Console.ReadLine();
        }
    }
}


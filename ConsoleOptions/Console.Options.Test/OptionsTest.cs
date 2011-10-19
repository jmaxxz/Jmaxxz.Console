using System;
using NUnit.Framework;
using ConsoleOptions;

namespace ConsoleOptions.Test
{
	[TestFixture]
	public class OptionsTest
	{
		[Test]
		public void SimpleOptionRan()
		{
			var opt1Ran =false;
			var opt2Ran =false;
			Options ops = new Options
			{
				new Option(new []{"r","run"}, ()=>opt1Ran=true),
				new Option(new []{"d","dont"}, ()=>opt2Ran=true),
			};
			
			ops.Parse(new []{"-r"});
			
			Assert.IsTrue(opt1Ran, "Option1 did not run");
			Assert.IsFalse(opt2Ran, "Option2 ran");
		}
		
		[Test]
		public void Simple2OptionRan()
		{
			var opt1Ran =false;
			var opt2Ran =false;
			Options ops = new Options
			{
				new Option(new []{"r","run"}, ()=>opt1Ran=true),
				new Option(new []{"d","dont"}, ()=>opt2Ran=true),
			};
			
			ops.Parse(new []{"-r","--dont"});
			
			Assert.IsTrue(opt1Ran, "Option1 did not run");
			Assert.IsTrue(opt2Ran, "Option2 did not run");
		}
		
		[Test]
		public void WindowsStyleFlag()
		{
			var opt1Ran =false;
			var opt2Ran =false;
			Options ops = new Options
			{
				new Option(new []{"r","run"}, ()=>opt1Ran=true),
				new Option(new []{"d","dont"}, ()=>opt2Ran=true),
			};
			
			ops.Parse(new []{"/r"});
			
			Assert.IsTrue(opt1Ran, "Option1 did not run");
			Assert.IsFalse(opt2Ran, "Option2 ran");
		}
		
		[Test]
		public void LiteralFlag()
		{
			var opt1Ran =false;
			var opt2Ran =false;
			Options ops = new Options
			{
				new Option(new []{"r","run"}, ()=>opt1Ran=true),
				new Option(new []{"d","dont"}, ()=>opt2Ran=true),
			};
			
			ops.Parse(new []{"--r"});
			
			Assert.IsTrue(opt1Ran, "Option1 did not run");
			Assert.IsFalse(opt2Ran, "Option2 ran");
		}
		
		[Test]
		public void LiteralFlag_LongForm()
		{
			var opt1Ran =false;
			var opt2Ran =false;
			Options ops = new Options
			{
				new Option(new []{"r","run"}, ()=>opt1Ran=true),
				new Option(new []{"d","dont"}, ()=>opt2Ran=true),
			};
			
			ops.Parse(new []{"--run"});
			
			Assert.IsTrue(opt1Ran, "Option1 did not run");
			Assert.IsFalse(opt2Ran, "Option2 ran");
		}
	}
}


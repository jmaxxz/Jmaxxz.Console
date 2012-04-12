using System;
using ConsoleOptions;
using NUnit.Framework;
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
                new Option(new []{"r","run"}, ()=>opt1Ran=true, ""),
                new Option(new []{"d","dont"}, ()=>opt2Ran=true, ""),
            };
            
            ops.Parse(new []{"-r"});
            
            Assert.IsTrue(opt1Ran, "Option1 did not run");
            Assert.IsFalse(opt2Ran, "Option2 ran");
        }
        
        [Test]
        public void TwoDataParameters()
        {
            var opt =0;
            var opt2 = 0.0;
            Options ops = new Options
            {
                new Option(new string[]{}, (int x)=>opt = x,"value", ""),
                new Option(new string[]{}, (double x)=>opt2 = x, "value",""),
            };
            
            ops.Parse(new []{"1", "2.0"});
            
            Assert.AreEqual(1,opt, "Opt was not initialized correctly");
            Assert.AreEqual(2.0, opt2, "Opt2 was not handled correctly");
        }
        
        [Test]
        public void SimpleDataParameter()
        {
            var opt =0;
            Options ops = new Options
            {
                new Option(new string[]{}, (int x)=>opt = x, "value",""),
            };
            
            ops.Parse(new []{"1"});
            
            Assert.AreEqual(1,opt, "Data was not initialized correctly");
        }
        
        [Test]
        public void OptionDoesNotRunTwice()
        {
            int opt1Ran =0;
            var opt2Ran =false;
            Options ops = new Options
            {
                new Option(new []{"r","run"}, ()=>opt1Ran++, ""),
                new Option(new []{"d","dont"}, ()=>opt2Ran=true, ""),
            };
            
            ops.Parse(new []{"-r","-r","--run"});
            
            Assert.AreEqual(1,opt1Ran, "Option1 did not run once and only once");
            Assert.IsFalse(opt2Ran, "Option2 ran");
        }
        
        [Test]
        public void Simple2OptionRan()
        {
            var opt1Ran =false;
            var opt2Ran =false;
            Options ops = new Options
            {
                new Option(new []{"r","run"}, ()=>opt1Ran=true, ""),
                new Option(new []{"d","dont"}, ()=>opt2Ran=true, ""),
            };
            
            ops.Parse(new []{"-r","--dont"});
            
            Assert.IsTrue(opt1Ran, "Option1 did not run");
            Assert.IsTrue(opt2Ran, "Option2 did not run");
        }
        
        
        [Test]
        public void LiteralFlag()
        {
            var opt1Ran =false;
            var opt2Ran =false;
            Options ops = new Options
            {
                new Option(new []{"r","run"}, ()=>opt1Ran=true, ""),
                new Option(new []{"d","dont"}, ()=>opt2Ran=true, ""),
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
                new Option(new []{"r","run"}, ()=>opt1Ran=true, ""),
                new Option(new []{"d","dont"}, ()=>opt2Ran=true, ""),
            };
            
            ops.Parse(new []{"--run"});
            
            Assert.IsTrue(opt1Ran, "Option1 did not run");
            Assert.IsFalse(opt2Ran, "Option2 ran");
        }
    }
}


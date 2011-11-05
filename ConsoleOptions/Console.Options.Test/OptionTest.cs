using System;
using ConsoleOptions;
using NUnit.Framework;
namespace ConsoleOptions.Test
{
    [TestFixture]
    public class OptionTest
    {
        [Test]
        public void SimpleExecution()
        {
            bool hasRun = false;
            Option o = new Option(new []{"r","run","doit"}, ()=>hasRun =true, "");
            o.Invoke("r","");

            Assert.IsTrue(hasRun);
        }

        [Test]
        public void IsMatchTest()
        {
            Option o = new Option(new []{"r","run","doit"}, ()=>Console.Write("Fail"), "");

            Assert.IsTrue(o.IsMatch("r"));
            Assert.IsTrue(o.IsMatch("run"));
            Assert.IsTrue(o.IsMatch("doit"));
        }

        [Test]
        public void IsNotMatchTest()
        {
            Option o = new Option(new []{"r","run","doit"}, ()=>Console.Write("Fail"), "");

            Assert.IsFalse(o.IsMatch("d"));
        }

        [Test]
        public void SimpleExecutionWith_Int()
        {
            int result = 0;
            Option o = new Option(new []{"r","run","doit"}, (int x)=>result=x, "value","");
            o.Invoke("r","1");

            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void SimpleExecutionWith_Bool()
        {
            bool result = false;
            Option o = new Option(new []{"r","run","doit"}, x=>result=x, "value","");
            o.Invoke("r","True");

            Assert.AreEqual(true, result);
        }


        [Test]
        public void SimpleExecutionWith_Short()
        {
            short result = 0;
            Option o = new Option(new []{"r","run","doit"}, (short x)=>result=x, "value","");
            o.Invoke("r","1");

            Assert.AreEqual(1, result);
        }


        [Test]
        public void SimpleExecutionWith_Double()
        {
            double result = 0;
            Option o = new Option(new []{"r","run","doit"}, (double x)=>result=x, "value","");
            o.Invoke("r","1.1");
            
            Assert.AreEqual(1.1, result);
        }
                        
        [Test]
        public void SimpleExecutionWith_Byte()
        {
            byte result = 0;
            Option o = new Option(new []{"r","run","doit"}, (byte x)=>result=x, "value","");
            o.Invoke("r","240");
            
            Assert.AreEqual(240, result);
        }

        [Test]
        public void SimpleExecutionWith_DateTime()
        {
            DateTime result = DateTime.MinValue;
            Option o = new Option(new []{"r","run","doit"}, (DateTime x)=>result=x, "value","");
            o.Invoke("r",DateTime.MaxValue.ToString("o"));

            Assert.AreEqual(DateTime.MaxValue, result);
        }

        [Test]
        public void SimpleExecutionWith_TimeSpan()
        {
            TimeSpan result = TimeSpan.Zero;
            Option o = new Option(new []{"r","run","doit"}, (TimeSpan x)=>result=x, "value","");
            o.Invoke("r",TimeSpan.FromDays(1.5).ToString());

            Assert.AreEqual(TimeSpan.FromDays(1.5), result);
        }
    }
}


using System;
using ConsoleOptions;
using NUnit.Framework;

namespace ConsoleOptions.Test
{
    public class FlaglessOptionTest
    {
            [Test]
            public void SimpleExecutionWith_Int()
            {
                int result = 0;
                Option o = new Option((int x)=>result=x);
                o.Invoke("","1");
    
                Assert.AreEqual(1, result);
            }

            [Test]
            public void SimpleExecutionWith_Bool()
            {
                bool result = false;
                Option o = new Option(x=>result=x);
                o.Invoke("","True");

                Assert.AreEqual(true, result);
            }
    
    
            [Test]
            public void SimpleExecutionWith_Short()
            {
                short result = 0;
                Option o = new Option((short x)=>result=x);
                o.Invoke("","1");
    
                Assert.AreEqual(1, result);
            }

    
            [Test]
            public void SimpleExecutionWith_Double()
            {
                double result = 0;
                Option o = new Option((double x)=>result=x);
                o.Invoke("","1.1");
    
                Assert.AreEqual(1.1, result);
            }
    
            [Test]
            public void SimpleExecutionWith_Byte()
            {
                byte result = 0;
                Option o = new Option((byte x)=>result=x);
                o.Invoke("","240");
                
                Assert.AreEqual(240, result);
            }
    
            [Test]
            public void SimpleExecutionWith_DateTime()
            {
                DateTime result = DateTime.MinValue;
                Option o = new Option((DateTime x)=>result=x);
                o.Invoke("",DateTime.MaxValue.ToString("o"));
    
                Assert.AreEqual(DateTime.MaxValue, result);
            }

            [Test]
            public void SimpleExecutionWith_TimeSpan()
            {
                TimeSpan result = TimeSpan.Zero;
                Option o = new Option((TimeSpan x)=>result=x);
                o.Invoke("",TimeSpan.FromDays(1.5).ToString());

                Assert.AreEqual(TimeSpan.FromDays(1.5), result);
            }

            [Test]
            public void IsNotMatchTest()
            {
                Option o = new Option((int x)=>Console.Write(x));

                Assert.IsFalse(o.IsMatch(""));
            }
        }
    }


using System;
using ConsoleOptions;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace ConsoleOptions.Test
{
    [TestFixture]
    public class OptionRedirectedOutputTest
    {
        [Test]
        public void RedirectedError()
        {
            int x =0;
            var o = new Option((int y)=>x=y,"somevalue","description");
            StringBuilder errOut = new StringBuilder();
            using(var errStream = new StringWriter(errOut))
            {
                o.Invoke("","Nope this is wrong",errStream);
                Assert.IsTrue(errOut.ToString().Contains("Nope this is wrong"));
            }

        }
    }
}


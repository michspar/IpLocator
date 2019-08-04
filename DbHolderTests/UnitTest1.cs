using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace DbHolderTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLoadSpeed()
        {
            var holder = new DbHolder.DbHolder();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            holder.LoadDbFromFile(Path.Combine(Environment.CurrentDirectory, "geobase.dat"));
            stopwatch.Stop();

            Logger.LogMessage("Db loaded in {0}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}

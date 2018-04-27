using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exmo;
using API.Exmo;

namespace TestCase
{
    [TestClass]
    public class ExmoTest
    {      
        [TestMethod]
        public void ReloadExmoTicker()
        {
            IExmoRepertory client = new ExmoRepertory();

            var isTrue = client.ReloadTicker();
            Assert.IsTrue(isTrue, "ExmoTicker reload failed");
        }
    }
}

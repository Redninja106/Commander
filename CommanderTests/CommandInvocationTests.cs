using Commander;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderTests
{
    [TestClass]
    public class CommandInvocationTests
    {
        [TestMethod]
        public void TestCommandInvocation_Equals()
        {
            // for service and name, case should not matter.
            var invocation1 = new CommandInvocation("commander:", "print", new[] { "hi" });
            var invocation2 = new CommandInvocation("Commander:", "PRINT", new[] { "hi" });

            Assert.IsTrue(invocation1.Equals(invocation2));
            Assert.IsTrue(invocation2.Equals(invocation1));

            // for parameters, case should matter.
            invocation2.Parameters[0] = "Hi";

            Assert.IsFalse(invocation1.Equals(invocation2));
            Assert.IsFalse(invocation2.Equals(invocation1));
        }
    }
}

using Commander;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommanderTests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void TestBasicPrint()
        {
            var parser = new CommandParser();

            var invocation = parser.GetInvocation("print hello");

            Assert.AreEqual(invocation.Service, null);
            Assert.AreEqual(invocation.Name, "print", $"invocation name is '{invocation.Name}' not 'print'");
            Assert.AreEqual(invocation.ParameterCount, 1, $"Parameter count {invocation.ParameterCount}, not 1");
            Assert.AreEqual(invocation.Parameters, new[] { "hello" }, "Parameters not equal");
        }
    }
}

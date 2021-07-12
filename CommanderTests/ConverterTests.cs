using Commander;
using Commander.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderTests
{
    [TestClass]
    public class ConverterTests
    {
        private static void TestBaseConverterType<TConverter, TExpectedType>(string[] sampleFailures, (string, TExpectedType)[] samples) where TConverter : CommandArgumentConverter, new()
        {
            var converter = new TConverter();

            Assert.AreEqual(typeof(TExpectedType), converter.GetTargetType(), "Converter does not target the expected target type.");

            foreach (var (sample, expected) in samples)
            {
                Assert.IsTrue(converter.TryConvertToObject(sample, out object result), $"Conversion of value {sample} failed, success expected.");

                Assert.AreEqual(typeof(TExpectedType), result.GetType(), $"conversion result of value {sample} is not the expected type '{typeof(TExpectedType)}'. It is instead '{result.GetType()}'");

                Assert.AreEqual(expected, result, $"conversion of {sample} does not provide the expected result of '{result}'.");
            }

            foreach (var failSample in sampleFailures)
            {
                Assert.IsFalse(converter.TryConvertToObject(failSample, out object failResult), $"Conversion of value '{failSample}' expected failure, got success");

                Assert.IsNull(failResult, $"parameter result is not null after failed conversion");
            }
        }

        [TestMethod]
        public void TestBoolConverter() => TestBaseConverterType<BoolConverter, bool>(new[] 
        { 
            "hello", "123455" 
        }, new[] 
        { 
            ("true", true),
            ("false", false)
        });

        [TestMethod]
        public void TestByteConverter() => TestBaseConverterType<ByteConverter, byte>(new[]
        {
            "asdasd", "256", "-1", "12.12"
        }, new[]
        {
            ("255", (byte)255),
            ("100", (byte)100),
            ("0", (byte)0)
        });

        [TestMethod]
        public void TestDecimalConverter() => TestBaseConverterType<DecimalConverter, decimal>(new[]
        {
            "asdasd", "==|}{256", "-124d", "12.asd2"
        }, new[]
        {
            ("255.2", 255.2m),
            ("100", 100),
            ("0", 0)
        });
    }
}

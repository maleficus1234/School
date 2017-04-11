using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void ExceptionHandler1()
        {
            string code = @"
import System

module foo:
	int bar():
		int i = 1
		int zero = 0
		try:
			i /= zero
		catch Exception e:
			Console.WriteLine(ex)
		finally:
			i = 1234
		return i
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);

            Type foo = results.CompiledAssembly.GetType("foo");
            var method = foo.GetMethod("bar");
            int o = (int)method.Invoke(null, null);
            Assert.AreEqual(1234, o);
        }

        [TestMethod]
        public void GenerateThrow()
        {
            string code = @"
import System

module foo:
	int bar():
		throw new Exception()
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }
    }
}

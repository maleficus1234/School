using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class InstantiationTests
    {
        [TestMethod]
        public void SimpleInstantiation()
        {
            string code = @"
import System

class foo:
	void bar():
		var f = new foo()
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void ArrayInstantiation()
        {
            string code = @"
import System

class foo:
	void bar():
		int [] f = new int[3]
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenericInstantiation()
        {
            string code = @"
import System.Collections.Generic

class foo:
	void bar():
		List{int} v = new List{int}()
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }
    }
}

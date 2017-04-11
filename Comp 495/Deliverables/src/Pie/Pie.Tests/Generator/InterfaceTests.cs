
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class InterfaceTests
    {
        [TestMethod]
        public void GenerateInterface()
        {
            string code = @"
import System

interface IFoo:
    int i
    int bar()
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

using Pie;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class ArrayTests
    {
        [TestMethod]
        public void GenerateArrayMethod()
        {
            string code = @"
module bar:
	int [] test(int x):
		return null
                            ";

            // Compile the code.
            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);
            foreach (CompilerError e in results.Errors)
            {
                System.Diagnostics.Debug.WriteLine(e.ErrorText);
            }
            // Verify that there are not compiler errors.
            Assert.AreEqual(0, results.Errors.Count);

        }

        [TestMethod]
        public void GenerateArrayMethodReturn()
        {
            string code = @"
module bar:
	int [] test(int x):
		return new int[1]
                            ";

            // Compile the code.
            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);
            foreach (CompilerError e in results.Errors)
            {
                System.Diagnostics.Debug.WriteLine(e.ErrorText);
            }
            // Verify that there are not compiler errors.
            Assert.AreEqual(0, results.Errors.Count);

        }

        [TestMethod]
        public void GenerateArrayIndexer()
        {
            string code = @"
module bar:
	int test(int [] x):
		return x[0]
                            ";

            // Compile the code.
            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);
            foreach (CompilerError e in results.Errors)
            {
                System.Diagnostics.Debug.WriteLine(e.ErrorText);
            }
            // Verify that there are not compiler errors.
            Assert.AreEqual(0, results.Errors.Count);

        }
    }
}

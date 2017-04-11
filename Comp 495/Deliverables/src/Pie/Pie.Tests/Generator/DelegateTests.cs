using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class DelegateTests
    {
        [TestMethod]
        public void GenerateDelegate()
        {
            string code = @"
                                delegate void d()
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateDelegate2()
        {
            string code = @"
                                delegate int d()
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateDelegate3()
        {
            string code = @"
                                delegate int d(ref int d)
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateDelegate4()
        {
            string code = @"
                                delegate int d(int [] d)
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateDelegate5()
        {
            string code = @"
                                delegate int [] d(int [] d)
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateDelegate6()
        {
            string code = @"
import System.Collections.Generic
delegate List{string} d(List{string} d)
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError error in results.Errors)
                Console.WriteLine(error.ErrorText);
            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateEvent()
        {
            string code = @"
import System.Collections.Generic
delegate int del(int d)

class foo:
    public event del delegateEvent
    void Main():
        delegateEvent += fn
    int fn(int d):
		return 1
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError error in results.Errors)
                Console.WriteLine(error.ErrorText);
            Assert.AreEqual(0, results.Errors.Count);
        }
    }
}

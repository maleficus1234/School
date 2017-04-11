using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class LoopTests
    {
        [TestMethod]
        public void GenerateWhileLoop()
        {
            string code = @"
module bar:
    int Counter():
        int i = 0
        while i < 10:
            i = i + 1
        return i
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }
            Assert.AreEqual(0, results.Errors.Count);

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Counter");
            Assert.IsNotNull(counterMethod);

            int i = (int)counterMethod.Invoke(null, null);
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void GenerateWhileLoop2()
        {
            string code = @"
module bar:
    int Counter():
        int i = 0
        while i < 10:
            i = i + 1
        return i
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }
            Assert.AreEqual(0, results.Errors.Count);

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Counter");
            Assert.IsNotNull(counterMethod);

            int i = (int)counterMethod.Invoke(null, null);
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void GenerateForLoop1()
        {
            string code = @"
module bar:
    int Counter():
        int c = 0
        for int i in 1 to 10:
            c = i
        return c
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }
            Assert.AreEqual(0, results.Errors.Count);

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Counter");
            Assert.IsNotNull(counterMethod);

            int i = (int)counterMethod.Invoke(null, null);
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void GenerateForLoop2()
        {
            string code = @"
module bar:
    int Counter():
        int c = 0
        int i = 0
        for i in 1 to 10 step 2:
            c = c + 1
        return c
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }
            Assert.AreEqual(0, results.Errors.Count);

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Counter");
            Assert.IsNotNull(counterMethod);

            int i = (int)counterMethod.Invoke(null, null);
            Assert.AreEqual(5, i);
        }

        [TestMethod]
        public void GenerateForLoop3()
        {
            string code = @"
module bar:
    int Counter():
        int c = 0
        int i = 0
        for i in 10 step 2:
            c = c + 1
        return c
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }
            Assert.AreEqual(0, results.Errors.Count);

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Counter");
            Assert.IsNotNull(counterMethod);

            int i = (int)counterMethod.Invoke(null, null);
            Assert.AreEqual(6, i);
        }
    }
}

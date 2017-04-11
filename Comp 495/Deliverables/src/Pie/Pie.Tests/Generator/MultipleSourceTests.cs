using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class MultipleSourceTests
    {
        [TestMethod]
        public void GenerateMultipleSource1()
        {
            string code1 = @"
                                class bar:
                                    void foo():
                            ";

            string code2 = @"
                                class wow:
                                    void rawr():
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code1, code2);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(0, results.Errors.Count);
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);
            Assert.IsTrue(results.CompiledAssembly.GetType("wow").GetMethod("rawr").IsPublic);
        }

        [TestMethod]
        public void GenerateMultipleSource2()
        {
            string code1 = @"
                                module bar:
                                    void foo():
                            ";

            string code2 = @"
                                module bar:
                                    void rawr():
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code1, code2);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(0, results.Errors.Count);
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("rawr").IsPublic);
        }
    }
}

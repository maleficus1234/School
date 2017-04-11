using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class NamespaceTests
    {
        [TestMethod]
        public void GenerateGlobalNamespace()
        {
            string code = @"
                                class bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            Assert.IsNotNull(assembly.GetType("bar"));
        }

        [TestMethod]
        public void GenerateNamespace()
        {
            string code = @"
                            namespace foo:
                                class bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            Assert.IsNotNull(assembly.GetType("foo.bar"));
        }

        [TestMethod]
        public void GenerateGlobalNamespaceImports()
        {
            string code = @"
                                import System
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateNamespaceImports()
        {
            string code = @"
                            namespace foo:
                                import System
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);
        }
    }
}

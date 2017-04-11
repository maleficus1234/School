
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void GenerateEnumNoName()
        {
            string code = @"
                                enum:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateEnumDeclaration()
        {
            string code = @"
                                enum bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsEnum);
            Assert.IsTrue(t.IsPublic);
        }

        [TestMethod]
        public void GenerateEnumDeclaration2()
        {
            string code = @"
internal enum bar:
    boo
    foo
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsEnum);
            Assert.IsTrue(t.IsNotPublic);

            var a = t.GetEnumValues();
            Assert.AreEqual(0, (int)a.GetValue(0));

            Assert.AreEqual(1, (int)a.GetValue(1));
        }

        [TestMethod]
        public void GenerateEnumDeclaration3()
        {
            string code = @"
internal enum bar:
    boo
    foo = 23
    woo
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsEnum);

            var a = t.GetEnumValues();
            Assert.AreEqual(0, (int)a.GetValue(0));

            Assert.AreEqual(23, (int)a.GetValue(1));

            Assert.AreEqual(24, (int)a.GetValue(2));
        }
    }
}

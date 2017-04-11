using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class ClassVariableTests
    {
        [TestMethod]
        public void ClassVariableTest1()
        {
            string code = @"
class foo:
    int bar
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);

            Type t = results.CompiledAssembly.GetType("foo");
            Assert.IsNotNull(t);

            var variable = t.GetField("bar", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(variable);
            Assert.IsTrue(variable.IsPrivate);

            var foo = Activator.CreateInstance(t);

            Assert.IsTrue(variable.GetValue(foo) is int);
        }

        [TestMethod]
        public void ClassVariableTest2()
        {
            string code = @"
class foo:
    int [] bar
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            Assert.AreEqual(0, results.Errors.Count);

            Type t = results.CompiledAssembly.GetType("foo");
            Assert.IsNotNull(t);

            var variable = t.GetField("bar", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(variable);

            Assert.IsTrue(variable.IsPrivate);
        }
    }
}

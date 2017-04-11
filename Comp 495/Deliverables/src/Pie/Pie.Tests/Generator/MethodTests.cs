using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class MethodTests
    {
        [TestMethod]
        public void GenerateClassMethodDefault()
        {
            string code = @"
class bar:
    void foo():
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);
        }

        [TestMethod]
        public void GenerateClassMethodDefault2()
        {
            string code = @"
class bar:
    void foo(ref int i):
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);
        }

        [TestMethod]
        public void GenerateClassMethodDefault3()
        {
            string code = @"
class bar:
    int [] foo(int [] i):
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);
        }

        [TestMethod]
        public void GenerateClassMethodDefault4()
        {
            string code = @"
import System.Collections.Generic
class bar:
    List{int} foo(List{int} i):
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);
        }

        [TestMethod]
        public void GenerateStructMethodDefault()
        {
            string code = @"
struct bar:
    void foo():
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsPublic);

        }

        [TestMethod]
        public void GenerateClassMethodPrivate()
        {
            string code = @"
class bar:
    private void foo():
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo", System.Reflection.BindingFlags.NonPublic | BindingFlags.Instance).IsPrivate);
        }

        [TestMethod]
        public void GenerateClassMethodProtected()
        {
            string code = @"
class bar:
    protected void foo():
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
        }

        [TestMethod]
        public void GenerateClassMethodShared()
        {
            string code = @"
class bar:
    shared void foo():
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
        }


        [TestMethod]
        public void GenerateClassMethodFinal()
        {
            string code = @"
class bar:
    final void foo():
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
        }

        [TestMethod]
        public void GenerateClassMethodFinalShared()
        {
            string code = @"
class bar:
    shared final void foo():
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(1, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateClassMethodOverride()
        {
            string code = @"
class bar:
    virtual void foo():
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
            Assert.IsTrue(results.CompiledAssembly.GetType("bar").GetMethod("foo").IsVirtual);
        }

        [TestMethod]
        public void GenerateClassMethodOut()
        {
            string code = @"
class bar:
    void foo(out int i):
        i = 123
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
        }

        [TestMethod]
        public void GenerateClassConstructor()
        {
            string code = @"
class bar:
    new(int i):
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
        }

        [TestMethod]
        public void GenerateModuleConstructor()
        {
            string code = @"
module bar:
    new(int i):
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ModuleConstructorCompilerError);
        }

        [TestMethod]
        public void GeneratePrivateConstructor()
        {
            string code = @"
class bar:
    private new(int i):

module foo:
    void test():
        bar b = new bar(99)
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(1, results.Errors.Count);
        }

        [TestMethod]
        public void GeneratePublicConstructor()
        {
            string code = @"
class bar:
    public new(int i):

module foo:
    void test():
        bar b = new bar(99)
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
        }

        [TestMethod]
        public void GenerateInstantiation()
        {
            string code = @"
class bar:
	int number
    
	new(int number):
		this.number = number
	
	int GetNumber():
		return number
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

            var assembly = results.CompiledAssembly;

            Type t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            object o = Activator.CreateInstance(t, 1234);
            Assert.IsNotNull(o);

            var method = t.GetMethod("GetNumber");
            Assert.IsNotNull(method);

            int n = (int)method.Invoke(o, null);
            Assert.AreEqual(1234, n);
        }
    }
}

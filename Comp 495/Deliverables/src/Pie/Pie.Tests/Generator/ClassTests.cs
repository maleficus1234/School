using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void GenerateIncompleteClass()
        {
            string code = @"
class:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateModule()
        {
            string code = @"
module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsSealed);
            Assert.IsFalse(t.IsAbstract);
            Assert.IsTrue(t.IsClass);
            Assert.IsTrue(t.IsPublic);
        }

        [TestMethod]
        public void GenerateStruct()
        {
            string code = @"
struct bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsValueType);
            Assert.IsTrue(t.IsPublic);
        }

        [TestMethod]
        public void GeneratePublicModule()
        {
            string code = @"
public module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsSealed);
            Assert.IsFalse(t.IsAbstract);
            Assert.IsTrue(t.IsClass);
            Assert.IsTrue(t.IsPublic);
        }

        [TestMethod]
        public void GenerateInternalModule()
        {
            string code = @"
internal module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            var t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            Assert.IsTrue(t.IsSealed);
            Assert.IsFalse(t.IsAbstract);
            Assert.IsTrue(t.IsClass);
            Assert.IsFalse(t.IsVisible);
        }

        [TestMethod]
        public void GeneratePrivateModule()
        {
            string code = @"
private module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            
            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText + results.Errors[0].GetType());
            }
            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateProtectedModule()
        {
            string code = @"
protected module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;


            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }
            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateAbstractModule()
        {
            string code = @"
abstract module bar:
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
            Assert.IsTrue(results.Errors[0] is ModuleModifierCompilerError);
        }

        [TestMethod]
        public void GenerateSharedModule()
        {
            string code = @"
shared module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;


            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateSharedClass()
        {
            string code = @"
shared module bar:
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;


            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorText);
            }

            Assert.AreEqual(1, results.Errors.Count);
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateEmptyBaseType()
        {
            string code = @"
class bar ():
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
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateMissingBaseType()
        {
            string code = @"
class bar (foo):
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
        public void GenerateValidBaseType()
        {
            string code = @"
class bar (System.Object):
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
        public void GenerateMissingBaseType2()
        {
            string code = @"
class bar (Object):
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
        public void GenerateValidBaseType2()
        {
            string code = @"
import System
class bar (Object):
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
        public void GenerateModuleInheritance()
        {
            string code = @"
module bar (System.Object):
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
            Assert.IsTrue(results.Errors[0] is ModuleInheritanceCompilerError);
        }

        [TestMethod]
        public void GenerateStructInheritance1()
        {
            string code = @"
struct bar (System.Object):
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
        public void GenerateStructInheritance2()
        {
            string code = @"
struct bar (System.IDisposable):
    void Dispose():
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
        public void GenerateEmptyGenericType()
        {
            string code = @"
class bar {}:
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
            Assert.IsTrue(results.Errors[0] is ParserCompilerError);
        }

        [TestMethod]
        public void GenerateClassGenerics()
        {
            string code = @"
class bar {T, R}:
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
        public void GenerateClassGenerics2()
        {
            string code = @"
class bar {T, R}:
    T foo(T a):
        return a
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
        public void GenerateClassInheritance2()
        {
            string code = @"
import System

abstract class Animal:

	protected String name

	new(String name):
		this.name = name

	abstract String Speak():


class Monkey(Animal):

	new(String name): base(name):

	override String Speak():
		return ""Ook ook!""


class Human(Monkey):

	new(String name): base(name):

	override String Speak():
		return ""Hello world! My name is "" + this.name + "".""
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

            Type monkeyType = results.CompiledAssembly.GetType("Monkey");
            Assert.IsNotNull(monkeyType);

            var monkeyMethod = monkeyType.GetMethod("Speak");

            object monkey = Activator.CreateInstance(monkeyType, "Jibbers");
            string monkeySpeak = (string)monkeyMethod.Invoke(monkey, null);
            Assert.AreEqual("Ook ook!", monkeySpeak);

            Type humanType = results.CompiledAssembly.GetType("Human");
            Assert.IsNotNull(humanType);
            var humanMethod = humanType.GetMethod("Speak");
            object human = Activator.CreateInstance(humanType, "Sir Harold Von Rabberdasher");
            string humanSpeak = (string)humanMethod.Invoke(human, null);
            Assert.AreEqual("Hello world! My name is Sir Harold Von Rabberdasher.", humanSpeak);
        }

        [TestMethod]
        public void GenerateClassChainedCtor()
        {
            string code = @"
import System

class Test:
	
    private int testNumber

    int GetTestNumber():
        return testNumber

    new(int i):
        testNumber = i

    new() : this(1234):
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

            Type t = results.CompiledAssembly.GetType("Test");
            var testMethod = t.GetMethod("GetTestNumber");

            object instance1 = Activator.CreateInstance(t);
            int number = (int)testMethod.Invoke(instance1, null);
            Assert.AreEqual(1234, number);

            object instance2 = Activator.CreateInstance(t, 1357);
            number = (int)testMethod.Invoke(instance2, null);
            Assert.AreEqual(1357, number);
        }
    }
}

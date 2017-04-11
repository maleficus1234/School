using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{

    [TestClass]
    public class LiteralTests
    {

        [TestMethod]
        public void GenerateNullLiteral()
        {
            string code = @"
module bar:
    object Get():
        return null
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o =counterMethod.Invoke(null, null);
            Assert.IsNull(o);
        }

        [TestMethod]
        public void GenerateCharLiteral()
        {
            string code = @"
module bar:
    char Get():
        char c = 'c'
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Assert.IsTrue(o is Char);
            Assert.AreEqual('c', (Char)o);
        }

        [TestMethod]
        public void GenerateStringLiteral()
        {
            string code = @"
import System

module bar:
    String Get():
        String c = ""whee""
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Assert.IsTrue(o is String);
            Assert.AreEqual("whee", (String)o);
        }

        [TestMethod]
        public void GenerateByteLiteral()
        {
            string code = @"
module bar:
    byte Get():
        byte b = 255
        return b
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Assert.IsTrue(o is Byte);
            Assert.AreEqual(255, (Byte)o);
        }

        [TestMethod]
        public void GenerateShortLiteral()
        {
            string code = @"
module bar:
    short Get():
        short i = 1234
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is Int16);
            Assert.AreEqual(1234, (Int16)o);
        }

        [TestMethod]
        public void GenerateUShortLiteral()
        {
            string code = @"
module bar:
    ushort Get():
        ushort i = 1234
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is ushort);
            Assert.AreEqual(1234, (ushort)o);
        }

        [TestMethod]
        public void GenerateIntLiteral()
        {
            string code = @"
module bar:
    int Get():
        int i = 1234
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is int);
            Assert.AreEqual(1234, (int)o);
        }

        [TestMethod]
        public void GenerateUIntLiteral()
        {
            string code = @"
module bar:
    uint Get():
        uint i = 1234
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is uint);
            Assert.AreEqual((uint)1234, (uint)o);
        }

        [TestMethod]
        public void GenerateLongLiteral()
        {
            string code = @"
module bar:
    long Get():
        long i = 1234
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is long);
            Assert.AreEqual(1234, (long)o);
        }

        [TestMethod]
        public void GenerateULongLiteral()
        {
            string code = @"
module bar:
    ulong Get():
        ulong i = 1234
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is ulong);
            Assert.AreEqual((ulong)1234, (ulong)o);
        }

        [TestMethod]
        public void GenerateFloatLiteral()
        {
            string code = @"
module bar:
    float Get():
        float i = 1234.0f
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is float);
            Assert.AreEqual(1234.0f, (float)o);
        }

        [TestMethod]
        public void GenerateDoubleLiteral()
        {
            string code = @"
module bar:
    double Get():
        double i = 1234.0
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is double);
            Assert.AreEqual(1234.0, (double)o);
        }

        [TestMethod]
        public void GenerateDecimalLiteral()
        {
            string code = @"
module bar:
    decimal Get():
        decimal i = 1234.0M
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is decimal);
            Assert.AreEqual(1234.0M, (decimal)o);
        }

        [TestMethod]
        public void GenerateBoolLiteral()
        {
            string code = @"
module bar:
    bool Get():
        bool i = true
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

            var counterMethod = results.CompiledAssembly.GetType("bar").GetMethod("Get");

            object o = counterMethod.Invoke(null, null);
            Console.WriteLine(o.GetType());
            Assert.IsTrue(o is bool);
            Assert.AreEqual(true, (bool)o);
        }
    }
}

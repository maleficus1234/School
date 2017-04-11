using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

using Pie;

namespace Pie.Tests.Generator
{

    [TestClass]
    public class AlgorithmTests
    {
        // Test that the Fibonacci algorithm produces correct results.
        [TestMethod]
        public void GenerateFibonacci()
        {

            // The standard recursive fibonacci algorithm.
            string code = @"
module bar:
	int Fibonacci(int x):
		if x <= 1 return x
		return Fibonacci(x - 1) +
            Fibonacci(x - 2)
                            ";

            // Compile the code.
            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            // Verify that there are not compiler errors.
            Assert.AreEqual(0, results.Errors.Count);
            Console.WriteLine(results.Errors.Count);
            foreach (CompilerError e in results.Errors)
            {
                System.Diagnostics.Debug.WriteLine(e.ErrorText);
            }

            // Verify that the algorithm produces proper values for a few positions in the fibonacci sequence.
            Assert.AreEqual(Invocation.InvokeShared(results.CompiledAssembly, "bar", "Fibonacci", 8), 21);
            Assert.AreEqual(Invocation.InvokeShared(results.CompiledAssembly, "bar", "Fibonacci", 9), 34);
            Assert.AreEqual(Invocation.InvokeShared(results.CompiledAssembly, "bar", "Fibonacci", 10), 55);
        }

        // Test the FizzBuzz algorithm: a "super hard" algorithm often used to weed out people
        // in job interviews: if a number is divisible by 3, output fizz. If a number is divisible by
        // 5, output buzz. If a number is divisible by 15, output fizzbuzz. In all other cases output
        // the number.
        [TestMethod]
        public void GenerateFizzBuzz()
        {
            string code = @"
import System
                            
module bar:
	void FizzBuzz(int x):
		if x % 3 == 0:
			Console.Write(""Fizz"")
		if x % 5 == 0: 
			Console.Write(""Buzz"")
		if x % 5 != 0 && x % 3 != 0:
			Console.Write(x.ToString())
                            ";

            // Compile the code.
            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);



            foreach (CompilerError e in results.Errors)
            {
                System.Diagnostics.Debug.WriteLine(e.ErrorText);
            }

            // Verify that there are not compiler errors.
            Assert.AreEqual(0, results.Errors.Count);

            Type t = results.CompiledAssembly.GetType("bar");
            Assert.IsNotNull(t);

            var method = t.GetMethod("FizzBuzz");
            Assert.IsNotNull(method);

            for (int i = 1; i <= 100; i++)
            {
                Console.Write(i + ": ");
                method.Invoke(null, new object[] { i });
                Console.WriteLine();
            }
        }
    }
}

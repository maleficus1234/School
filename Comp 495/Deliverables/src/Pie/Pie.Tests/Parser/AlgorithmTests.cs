using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class AlgorithmTests
    {
        [TestMethod]
        public void ParseFibonacci()
        {
            // The standard recursive fibonacci algorithm.
            string code = @"
                            module bar:
	                            int Fibonacci(int x):
		                            if x <= 1 return x
		                            return Fibonacci(x - 1) +
                                        Fibonacci(x - 2)
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");
        }

        [TestMethod]
        public void ParseFizzBuzz()
        {
            string code = @"
import System.Console

module test:

    void FizzBuzz(int x):
        if x % 3 == 0 WriteLine(""Fizz"")
        else if x % 5 WriteLine(""Buzz"")
        else WriteLine(x)
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);


            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");
        }
    }
}

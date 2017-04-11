using System;
using System.CodeDom.Compiler;
using Pie;

namespace PieCBuilder
{
    class Program
    {
        // Build and test a command line compiler.
        static void Main(string[] args)
        {
            var file = new System.IO.StreamReader("PieCSource/Program.pie");

            string code = file.ReadToEnd();
            file.Close();

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("Pie.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = "piec.exe";
            parameters.GenerateExecutable = true;
            
            var provider = new PieCodeProvider();
            var results = provider.CompileAssemblyFromSource(parameters, code);

            foreach (CompilerError e in results.Errors)
            {
                Console.WriteLine(e.ErrorNumber + ", " + e.Line + ", " + e.Column + ": " + e.ErrorText);
            }

            if (results.Errors.Count == 0)
            {
                Type program = results.CompiledAssembly.GetType("Program");
                var main = program.GetMethod("Main");

                string[] strings = new string[] { "/out:test.exe", "TestCode/Program.pie", "TestCode/Hello.pie" };
                main.Invoke(null, new object[] { strings });
            }

            Console.ReadKey();
        }
    }
}

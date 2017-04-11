using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;
using System.Reflection;

using Pie;
using Pie.Expressions;

namespace Pie.Tests
{


    [TestClass]
    public class ClassCodeDomTests
    {
        void ValidateClassType(Assembly assembly, string fullName, Accessibility access, bool final)
        {
            // Check that the type exists in the assembly.
            Type t = assembly.GetType(fullName);
            Assert.IsNotNull(t);

            // Check that it is a class
            Assert.IsTrue(t.IsClass);

            // Check that the full name is correct
            Assert.AreEqual(t.FullName, fullName);

            // Check that the type is final (sealed)
            Assert.AreEqual(t.IsSealed, final);

            // Check that it has the correct accessibility
            switch(access)
            {
                case Accessibility.Public:
                    Assert.IsTrue(t.IsPublic);
                    break;
                case Accessibility.Internal:
                    Assert.IsTrue(t.IsNotPublic);
                    break;
                case Accessibility.Private:
                case Accessibility.Protected:
                    Assert.Fail("Invalid accessibility for class types");
                    break;
            }
        }

        CompilerResults Compile(string code)
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = (new PieCodeProvider()).CompileAssemblyFromSource(parameters, code);
            foreach (var e in results.Errors)
                Console.WriteLine(e);
            return results;
        }

        [TestMethod]
        public void IntegrateNamespaceClass()
        {
            string code = @"
                            namespace foo:
                                class bar:";

            var result = Compile(code);

            // Check that no errors were produced.
            Assert.AreEqual(result.Errors.Count, 0);

            // Check that the Type matches the code.
            ValidateClassType(result.CompiledAssembly,
                "foo.bar",
                Accessibility.Public,
                false);
        }
    }
}

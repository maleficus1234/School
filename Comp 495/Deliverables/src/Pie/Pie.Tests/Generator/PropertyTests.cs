using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie.Tests.Generator
{
    [TestClass]
    public class PropertyTests
    {
        [TestMethod]
        public void GenerateProperty1()
        {
            string code = @"
module bar:

    int testproperty:
        get:
            return 1234    
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            Type t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            var prop = t.GetProperty("testproperty");
            Assert.IsNotNull(prop);

            var method = prop.GetGetMethod();
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsPublic);
            Assert.IsTrue(method.IsStatic);

            int i = (int)method.Invoke(null, null);
            Assert.AreEqual(1234, i);
        }

        [TestMethod]
        public void GenerateProperty2()
        {
            string code = @"
class bar:

    int i

    int testproperty:
        get:
            return i
        set:
            i = value    
                            ";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);

            Type t = assembly.GetType("bar");
            Assert.IsNotNull(t);

            object inst = Activator.CreateInstance(t);

            var prop = t.GetProperty("testproperty");
            Assert.IsNotNull(prop);

            var setmethod = prop.GetSetMethod();
            Assert.IsNotNull(setmethod);
            Assert.IsTrue(setmethod.IsPublic);
            Assert.IsFalse(setmethod.IsStatic);
            setmethod.Invoke(inst, new object[]{1234});

            var getmethod = prop.GetGetMethod();
            Assert.IsNotNull(getmethod);

            int i = (int)getmethod.Invoke(inst, null);
            Assert.AreEqual(1234, i);
        }


        [TestMethod]
        public void GenerateProperty3()
        {
            string code = @"
module bar:

    int [] testproperty:
        get:
            return new int[2]   
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);
        }

        [TestMethod]
        public void GenerateProperty4()
        {
            string code = @"
import System.Collections.Generic
module bar:

    List{int} testproperty:
        get:
            return null  
";

            var provider = new PieCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(parameters, code);

            var assembly = results.CompiledAssembly;

            Assert.AreEqual(0, results.Errors.Count);
        }
    }
}

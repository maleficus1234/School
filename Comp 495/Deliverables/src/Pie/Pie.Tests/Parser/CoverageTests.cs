using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class CoverageTests
    {
        [TestMethod]
        public void TestParserCreation()
        {
            Assert.IsNotNull(Pie.Parser.Create());

            try
            {
                PieCodeProvider provider = new PieCodeProvider();
                provider.CreateGenerator();
                Assert.Fail();
            }
            catch(NotImplementedException e)
            {
                // Just to get rid of the "variable is never used" warning
                Console.WriteLine(e.StackTrace);
            }

            try
            {
                PieCodeProvider provider = new PieCodeProvider();
                provider.CreateCompiler();
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // Just to get rid of the "variable is never used" warning
                Console.WriteLine(e.StackTrace);
            }


        }

        [TestMethod]
        public void TestClassFullName()
        {
            Namespace n = new Namespace(null, null);
            n.Name = "foo";

            Class c = new Class(null, null);
            c.UnqualifiedName = "bar";
            Console.WriteLine(c.GetQualifiedName());
            Assert.AreEqual("foo", n.Name, "namespace name1");
            Assert.AreEqual("foo", n.GetFullName(), "namespace full name1");
            Assert.AreEqual("bar", c.UnqualifiedName, "class name1");
            Assert.AreEqual("bar", c.GetQualifiedName(), "class full name1");

            c.ParentExpression = n;
            Assert.AreEqual("foo", n.Name, "namespace name2");
            Assert.AreEqual("foo", n.GetFullName(), "namespace full name2");
            Assert.AreEqual("bar", c.UnqualifiedName, "class name2");
            Assert.AreEqual("foo.bar", c.GetQualifiedName(), "class full name2");

            c.ParentExpression = new Expression(null, null);
            Assert.AreEqual("foo", n.Name, "namespace name3");
            Assert.AreEqual("foo", n.GetFullName(), "namespace full name3");
            Assert.AreEqual("bar", c.UnqualifiedName, "class name3");
            Assert.AreEqual("bar", c.GetQualifiedName(), "class full name3");
        }
    }
}

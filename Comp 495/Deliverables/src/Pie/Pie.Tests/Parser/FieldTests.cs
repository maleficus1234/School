using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class FieldTests
    {
        // Check that a class's explicit variable field parses correctly.
        [TestMethod]
        public void ParseClassVariable()
        {
            string code = @"
namespace foo:
    class bar:
        boo i
        boo b
                        ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a namespace
            Assert.AreEqual(root.ChildExpressions.Count, 1, "root 1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Namespace, "root child namespace");

            // Check that the only child of the namespace is a class.
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions.Count, 1, "namespace 1 child");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is Class, "namespace child class");

            // Check that the only child of the class is an explicit variable declaration
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions.Count, 2, "class 1 child");

            ClassTests.ValidateClass((Class)root.ChildExpressions[0].ChildExpressions[0],
                root.ChildExpressions[0],
                "bar",
                "foo.bar",
                Accessibility.Public,
                false,
                false,
                false,
                false,
                false);
        }

        // Check that a module's explicit variable field parses correctly.
        [TestMethod]
        public void ParseModuleExplicitVariable()
        {
            string code = @"
                            namespace foo:
                                module bar:
                                    int foo():
                                        int i
                                        int b
                ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a namespace
       /*     Assert.AreEqual(root.ChildExpressions.Count, 1, "root 1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Namespace, "root child namespace");

            // Check that the only child of the namespace is a class.
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions.Count, 1, "namespace 1 child");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is Class, "namespace child class");

            // Check that the only child of the class is an explicit variable declaration
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions.Count, 2, "class 1 child");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is ExplicitVariableDeclaration, "class child 1 explicit variable");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[1] is ExplicitVariableDeclaration, "class child 2 explicit variable");
            */
            ClassTests.ValidateClass((Class)root.ChildExpressions[0].ChildExpressions[0],
                root.ChildExpressions[0],
                "bar",
                "foo.bar",
                Accessibility.Public,
                true,
                true,
                true,
                false,
                false);
        }
    }
}

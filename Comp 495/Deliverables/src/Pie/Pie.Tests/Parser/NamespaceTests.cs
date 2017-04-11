using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class NamespaceTests
    {
        // Test that an empty source file compiles correctly.
        [TestMethod]
        public void ParseEmptySource()
        {
            string code = @"";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        // Test that a namespace with no name correctly generates a compiler error.
        [TestMethod]
        public void ParseNamespaceNoName()
        {
            string code = @"namespace";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that a parser error was produced.
            Assert.AreEqual(root.CompilerErrors.Count, 1, "1 error");
            Assert.IsTrue(root.CompilerErrors[0] is ParserCompilerError, "correct error");
        }

        // Test that a properly declared namespace parses correctly
        [TestMethod]
        public void ParseNamespace()
        {
            string code = @"
                            namespace foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a namespace
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Namespace, "child is namespace");

            Assert.AreEqual("foo", (root.ChildExpressions[0] as Namespace).GetFullName(), "correct name");
        }

        // Test that a properly declared nested namespace parses correctly
        [TestMethod]
        public void ParseNestedNamespace()
        {
            string code = @"
                            namespace foo:
                                namespace bar:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a namespace
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Namespace, "child is namespace");

            Assert.AreEqual("foo", (root.ChildExpressions[0] as Namespace).GetFullName(), "correct name");

            // Check that the only child of the parent namespace is a namespace
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions.Count, 1, "1 child of namespace");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is Namespace, "namespace child is namespace");

            Assert.AreEqual("foo.bar", (root.ChildExpressions[0].ChildExpressions[0] as Namespace).GetFullName(), "correct name nested");
        }

        // Test that a properly declared import parses correctly
        [TestMethod]
        public void ParseImport()
        {
            string code = @"
                            import foo";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that there is one import
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Import, "child is import");

            Assert.IsTrue((root.ChildExpressions[0] as Import).Name == "foo", "correct name");
        }

        // Test that a properly declared import list parses correctly
        [TestMethod]
        public void ParseImportList()
        {
            string code = @"
                            import foo, bar";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that there is one import
            Assert.AreEqual(root.ChildExpressions.Count, 2, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Import, "child is import");

            Assert.IsTrue((root.ChildExpressions[0] as Import).Name == "foo", "correct name 1");
            Assert.IsTrue((root.ChildExpressions[1] as Import).Name == "bar", "correct name 2");
        }

        // Test that a properly declared import list parses correctly
        [TestMethod]
        public void ParseImportList2()
        {
            string code = @"
                            import foo, 
                                bar, bar2";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that there is one import
            Assert.AreEqual(root.ChildExpressions.Count, 3, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Import, "child is import");

            Assert.IsTrue((root.ChildExpressions[0] as Import).Name == "foo", "correct name 1");
            Assert.IsTrue((root.ChildExpressions[1] as Import).Name == "bar", "correct name 2");
        }

        // Test that a properly declared nested import parses correctly
        [TestMethod]
        public void ParseNestedImport()
        {
            string code = @"
                            namespace foo:
                                import bar";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a namespace
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Namespace, "child is namespace");

            Assert.AreEqual("foo", (root.ChildExpressions[0] as Namespace).GetFullName(), "correct name");

            // Check that the child of the namespace is an import
            // Check that the only child of the parent namespace is a namespace
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions.Count, 1, "1 child of namespace");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is Import, "namespace child is import");

            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as Import).Name == "bar", "correct name nested");
        }
    }
}

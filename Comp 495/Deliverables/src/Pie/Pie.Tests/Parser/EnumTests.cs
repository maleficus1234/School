using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void ParseEnumNoName()
        {
            string code = @"
                            enum";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that one error was produced.
            Assert.AreEqual(root.CompilerErrors.Count, 1, "1 error");

            // Check that the correct error was generated
            Assert.IsTrue(root.CompilerErrors[0] is ParserCompilerError, "correct error");
        }

        [TestMethod]
        public void ParseEnumSimple()
        {
            string code = @"
                            enum Birds:
                                Raven
                                Swallow
                                Robin
                                Eagle
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0);

            // Check that an enum was produced
            Assert.IsTrue(root.ChildExpressions[0] is Pie.Expressions.Enum);

            // Check the values in the enum
            var e = root.ChildExpressions[0] as Pie.Expressions.Enum;
            Assert.AreEqual("Birds", e.UnqualifiedName);
            Assert.AreEqual("Raven", e.Constants[0].Name);
            Assert.AreEqual(0, e.Constants[0].Value);
            Assert.AreEqual("Swallow", e.Constants[1].Name);
            Assert.AreEqual(1, e.Constants[1].Value);
            Assert.AreEqual("Robin", e.Constants[2].Name);
            Assert.AreEqual(2, e.Constants[2].Value);
            Assert.AreEqual("Eagle", e.Constants[3].Name);
            Assert.AreEqual(3, e.Constants[3].Value);
        }

        [TestMethod]
        public void ParseEnumAssignment()
        {
            string code = @"
                            enum Birds:
                                Raven
                                Swallow = 23
                                Robin
                                Eagle
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0);

            // Check that an enum was produced
            Assert.IsTrue(root.ChildExpressions[0] is Pie.Expressions.Enum);

            // Check the values in the enum
            var e = root.ChildExpressions[0] as Pie.Expressions.Enum;
            Assert.AreEqual("Birds", e.UnqualifiedName);
            Assert.AreEqual("Raven", e.Constants[0].Name);
            Assert.AreEqual(0, e.Constants[0].Value);
            Assert.AreEqual("Swallow", e.Constants[1].Name);
            Assert.AreEqual(23, e.Constants[1].Value);
            Assert.AreEqual("Robin", e.Constants[2].Name);
            Assert.AreEqual(24, e.Constants[2].Value);
            Assert.AreEqual("Eagle", e.Constants[3].Name);
            Assert.AreEqual(25, e.Constants[3].Value);
        }
    }
}

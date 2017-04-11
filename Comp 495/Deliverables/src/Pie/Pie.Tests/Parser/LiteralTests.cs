using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class LiteralTests
    {
        [TestMethod]
        public void ParseLiterals()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        b = 0b
                                        i = 1s
                                        i = 2us
                                        i = 3
                                        i = 4u
                                        i = 5L
                                        i = 6.0f
                                        i = 7.0
                                        i = 8.0d
                                        i = 9.0m 
                                        i = 10.0M
                                        s = ""whee""
                                        i = true
                                ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Validate each one.
            var method = root.ChildExpressions[0].ChildExpressions[0];

            Assert.IsTrue((method.ChildExpressions[0].ChildExpressions[1] as Literal).Value is Byte);
            Assert.IsTrue(((byte)(method.ChildExpressions[0].ChildExpressions[1] as Literal).Value) == 0);

            Assert.IsTrue((method.ChildExpressions[1].ChildExpressions[1] as Literal).Value is Int16);
            Assert.IsTrue(((Int16)(method.ChildExpressions[1].ChildExpressions[1] as Literal).Value) == 1);

            Assert.IsTrue((method.ChildExpressions[2].ChildExpressions[1] as Literal).Value is UInt16);
            Assert.IsTrue(((UInt16)(method.ChildExpressions[2].ChildExpressions[1] as Literal).Value) == 2);

            Assert.IsTrue((method.ChildExpressions[3].ChildExpressions[1] as Literal).Value is Int32);
            Assert.IsTrue(((Int32)(method.ChildExpressions[3].ChildExpressions[1] as Literal).Value) == 3);

            Assert.IsTrue((method.ChildExpressions[4].ChildExpressions[1] as Literal).Value is UInt32);
            Assert.IsTrue(((UInt32)(method.ChildExpressions[4].ChildExpressions[1] as Literal).Value) == 4);

            Assert.IsTrue((method.ChildExpressions[5].ChildExpressions[1] as Literal).Value is Int64);
            Assert.IsTrue(((Int64)(method.ChildExpressions[5].ChildExpressions[1] as Literal).Value) == 5);

            Assert.IsTrue((method.ChildExpressions[6].ChildExpressions[1] as Literal).Value is Single);
            Assert.IsTrue(((Single)(method.ChildExpressions[6].ChildExpressions[1] as Literal).Value) == 6);

            Assert.IsTrue((method.ChildExpressions[7].ChildExpressions[1] as Literal).Value is Double);
            Assert.IsTrue(((Double)(method.ChildExpressions[7].ChildExpressions[1] as Literal).Value) == 7);

            Assert.IsTrue((method.ChildExpressions[8].ChildExpressions[1] as Literal).Value is Double);
            Assert.IsTrue(((Double)(method.ChildExpressions[8].ChildExpressions[1] as Literal).Value) == 8);

            Assert.IsTrue((method.ChildExpressions[9].ChildExpressions[1] as Literal).Value is Decimal);
            Assert.IsTrue(((Decimal)(method.ChildExpressions[9].ChildExpressions[1] as Literal).Value) == 9);

            Assert.IsTrue((method.ChildExpressions[10].ChildExpressions[1] as Literal).Value is Decimal);
            Assert.IsTrue(((Decimal)(method.ChildExpressions[10].ChildExpressions[1] as Literal).Value) == 10);

            Assert.IsTrue((method.ChildExpressions[11].ChildExpressions[1] as Literal).Value is String);
            Assert.IsTrue(((String)(method.ChildExpressions[11].ChildExpressions[1] as Literal).Value) == "whee");

            Assert.IsTrue((method.ChildExpressions[12].ChildExpressions[1] as Literal).Value is bool);
            Assert.IsTrue(((bool)(method.ChildExpressions[12].ChildExpressions[1] as Literal).Value) == true);
        }

        [TestMethod]
        public void ParseIncorrectLiteral()
        {
            string code = @"
                                module bar:
                                    def void boo():
                                        b = 9999999999999999s
                                ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 1, "no errors");

            Assert.IsTrue(root.CompilerErrors[0] is ParserCompilerError);
        }
    }
}

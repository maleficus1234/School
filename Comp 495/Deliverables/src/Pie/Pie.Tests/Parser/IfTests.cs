using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class IfTests
    {
        [TestMethod]
        public void ParseSimpleIf()
        {
            string code = @"
module bar:
    void boo():
        if i == 0 return
";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).Conditional.ChildExpressions[0] is BinaryOperator);
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).TrueBlock.ChildExpressions[0] is Return);
        }

        [TestMethod]
        public void ParseBodiedIf()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        if i == 0:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).Conditional.ChildExpressions[0] is BinaryOperator);
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).TrueBlock.ChildExpressions[0] is Return);
        }

        [TestMethod]
        public void ParseSimpleIfSimpleElse()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        if i == 0 return 
                                        else return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).Conditional.ChildExpressions[0] is BinaryOperator);
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).TrueBlock.ChildExpressions[0] is Return);
        }

        [TestMethod]
        public void ParseSimpleIfBodiedElse()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        if i == 0 return 
                                        else:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).Conditional.ChildExpressions[0] is BinaryOperator);
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).TrueBlock.ChildExpressions[0] is Return);
        }

        [TestMethod]
        public void ParseBodiedIfBodiedElse()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        if i == 0:
                                            return 
                                        else:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).Conditional.ChildExpressions[0] is BinaryOperator);
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as IfBlock).TrueBlock.ChildExpressions[0] is Return);
        }
    }
}

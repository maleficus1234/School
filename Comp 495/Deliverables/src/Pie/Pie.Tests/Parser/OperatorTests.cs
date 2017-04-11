using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        public void ParseExplicitVariableAssignLiteral()
        {
            string code = @"
                                module bar:
                                    void foo():
                                        int boo = 2";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");


            // Check for the assignment
            var assignment = (root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as Assignment);
            Assert.IsTrue(assignment is Assignment, "is assignment");
            Assert.AreEqual("boo", (assignment.ChildExpressions[0] as ExplicitVariableDeclaration).Name, "assignable name");
            Assert.AreEqual(2, (assignment.ChildExpressions[1] as Literal).Value, "literal");
        }

        [TestMethod]
        public void ParseExplicitVariableAssignInvocation()
        {
            string code = @"
                                module bar:
                                    void foo():
                                        int boo = rawr()";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");


            // Check for the assignment
            var assignment = (root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as Assignment);
            Assert.IsTrue(assignment is Assignment, "is assignment");
            Assert.AreEqual("boo", (assignment.ChildExpressions[0] as ExplicitVariableDeclaration).Name, "assignable name");
            Assert.AreEqual("rawr", (assignment.ChildExpressions[1] as MethodInvocation).Name, "Invocation");
        }

        [TestMethod]
        public void ParseIndentedAssignment()
        {
            string code = @"
                                module bar:
                                    void foo():
                                        int boo = 
                                            rawr()";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");


            // Check for the assignment
            var assignment = (root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as Assignment);
            Assert.IsTrue(assignment is Assignment, "is assignment");
            Assert.AreEqual("boo", (assignment.ChildExpressions[0] as ExplicitVariableDeclaration).Name, "assignable name");
            Assert.AreEqual("rawr", (assignment.ChildExpressions[1] as MethodInvocation).Name, "Invocation");
        }

        [TestMethod]
        public void ParseIndentedAssignment2()
        {
            string code = @"
                                module bar:
                                    void foo():
                                        int boo =
                                              rawr()";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");


            // Check for the assignment
            var assignment = (root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as Assignment);
            Assert.IsTrue(assignment is Assignment, "is assignment");
            Assert.AreEqual("boo", (assignment.ChildExpressions[0] as ExplicitVariableDeclaration).Name, "assignable name");
            Assert.AreEqual("rawr", (assignment.ChildExpressions[1] as MethodInvocation).Name, "Invocation");
        }

        [TestMethod]
        public void ParseDeclareAssignAdd()
        {
            string code = @"
                                module bar:
                                    void foo():
                                        int boo = boo + 
                                            2";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseDeclareAssignAddMethodInvocation()
        {
            string code = @"
                                module bar:
                                    void foo():
                                        int boo = boo + 
                                            foo()";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBinaryOperator()
        {
            string code = @"
module bar:
    int foo():
        boo = boo + 2
                    ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBinaryOperatorIndented()
        {
            string code = @"
                                module bar:
                                    int foo():
                                        int boo = boo + 
                                            2";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBinaryOperators()
        {
            string code = @"
                                module bar:
                                    int foo():
                                        boo = boo + 1
                                        boo = boo - 2
                                        boo = boo * 3
                                        boo = boo / 3
                                        boo = boo < 2
                                        boo = boo <= 2
                                        boo = boo == 2
                                        boo = boo >= 2
                                        boo = boo > 2      
                                        boo = boo % 2           // Modulo     
                                        boo = boo << 2          // Bitwise shift left
                                        boo = boo >> 2          // Bitwise shift right
                                        boo = boo | 2           // Bitwise or
                                        boo = boo & 2           // Bitwise and
                                        boo = boo ^ 2           // Bitwise xor
                                        boo = boo || 2          // Logical or
                                        boo = boo && 2         // Logical and
                                    ";
                                   

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseUnaryOperators()
        {
            string code = @"
                                module bar:
                                    int foo():
                                        boo = !boo
                                        boo = -boo
                                    ";


            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        // Verify that each tier of operator precedence is correct.
        [TestMethod]
        public void ParseBinaryOperatorPrecedence()
        {
            string code = @"
module bar:
    int foo():
        boo = 1 || 1 && 1 | 1 ^ 1 & 1 == 1 < 1 << 1 + 1 * 1
";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            var assignment = root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as Assignment;
            Assert.AreEqual(BinaryOperatorType.LogicalOr,         (assignment.ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.LogicalAnd,        (assignment.ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.BitwiseOr,         (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.BitwiseXor,        (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.BitwiseAnd,        (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.Equal,             (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.Less,              (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.BitwiseShiftLeft,  (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.Add,               (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);
            Assert.AreEqual(BinaryOperatorType.Multiply,          (assignment.ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1].ChildExpressions[1] as BinaryOperator).OperatorType);

        }
    }
}

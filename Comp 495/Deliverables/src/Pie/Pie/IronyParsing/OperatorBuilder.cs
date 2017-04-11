
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class OperatorBuilder
    {
        // Build an assignment statement: i = 0, i += 1, i |= foo, etc
        public static void BuildAssignment(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var assignment = new Assignment(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(assignment);

            // Get the expression being assigned to
            parser.ConsumeParseTree(root, assignment, currentNode.ChildNodes[0]);

            // Determine what kind of assignment this is
            switch(currentNode.ChildNodes[1].FindTokenAndGetText())
            {
                case "=":
                    assignment.AssignmentType = AssignmentType.Equal;
                    break;
                case "+=":
                    assignment.AssignmentType = AssignmentType.Add;
                    break;
                case "-=":
                    assignment.AssignmentType = AssignmentType.Subtract;
                    break;
                case "*=":
                    assignment.AssignmentType = AssignmentType.Multiply;
                    break;
                case "/=":
                    assignment.AssignmentType = AssignmentType.Divide;
                    break;
                case "|=":
                    assignment.AssignmentType = AssignmentType.Or;
                    break;
                case "&=":
                    assignment.AssignmentType = AssignmentType.And;
                    break;
            }

            // Get the value being assigned.
            parser.ConsumeParseTree(root, assignment, currentNode.ChildNodes[2]);  
        }

        // Build a binary operator statement
        public static void BuildBinaryOperator(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var op = new BinaryOperator(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(op);

            // Determine which binary operator this is
            switch (currentNode.ChildNodes[1].Term.ToString())
            {
                case "*":
                    op.OperatorType = BinaryOperatorType.Multiply;
                    break;
                case "/":
                    op.OperatorType = BinaryOperatorType.Divide;
                    break;
                case "+":
                    op.OperatorType = BinaryOperatorType.Add;
                    break;
                case "-":
                    op.OperatorType = BinaryOperatorType.Subtract;
                    break;
                case "%":
                    op.OperatorType = BinaryOperatorType.Modulo;
                    break;
                case "<=":
                    op.OperatorType = BinaryOperatorType.LessOrEqual;
                    break;
                case "<":
                    op.OperatorType = BinaryOperatorType.Less;
                    break;
                case "==":
                    op.OperatorType = BinaryOperatorType.Equal;
                    break;
                case "!=":
                    op.OperatorType = BinaryOperatorType.NotEqual;
                    break;
                case ">=":
                    op.OperatorType = BinaryOperatorType.GreaterOrEqual;
                    break;
                case ">":
                    op.OperatorType = BinaryOperatorType.Greater;
                    break;
                case ">>":
                    op.OperatorType = BinaryOperatorType.BitwiseShiftRight;
                    break;
                case "<<":
                    op.OperatorType = BinaryOperatorType.BitwiseShiftLeft;
                    break;
                case "&&":
                    op.OperatorType = BinaryOperatorType.LogicalAnd;
                    break;
                case "||":
                    op.OperatorType = BinaryOperatorType.LogicalOr;
                    break;
                case "|":
                    op.OperatorType = BinaryOperatorType.BitwiseOr;
                    break;
                case "&":
                    op.OperatorType = BinaryOperatorType.BitwiseAnd;
                    break;
                case "^":
                    op.OperatorType = BinaryOperatorType.BitwiseXor;
                    break;
                case "as":
                    op.OperatorType = BinaryOperatorType.As;
                    break;
            }

            // Get the left operand
            parser.ConsumeParseTree(root, op, currentNode.ChildNodes[0]);

            // "as" operator is a type case and needs to be handled differently
            if(op.OperatorType == BinaryOperatorType.As)
            {
                var v = new VariableReference(op, null);
                v.Name = currentNode.ChildNodes[2].ChildNodes[0].FindTokenAndGetText();
                op.ChildExpressions.Add(v);
            }
            else
                parser.ConsumeParseTree(root, op, currentNode.ChildNodes[2]);
        }

        public static void BuildUnaryOperator(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var op = new UnaryOperator(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(op);
            switch(currentNode.ChildNodes[0].FindTokenAndGetText())
            {
                case "!":
                    op.OperatorType = UnaryOperatorType.Not;
                    break;
                case "-":
                    op.OperatorType = UnaryOperatorType.Negate;
                    break;
            }
            parser.ConsumeParseTree(root, op, currentNode.ChildNodes[1]);
        }
    }
}

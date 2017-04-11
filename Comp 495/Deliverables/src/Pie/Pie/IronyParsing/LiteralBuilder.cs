
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class LiteralBuilder
    {
        // Build a number, string, or character literal: v = 1234
        public static void BuildLiteral(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var e = new Literal(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);
            e.ParentExpression = parentExpression;

            e.Value = currentNode.Token.Value;  
        }

        // Build a boolean literal: v = true
        public static void BuildBoolLiteral(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var e = new Literal(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);
            e.ParentExpression = parentExpression;

            if (currentNode.ChildNodes[0].FindTokenAndGetText() == "true")
                e.Value = true;
            else
                e.Value = false;
        }

        // Build a null literal
        public static void BuildNullLiteral(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var e = new Literal(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);
            e.Value = null;
        }
    }
}


using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class IfBuilder
    {
        // Build an if conditional statement.
        public static void BuildIfBlock(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var i = new IfBlock(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(i);

            int c = 1;

            // Build the conditional expression
            parser.ConsumeParseTree(root, i.Conditional, currentNode.ChildNodes[c]);

            c++;

            // Build the true block
            parser.ConsumeParseTree(root, i.TrueBlock, currentNode.ChildNodes[c]);

            c++;

            // Build the false block if one exists.
            if(currentNode.ChildNodes[c].ChildNodes.Count != 0)
                parser.ConsumeParseTree(root, i.FalseBlock, currentNode.ChildNodes[c]);
        }
    }
}

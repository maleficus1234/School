
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class WhileLoopBuilder
    {
        // Build a while loop statement (a for loop with just the conditional)
        public static void BuildWhileLoop(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var whileLoop = new WhileLoop(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(whileLoop);

            // Build the conditional expressoin
            parser.ConsumeParseTree(root, whileLoop.Condition, currentNode.ChildNodes[1]);

            // Build the statements that make up the body of the while loop
            parser.ConsumeParseTree(root, whileLoop, currentNode.ChildNodes[2]);
        }
    }
}


using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class ExceptionHandlerBuilder
    {
        // Build a try-catch-finally statement
        public static void BuildExceptionHandler(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var ex = new ExceptionHandler(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(ex);

            // Build the contents of the try block
            parser.ConsumeParseTree(root, ex.Try, currentNode.ChildNodes[0]);

            // Build each case block.
            if(currentNode.ChildNodes[1].ChildNodes.Count > 0)
            {
                foreach(var node in currentNode.ChildNodes[1].ChildNodes)
                {
                    var c = new CatchClause(ex, node.Token.Convert());
                    c.Type = node.ChildNodes[1].FindTokenAndGetText();
                    c.Name = node.ChildNodes[2].FindTokenAndGetText();
                    ex.CatchClauses.Add(c);
                    parser.ConsumeParseTree(root, c, node.ChildNodes[3]);
                }
            }

            // Build the finally block.
            parser.ConsumeParseTree(root, ex.Finally, currentNode.ChildNodes[2]);
        }

        // Build a throw expression
        public static void BuildThrow(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            Throw e = new Throw(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);
            e.ParentExpression = parentExpression;

            parser.ConsumeParseTree(root, e, currentNode.ChildNodes[1]);
        }
    }
}

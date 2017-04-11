
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class ForLoopBuilder
    {
        // Build a for loop statement.
        public static void BuildForLoop(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var loop = new ForLoop(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(loop);

            // Build the initializer expression for the loop.
            parser.ConsumeParseTree(root, loop.Initialization, currentNode.ChildNodes[1]);

            // Build the conditional expression for the loop.
            switch(currentNode.ChildNodes[3].ChildNodes[0].Term.ToString())
            {
                case "for_range":
                    parser.ConsumeParseTree(root, loop.Condition, currentNode.ChildNodes[3].ChildNodes[0].ChildNodes[0]);
                    parser.ConsumeParseTree(root, loop.Condition, currentNode.ChildNodes[3].ChildNodes[0].ChildNodes[2]);
                    break;
                default:
                    parser.ConsumeParseTree(root, loop.Condition, currentNode.ChildNodes[3].ChildNodes[0]);
                    break;
            }

            // Build the increment/decrement step expression for the loop.
            if(currentNode.ChildNodes[4].ChildNodes.Count > 0)
            {
                parser.ConsumeParseTree(root, loop.Step, currentNode.ChildNodes[4]);
            }
            else
            {
                var literal = new Literal(loop, null);
                literal.Value = 1;
                loop.Step.ChildExpressions.Add(literal);
            }
            
            // Form the body of the loop.
            if(currentNode.ChildNodes[5].ChildNodes.Count > 0)
            {
                parser.ConsumeParseTree(root, loop, currentNode.ChildNodes[5]);
            }
        }
    }
}


using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal class SwitchBlockBuilder
    {
        // Build a switch block statement
        public static void BuildSwitchBlock(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var s = new SwitchBlock(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(s);

            // Get the expression being tested
            parser.ConsumeParseTree(root, s.Variable, currentNode.ChildNodes[1]);

            // Build each case block
            if(currentNode.ChildNodes[2].ChildNodes.Count > 0)
                foreach(var node in currentNode.ChildNodes[2].ChildNodes[0].ChildNodes)
                    BuildCaseBlock(parser, root, s, node);
        }

        // Build a case block statement
        public static void BuildCaseBlock(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var caseBlock = new CaseBlock(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(caseBlock);

            // Get the expression being tested against
            parser.ConsumeParseTree(root, caseBlock.Variable, currentNode.ChildNodes[1]);

            // Build the expressions in the body of the case block
            foreach (var e in currentNode.ChildNodes[2].ChildNodes)
                parser.ConsumeParseTree(root, caseBlock, e);
        }
    }
}

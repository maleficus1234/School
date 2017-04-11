
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class NamespaceBuilder
    {
        // Build an import expression: one for each name imported.
        public static void BuildImport(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            foreach (var node in currentNode.ChildNodes[1].ChildNodes)
            {
                var i = new Import(parentExpression, currentNode.FindToken().Convert());
                parentExpression.ChildExpressions.Add(i);
                i.ParentExpression = parentExpression;
                i.Name = node.FindTokenAndGetText();
            }
        }

        // Build a namespace expression
        public static void BuildNamespace(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            Namespace n = new Namespace(parentExpression, currentNode.FindToken().Convert());
            n.Name = currentNode.ChildNodes[1].FindTokenAndGetText();
            parentExpression.ChildExpressions.Add(n);

            parser.ConsumeParseTree(root, n, currentNode.ChildNodes[2]);
        }
    }
}

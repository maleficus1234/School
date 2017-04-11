
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal class IndexedIdentifierBuilder
    {
        // Build an indexed identifier expression (foo[0])
        public static void BuildIndexedIdentifier(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var indexedIdentifier = new IndexedIdentifier(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(indexedIdentifier);

            indexedIdentifier.Name = currentNode.ChildNodes[0].FindTokenAndGetText();
            parser.ConsumeParseTree(root, indexedIdentifier, currentNode.ChildNodes[2]);
        }
    }
}

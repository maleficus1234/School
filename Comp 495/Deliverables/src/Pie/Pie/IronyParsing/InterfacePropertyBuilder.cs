
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class InterfacePropertyBuilder
    {
        // Build an interface property declaration
        public static void BuildInterfaceProperty(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var property = new Property(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(property);

            property.TypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[0].FindTokenAndGetText());
            property.Name = currentNode.ChildNodes[0].ChildNodes[1].FindTokenAndGetText();
        }
    }
}

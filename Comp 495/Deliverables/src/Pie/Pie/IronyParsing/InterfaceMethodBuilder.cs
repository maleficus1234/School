
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class InterfaceMethodBuilder
    {
        // Build an interface method declaration.
        public static void BuildInterfaceMethod(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var method = new MethodDeclaration(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(method);

            // Build the return type of the interface method.
            method.ReturnTypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[0].FindTokenAndGetText());

            // The name of the interface method.
            method.Name = currentNode.ChildNodes[0].ChildNodes[1].FindTokenAndGetText();

            // Build the list of generic type names.
            if (currentNode.ChildNodes[1].ChildNodes.Count > 0)
            {
                var generics = currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[1];
                foreach (string s in IronyParser.InterpretList(generics))
                    method.GenericTypeNames.Add(parser.CheckAlias(s));
            }

            // Build the arguments of the method
            if (currentNode.ChildNodes[2].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[2].ChildNodes)
                {
                    MethodDeclarationBuilder.BuildArgument(parser, method, n.ChildNodes[0]);
                }
            }
        }
    }
}

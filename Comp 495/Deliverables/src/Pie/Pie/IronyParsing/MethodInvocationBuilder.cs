
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class MethodInvocationBuilder
    {
        // Build a method invocation statement (foo(1))
        public static void BuildMethodInvocation(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var methodInvocation = new MethodInvocation(parentExpression, currentNode.FindToken().Convert());
            methodInvocation.Name = currentNode.ChildNodes[0].FindTokenAndGetText();
            parentExpression.ChildExpressions.Add(methodInvocation);

            // interpret the expressions that are passed to the invocation as arguments
            if (currentNode.ChildNodes[1].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[1].ChildNodes)
                {
                    parser.ConsumeParseTree(root, methodInvocation.Parameters, n);

                }
            }
        }

        // Build a method invocation with generic type names
        public static void BuildGenericMethodInvocation(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var methodInvocation = new MethodInvocation(parentExpression, currentNode.FindToken().Convert());
            methodInvocation.Name = currentNode.ChildNodes[0].FindTokenAndGetText();
            parentExpression.ChildExpressions.Add(methodInvocation);
            methodInvocation.ParentExpression = parentExpression;

            // Interpret the generic type names
            if(currentNode.ChildNodes[2].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[2].ChildNodes)
                    methodInvocation.GenericTypes.Add(n.FindTokenAndGetText());
            }

            // interpret the expressions that are passed to the invocation as arguments
            if (currentNode.ChildNodes[4].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[1].ChildNodes)
                {
                    parser.ConsumeParseTree(root, methodInvocation.Parameters, n);
                }
            }
        }
    }
}


using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class InstantiationBuilder
    {
        // Build an instantiation expression "new foo()"
        public static void BuildInstantiation(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var ist = new Instantiation(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(ist);

            // A non-array instantiation
            if (currentNode.ChildNodes[1].ChildNodes[0].Term.ToString() == "new_instantiate")
            {
               
                ist.Name = parser.CheckAlias(currentNode.ChildNodes[1].ChildNodes[0].FindTokenAndGetText());

                if (currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes.Count > 0)
                    foreach (var n in currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes)
                    {
                        ist.GenericTypes.Add(parser.CheckAlias(n.FindTokenAndGetText()));
                    }

                foreach (var n in currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes)
                {
                    parser.ConsumeParseTree(root, ist.Parameters, n);
                }
            }
            else // An array instantiation
            {
                ist.IsArray = true;
                ist.Name = parser.CheckAlias(currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[0].FindTokenAndGetText());

                parser.ConsumeParseTree(root, ist.Parameters, currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[2]);
            }
        }
    }
}

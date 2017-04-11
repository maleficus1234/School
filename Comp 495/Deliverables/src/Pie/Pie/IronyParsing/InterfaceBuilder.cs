
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class InterfaceBuilder
    {
        // Build an interface expression.
        public static void BuildInterface(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var inter = new Interface(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(inter);

            int i = 0;

            // Find modifiers for the declaration
            InterpretModifiers(root, inter, currentNode.ChildNodes[i]);

            i++;

            i++;

            // get interface name
            inter.UnqualifiedName = currentNode.ChildNodes[i].FindTokenAndGetText();

            i++;

            // Build the generic type list
            if (currentNode.ChildNodes[i].ChildNodes.Count > 0)
            {
                var generics = currentNode.ChildNodes[i].ChildNodes[0].ChildNodes[1];
                foreach (string s in IronyParser.InterpretList(generics))
                    inter.GenericTypeNames.Add(s);
            }

            i++;

            // Build the base type list.
            if (currentNode.ChildNodes[i].ChildNodes.Count > 0)
            {
                var baseTypes = currentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0];
                foreach (string s in IronyParser.InterpretList(baseTypes))
                    inter.BaseTypeNames.Add(s);
            }

            i += 1;

            // Build the children of the interface
            parser.ConsumeParseTree(root, inter, currentNode.ChildNodes[i]);
        }

        // Interpret the interface declaration modifiers.
        static void InterpretModifiers(Root root, Interface inter, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;

            foreach (var modifierNode in node.ChildNodes)
            {
                string text = modifierNode.FindTokenAndGetText();

                if (text == "public")
                {
                    if (foundPublic || foundInternal)
                    {
                        root.CompilerErrors.Add(new MultipleAccessModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        inter.Accessibility = Accessibility.Public;
                        foundPublic = true;
                    }
                }

                if (text == "internal")
                {
                    if (foundInternal || foundPublic)
                    {
                        root.CompilerErrors.Add(new MultipleAccessModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        inter.Accessibility = Accessibility.Internal;
                        foundInternal = true;
                    }
                }
            }
        }
    }
}

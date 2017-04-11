using System;

using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class EnumBuilder
    {
        // Build an enum expression
        public static void BuildEnum(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            Pie.Expressions.Enum e = new Pie.Expressions.Enum(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(e);

            int i = 0;

            // Find modifiers
            InterpretModifiers(root, e, currentNode.ChildNodes[i]);

            i+=2;

            e.UnqualifiedName = currentNode.ChildNodes[i].FindTokenAndGetText();

            i+=1;

            int enumValue = 0;

            // Get the constants defined by this enum: check that they have values assigned.
            if(currentNode.ChildNodes[i].ChildNodes.Count > 0)
            {
                foreach(var node in currentNode.ChildNodes[i].ChildNodes[0].ChildNodes)
                {
                    if(node.ChildNodes[0].Term.ToString() == "identifier_constant")
                    {
                        e.Constants.Add(new EnumConstant(node.ChildNodes[0].FindTokenAndGetText(), enumValue++));
                    }

                    if (node.ChildNodes[0].Term.ToString() == "assignment_constant")
                    {
                        enumValue = Int32.Parse(node.ChildNodes[0].ChildNodes[2].FindTokenAndGetText());
                        e.Constants.Add(new EnumConstant(node.ChildNodes[0].FindTokenAndGetText(), enumValue++));
                    }
                }
            }
        }

        // Interpret the modifiers for this enum declaration.
        static void InterpretModifiers(Root root, Pie.Expressions.Enum c, ParseTreeNode node)
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
                        c.Accessibility = Accessibility.Public;
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
                        c.Accessibility = Accessibility.Internal;
                        foundInternal = true;
                    }
                }
            }
        }
    }
}

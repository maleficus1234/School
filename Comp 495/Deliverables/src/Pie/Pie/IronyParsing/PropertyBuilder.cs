
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal class PropertyBuilder
    {
        // Build property declaration statement
        public static void BuildProperty(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var property = new Property(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(property);

            // If the parent is a module, make the property shared
            var c = parentExpression as Class;
            if (c.IsModule)
            {
                property.IsShared = true;
            }

            // Interpret the modifiers for the property declaration
            InterpretModifiers(root, property, currentNode.ChildNodes[0].ChildNodes[0]);

            // Check for conflicting/invalid property modifiers
            if (property.IsShared && (property.IsFinal || property.IsOverride))
                root.CompilerErrors.Add(new IncompatibleModifiersCompilerError("", currentNode.FindToken().Location.Line,
                    currentNode.FindToken().Location.Position));

            // Find the return type for the property: check if it's generic or an array
            var typeNode = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0];
            if(typeNode.ChildNodes[0].Term.ToString() == "array")
            {
                property.Name = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText();
                property.TypeName = parser.CheckAlias(typeNode.ChildNodes[0].ChildNodes[0].FindTokenAndGetText()) + "[]";
            }
            else if (typeNode.ChildNodes[0].Term.ToString() == "generic_identifier")
            {
                property.Name = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText();
                property.TypeName = typeNode.ChildNodes[0].ChildNodes[0].FindTokenAndGetText() + "<";
                for(int i = 0; i < typeNode.ChildNodes[0].ChildNodes[2].ChildNodes.Count; i++)
                {
                    property.TypeName += typeNode.ChildNodes[0].ChildNodes[2].ChildNodes[i].FindTokenAndGetText();
                    if (i < typeNode.ChildNodes[0].ChildNodes[2].ChildNodes.Count - 1)
                        property.TypeName += ",";
                }
                property.TypeName += ">";
            }
            else
            {
                property.Name = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText();
                property.TypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].FindTokenAndGetText());
            }

            // Build the get block for the property
            parser.ConsumeParseTree(root, property.GetBlock, currentNode.ChildNodes[1]);

            // Build the set block for the property
            parser.ConsumeParseTree(root, property.SetBlock, currentNode.ChildNodes[2]);
        }

        // Build a value expression: "value" keyword is used to get the value in the setter
        public static void BuildValue(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var v = new Value(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(v);
        }

        // Interpret the property declaration modifiers
        static void InterpretModifiers(Root root, Property c, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;
            bool foundFinal = false;
            bool foundShared = false;
            bool foundAbstract = false;
            bool foundPrivate = false;
            bool foundProtected = false;
            bool foundOverride = false;
            bool foundVirtual = false;

            foreach (var modifierNode in node.ChildNodes)
            {
                string text = modifierNode.FindTokenAndGetText();

                if (text == "public")
                {
                    if (foundPublic || foundInternal || foundPrivate || foundProtected)
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
                    if (foundInternal || foundPublic || foundPrivate || foundProtected)
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

                if (text == "protected")
                {
                    if (foundInternal || foundPublic || foundPrivate || foundProtected)
                    {
                        root.CompilerErrors.Add(new MultipleAccessModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.Accessibility = Accessibility.Protected;
                        foundProtected = true;
                    }
                }

                if (text == "private")
                {
                    if (foundInternal || foundPublic || foundPrivate || foundProtected)
                    {
                        root.CompilerErrors.Add(new MultipleAccessModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.Accessibility = Accessibility.Private;
                        foundPrivate = true;
                    }
                }

                if (text == "final")
                {
                    if (foundFinal)
                    {
                        root.CompilerErrors.Add(new DuplicateModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.IsFinal = true;
                        foundFinal = true;
                    }
                }


                if (text == "abstract")
                {
                    if (foundAbstract)
                    {
                        root.CompilerErrors.Add(new DuplicateModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.IsAbstract = true;
                        foundAbstract = true;
                    }
                }

                if (text == "shared")
                {
                    if (foundShared)
                    {
                        root.CompilerErrors.Add(new DuplicateModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.IsShared = true;
                        foundShared = true;
                    }
                }

                if (text == "virtual")
                {
                    if (foundVirtual)
                    {
                        root.CompilerErrors.Add(new DuplicateModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.IsVirtual = true;
                        foundVirtual = true;
                    }
                }

                if (text == "override")
                {
                    if (foundOverride)
                    {
                        root.CompilerErrors.Add(new DuplicateModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        c.IsOverride = true;
                        foundOverride = true;
                    }
                }
            }
        }
    }
}


using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class MethodDeclarationBuilder
    {
        // Build a method declaration expression
        public static void BuildMethodDeclaration(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            MethodDeclaration e = new MethodDeclaration(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);

            int i = 0;

            // Set default modifiers
            var c = parentExpression as Class;
            if(c.IsModule) // If the parent is a module, set the method to shared.
            {
                e.IsShared = true;
            }

            // Interpret the modifiers for the method declaration
            InterpretModifiers(root, e, currentNode.ChildNodes[0].ChildNodes[0]);

            if(e.IsShared && (e.IsFinal || e.IsOverride))
                root.CompilerErrors.Add(new IncompatibleModifiersCompilerError("", currentNode.FindToken().Location.Line, 
                    currentNode.FindToken().Location.Position));

            i+=1; // skip the def

            // Interpret the return type name: check if it's an array, generic, or simple type name
            if (currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Term.ToString() == "array")
            {
                e.Name = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText();
                e.ReturnTypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].FindTokenAndGetText()) + "[]";
            }
            else if (currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Term.ToString() == "generic_identifier")
            {
                var genericNode = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0];
                e.Name = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText());
                e.ReturnTypeName = parser.CheckAlias(genericNode.ChildNodes[0].FindTokenAndGetText()) + "<";
                for(int n = 0; n < genericNode.ChildNodes[2].ChildNodes.Count; n++)
                {
                    e.ReturnTypeName += parser.CheckAlias(genericNode.ChildNodes[2].ChildNodes[n].FindTokenAndGetText());
                    if(n < genericNode.ChildNodes[2].ChildNodes.Count - 1)
                        e.ReturnTypeName += ",";
                }
                e.ReturnTypeName += ">";
            }
            else
            {
                e.Name = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText();
                e.ReturnTypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].FindTokenAndGetText());
            }

            i++;

            i++;

            // Add the generic type names for the method declaration
            if (currentNode.ChildNodes[1].ChildNodes.Count > 0)
            {
                var generics = currentNode.ChildNodes[1].ChildNodes[0].ChildNodes[1];
                foreach (string s in IronyParser.InterpretList(generics))
                    e.GenericTypeNames.Add(s);
            }

            i+=1;

            // add the arguments for the method declaration
            if (currentNode.ChildNodes[2].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[2].ChildNodes)
                {
                    BuildArgument(parser, e, n.ChildNodes[0]);
                }
            }

            // Build the body of statements in the method declaration
            parser.ConsumeParseTree(root, e, currentNode.ChildNodes[3]);

            
        }

        // Interpret the modifiers for the method declaration
        static void InterpretModifiers(Root root, MethodDeclaration c, ParseTreeNode node)
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

        // Build the arguments for the method: check if it's directioned, array, generic, or simple
        public static void BuildArgument(IronyParser parser, MethodDeclaration method, ParseTreeNode node)
        {
            if (node.Term.ToString() == "out_parameter")
            {
                var a = new DirectionedParameter(null, node.FindToken().Convert());
                switch(node.ChildNodes[0].ChildNodes[0].Term.ToString())
                {
                    case "ref":
                        a.Direction = ParameterDirection.Ref;
                        break;
                    case "out":
                        a.Direction = ParameterDirection.Out;
                        break;


                }
                a.TypeName = parser.CheckAlias(node.ChildNodes[1].FindTokenAndGetText());
                a.Name = node.ChildNodes[2].FindTokenAndGetText();
                method.Parameters.Add(a);
            }
            else if(node.Term.ToString() == "array_parameter")
            {
                var a = new SimpleParameter(null, node.FindToken().Convert());
                a.TypeName = parser.CheckAlias(node.ChildNodes[0].FindTokenAndGetText()) + "[]";
                a.Name = node.ChildNodes[3].FindTokenAndGetText();
                method.Parameters.Add(a);
            }
            else if(node.Term.ToString() == "generic_parameter")
            {
                var a = new SimpleParameter(null, node.FindToken().Convert());
                a.TypeName = node.ChildNodes[0].ChildNodes[0].FindTokenAndGetText()+"<";
                for(int i = 0; i < node.ChildNodes[0].ChildNodes[2].ChildNodes.Count; i++)
                {
                    a.TypeName += parser.CheckAlias(node.ChildNodes[0].ChildNodes[2].ChildNodes[i].FindTokenAndGetText());
                    if (i < node.ChildNodes[0].ChildNodes[2].ChildNodes.Count - 1)
                        a.TypeName += ",";
                }
                a.TypeName += ">";
                a.Name = node.ChildNodes[1].FindTokenAndGetText();
                method.Parameters.Add(a);
            }
            else
            {
                var a = new SimpleParameter(null, node.FindToken().Convert());
                a.TypeName = parser.CheckAlias(node.ChildNodes[0].FindTokenAndGetText());
                a.Name = node.ChildNodes[1].FindTokenAndGetText();
                method.Parameters.Add(a);
            }
        }
    }
}

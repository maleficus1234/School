
using Irony.Parsing;

using Pie.Expressions;
namespace Pie.IronyParsing
{
    internal static class DelegateBuilder
    {
        // Build a delegate declaration expression.
        public static void BuildDelegate(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var d = new DelegateDeclaration(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(d);

            // Interpret the declaration modifiers.
            InterpretModifiers(root, d, currentNode.ChildNodes[0]);

            // Find the return type of the declaration: check for array and generic types differently.
            if (currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].Term.ToString() == "array")
            {
                d.ReturnTypeName = parser.CheckAlias(currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].FindTokenAndGetText()) + "[]";
            }
            else if (currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].Term.ToString() == "generic_identifier")
            {

                string returnType = currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].FindTokenAndGetText() + "<";
                for(int i = 0; i < currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes.Count; i++)
                {
                    var genericNode = currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[i];
                    returnType += parser.CheckAlias(genericNode.FindTokenAndGetText());
                    if (i < currentNode.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes.Count - 1)
                        returnType += ",";
                }
                returnType += ">";
                d.ReturnTypeName = returnType;
            }
            else
                d.ReturnTypeName = parser.CheckAlias(currentNode.ChildNodes[2].ChildNodes[0].FindTokenAndGetText());

            d.Name = currentNode.ChildNodes[2].ChildNodes[1].FindTokenAndGetText();

            // Build the arguments of the delegate declaration.
            if (currentNode.ChildNodes[3].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[3].ChildNodes)
                {
                    BuildArgument(parser, d, n.ChildNodes[0]);
                }
            }
        }

        // Build an event declaration.
        public static void BuildEvent(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var e = new Event(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(e);

            InterpretModifiers(root, e, currentNode.ChildNodes[0]);

            e.DelegateName = currentNode.ChildNodes[2].ChildNodes[0].FindTokenAndGetText();
            e.Name = currentNode.ChildNodes[2].ChildNodes[1].FindTokenAndGetText();
        }

        // Build the arguments of the delegate declaration.
        public static void BuildArgument(IronyParser parser, DelegateDeclaration method, ParseTreeNode node)
        {
            if (node.Term.ToString() == "out_parameter")
            {
                var a = new DirectionedParameter(null, node.FindToken().Convert());
                switch (node.ChildNodes[0].ChildNodes[0].Term.ToString())
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
            else if (node.Term.ToString() == "array_parameter")
            {
                var a = new SimpleParameter(null, node.FindToken().Convert());
                a.TypeName = parser.CheckAlias(node.ChildNodes[0].FindTokenAndGetText()) + "[]";
                a.Name = node.ChildNodes[3].FindTokenAndGetText();
                method.Parameters.Add(a);
            }
            else if(node.Term.ToString() == "generic_parameter")
            {
                var a = new SimpleParameter(null, node.FindToken().Convert());
                string typeName = node.ChildNodes[0].ChildNodes[0].FindTokenAndGetText() + "<";
                for (int i = 0; i < node.ChildNodes[0].ChildNodes[2].ChildNodes.Count; i++ )
                {

                    typeName += parser.CheckAlias(node.ChildNodes[0].ChildNodes[2].ChildNodes[i].FindTokenAndGetText());
                    if (i < node.ChildNodes[0].ChildNodes[2].ChildNodes.Count - 1)
                        typeName += ",";
                }
                typeName += ">";
                a.TypeName = typeName;
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

        static void InterpretModifiers(Root root, DelegateDeclaration c, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;

            bool foundShared = false;

            bool foundPrivate = false;
            bool foundProtected = false;


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


            }
        }

        static void InterpretModifiers(Root root, Event c, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;

            bool foundShared = false;

            bool foundPrivate = false;
            bool foundProtected = false;


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
            }
        }
    }
}


using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    static class ConstructorBuilder
    {
        // Build a constructor expression.
        public static void BuildConstructor(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var ctor = new Constructor(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(ctor);

            // Interpret the declaration modifiers.
            InterpretModifiers(root, ctor, currentNode.ChildNodes[0]);

            // Interpret the arguments of the constructor.
            if (currentNode.ChildNodes[2].ChildNodes.Count > 0)
            {
                foreach (var n in currentNode.ChildNodes[2].ChildNodes)
                {
                    BuildArgument(parser, ctor, n.ChildNodes[0]);
                }
            }

            // Build the "this" or "base" constructors called, if any.
            if(currentNode.ChildNodes[3].ChildNodes.Count > 0)
            {
                if(currentNode.ChildNodes[3].ChildNodes[0].ChildNodes[0].Term.ToString() == "this")
                {
                    ctor.Sub = true;
                }
                foreach (var n in currentNode.ChildNodes[3].ChildNodes[0].ChildNodes[1].ChildNodes)
                {
                    parser.ConsumeParseTree(root, ctor.SubParameters, n);
                }
            }

            // Build the expressions in the method body of the constructor.
            parser.ConsumeParseTree(root, ctor, currentNode.ChildNodes[4]);
        }

        // Builds an argument for the constructor.
        public static void BuildArgument(IronyParser parser, Constructor method, ParseTreeNode node)
        {
            // Check for a directioned argument (out, ref).
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
            else
            {
                // Build an undirectioned argument.
                var a = new SimpleParameter(null, node.FindToken().Convert());
                a.TypeName = parser.CheckAlias(node.ChildNodes[0].FindTokenAndGetText());
                a.Name = node.ChildNodes[1].FindTokenAndGetText();
                method.Parameters.Add(a);
            }
        }

        // Interpret the modifiers for the constructor declaration.
        static void InterpretModifiers(Root root, Constructor c, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;
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
            }
        }
    }
}

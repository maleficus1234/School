
using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    internal static class ClassBuilder
    {
        // Builds a "class" expression: really could be a class, struct, or module.
        public static void BuildClass(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            
            Class c = new Class(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(c);

            int i = 0;

            // Interpret the declaration modifiers (abstract, public, internal, etc).
            InterpretClassModifiers( root, c, currentNode.ChildNodes[i]);

            i++;

            // Determine if it's a class, module, or struct.
            switch(currentNode.ChildNodes[i].Term.ToString())
            {
                case "module":
                    c.IsModule = true;
                    c.IsFinal = true;
                    c.IsPartial = true;
                    break;
                case "struct":
                    c.IsStruct = true;
                    c.IsModule = false;
                    break;
                default:
                    c.IsStruct = false;
                    c.IsModule = false;
                    break;
            }

            i++;

            // Class name
            c.UnqualifiedName = currentNode.ChildNodes[i].FindTokenAndGetText();

            i++;

            // Get the generic type list.
            if (currentNode.ChildNodes[i].ChildNodes.Count > 0)
            {
                var generics = currentNode.ChildNodes[i].ChildNodes[0].ChildNodes[1];
                foreach (string s in IronyParser.InterpretList(generics))
                    c.GenericTypeNames.Add(s);
            }

            i++;

            // Get the base type list.
            if (currentNode.ChildNodes[i].ChildNodes.Count > 0)
            {
                var baseTypes = currentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0];
                foreach (string s in IronyParser.InterpretList(baseTypes))
                    c.BaseTypeNames.Add(s);
            }

            i+=1;

            // Build the child expressions of the class.
            parser.ConsumeParseTree(root, c, currentNode.ChildNodes[i]);      
        }

        // Build a class variable expression.
        public static void BuildClassVariable(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            ClassVariable c = new ClassVariable(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(c);

            // Set default modifiers
            var p = parentExpression as Class;
            if (p.IsModule)
            {
                c.IsShared = true;
            }
            if (p.IsStruct)
                c.Accessibility = Accessibility.Public;

            // Interpret declaration modifiers (shared, public, etc).
            InterpretClassVariableModifiers(root, c, currentNode.ChildNodes[0].ChildNodes[0]);

            // Treat array type and generic type declarations differently.
            if (currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Term.ToString() == "array")
            {
                c.TypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].FindTokenAndGetText()) + "[]";
            }
            else if (currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Term.ToString() ==
                     "generic_identifier")
            {
                var genericNode = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0];
                c.Name = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText());
                c.TypeName = parser.CheckAlias(genericNode.ChildNodes[0].FindTokenAndGetText()) + "<";
                for (int n = 0; n < genericNode.ChildNodes[2].ChildNodes.Count; n++)
                {
                    c.TypeName += parser.CheckAlias(genericNode.ChildNodes[2].ChildNodes[n].FindTokenAndGetText());
                    if (n < genericNode.ChildNodes[2].ChildNodes.Count - 1)
                        c.TypeName += ",";
                }
                c.TypeName += ">";
            }
            else
                c.TypeName = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].FindTokenAndGetText());

            // Get the variable name.
            c.Name = currentNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].FindTokenAndGetText();
        }

        // Interpret class variable declaration modifiers: make sure to check for duplicates or conflicting ones.
        static void InterpretClassVariableModifiers(Root root, ClassVariable c, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;
            bool foundShared = false;
            bool foundPrivate = false;
            bool foundProtected = false;
            bool foundFinal = false;

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

        // Interpret class declaration modifiers: make sure to check for duplicates or conflicting ones.
        static void InterpretClassModifiers(Root root, Class c, ParseTreeNode node)
        {
            bool foundPublic = false;
            bool foundInternal = false;
            bool foundFinal = false;
            bool foundPartial = false;
            bool foundAbstract = false;

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
                        if (c.IsModule)
                            root.CompilerErrors.Add(new ModuleModifierCompilerError("",
                                modifierNode.FindToken().Location.Line,
                                modifierNode.FindToken().Location.Position));
                        c.IsFinal = true;
                        foundFinal = true;
                    }
                }

                if (text == "partial")
                {
                    if (foundPartial)
                    {
                        root.CompilerErrors.Add(new DuplicateModifiersCompilerError("",
                            modifierNode.FindToken().Location.Line,
                            modifierNode.FindToken().Location.Position));
                    }
                    else
                    {
                        if (c.IsModule)
                            root.CompilerErrors.Add(new ModuleModifierCompilerError("",
                                modifierNode.FindToken().Location.Line,
                                modifierNode.FindToken().Location.Position));
                        c.IsPartial = true;
                        foundPartial = true;
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
                        if (c.IsModule)
                            root.CompilerErrors.Add(new ModuleModifierCompilerError("",
                                modifierNode.FindToken().Location.Line,
                                modifierNode.FindToken().Location.Position));
                        c.IsAbstract = true;
                        foundAbstract = true;
                    }
                }
            }
        }
    }
}

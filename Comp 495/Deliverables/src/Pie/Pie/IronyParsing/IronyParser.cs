using System;
using System.Collections.Generic;

using Irony.Parsing;

using Pie.Expressions;

namespace Pie.IronyParsing
{
    // The method signature that expression builders are expected to use.
    internal delegate void BuildExpressionDelegate(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode node);

    // Walks through the parse tree and builds the tree of expressions from it.
    internal class IronyParser
        : Parser
    {
        Irony.Parsing.Parser parser;

        // A dictionary of expression builder methods: the key is the name of the grammatical rule.
        Dictionary<string, BuildExpressionDelegate> buildDelegates;

        // A dictionary of aliases: for example "int" maps to "System.Int32"
        Dictionary<string, string> aliases;

        public IronyParser()
        {

            parser = new Irony.Parsing.Parser(new Pie.IronyParsing.Rules.PieGrammar());

            buildDelegates = new Dictionary<string, BuildExpressionDelegate>()
            {
                {"namespace", NamespaceBuilder.BuildNamespace},
                {"class_declaration", ClassBuilder.BuildClass},
                {"explicit_variable_declaration", BuildExplicitVariableDeclaration},
                {"method_declaration", MethodDeclarationBuilder.BuildMethodDeclaration},
                {"typed_method_declaration", MethodDeclarationBuilder.BuildMethodDeclaration},
                {"return", BuildReturn},
                {"return_value", BuildReturnValue},
                {"identifier", BuildVariableReference},
                {"number_literal", LiteralBuilder.BuildLiteral},
                {"string_literal", LiteralBuilder.BuildLiteral},
                {"char_literal", LiteralBuilder.BuildLiteral},
                {"null_literal", LiteralBuilder.BuildNullLiteral},
                {"bool_literal", LiteralBuilder.BuildBoolLiteral},
                {"assignment", OperatorBuilder.BuildAssignment},
                {"method_invocation", MethodInvocationBuilder.BuildMethodInvocation},
                {"generic_method_invocation", MethodInvocationBuilder.BuildGenericMethodInvocation},
                {"binary_operator", OperatorBuilder.BuildBinaryOperator},
                {"unary_operator", OperatorBuilder.BuildUnaryOperator},
                {"import", NamespaceBuilder.BuildImport},
                {"simple_if_block", IfBuilder.BuildIfBlock},
                {"bodied_if_block", IfBuilder.BuildIfBlock},
                {"enum_declaration", EnumBuilder.BuildEnum},
                {"simple_for_loop", ForLoopBuilder.BuildForLoop},
                {"bodied_for_loop", ForLoopBuilder.BuildForLoop},
                {"simple_while_loop", WhileLoopBuilder.BuildWhileLoop},
                {"bodied_while_loop", WhileLoopBuilder.BuildWhileLoop},
                {"out_argument", BuildDirectionedArgument},
                {"class_variable", ClassBuilder.BuildClassVariable},
                {"constructor_declaration", ConstructorBuilder.BuildConstructor},
                {"instantiation", InstantiationBuilder.BuildInstantiation},
                {"property_declaration", PropertyBuilder.BuildProperty},
                {"value", PropertyBuilder.BuildValue},
                {"exception_handling_block", ExceptionHandlerBuilder.BuildExceptionHandler},
                {"throw_statement", ExceptionHandlerBuilder.BuildThrow},
                {"switch_block", SwitchBlockBuilder.BuildSwitchBlock},
                {"case_block", SwitchBlockBuilder.BuildCaseBlock},
                {"delegate_declaration", DelegateBuilder.BuildDelegate},
                {"event_member", DelegateBuilder.BuildEvent},
                {"indexed_identifier", IndexedIdentifierBuilder.BuildIndexedIdentifier},
                {"interface_declaration", InterfaceBuilder.BuildInterface},
                {"method_interface", InterfaceMethodBuilder.BuildInterfaceMethod},
                {"property_interface", InterfacePropertyBuilder.BuildInterfaceProperty}
            };

            aliases = new Dictionary<string, string>();
            aliases.Add("int", "System.Int32");
            aliases.Add("long", "System.Int64");
            aliases.Add("float", "System.Single");
            aliases.Add("double", "System.Double");
            aliases.Add("decimal", "System.Decimal");
            aliases.Add("short", "System.Int16");
            aliases.Add("byte", "System.Byte");
            aliases.Add("char", "System.Char");
            aliases.Add("uint", "System.UInt32");
            aliases.Add("ushort", "System.UInt16");
            aliases.Add("ulong", "System.UInt64");
            aliases.Add("bool", "System.Boolean");
            aliases.Add("object", "System.Object");
        }

        // Return the alias for a name.
        internal string CheckAlias(string s)
        {
            if (aliases.ContainsKey(s))
                return aliases[s];
            return s;
        }

        // Convert an Irony token to one that is independant of Irony.
        public Pie.Expressions.Token ConvertToken(Irony.Parsing.Token ironyToken)
        {
            var t = new Pie.Expressions.Token();
            t.Column = ironyToken.Location.Column;
            t.Line = ironyToken.Location.Line;
            t.Position = ironyToken.Location.Position;
            return t;
        }

        // Build the Irony parse tree, then do a depth-first search through it, building an expression tree.
        public override void Parse(Root root, string text)
        {
            if (text.Trim() == "") return;

            ParseTree parseTree = parser.Parse(text);

            foreach(var parseError in parseTree.ParserMessages)
            {
                root.CompilerErrors.Add(new ParserCompilerError("", parseError.Location.Line, parseError.Location.Column, parseError.Message));
            }
            if (root.CompilerErrors.Count > 0) return;

            ConsumeParseTree(root, root, parseTree.Root);
        }

        // Recursively build expressions from Irony parse tree nodes in a depth-first manner.
        public void ConsumeParseTree(Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            
            foreach (ParseTreeNode n in currentNode.ChildNodes)
            {
                if (buildDelegates.ContainsKey(n.Term.ToString()))
                    buildDelegates[n.Term.ToString()].Invoke(this, root, parentExpression, n);
                else
                {
                    ConsumeParseTree(root, parentExpression, n);
                }
            }
        }

        // Build a variable declaration
        void BuildExplicitVariableDeclaration(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            ExplicitVariableDeclaration e = new ExplicitVariableDeclaration(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);

            // "int foo"
            if(currentNode.ChildNodes[0].ChildNodes[0].Term.ToString() == "simple_explicit_variable_declaration")
            {
                e.TypeName = CheckAlias(currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].FindTokenAndGetText());
                e.Name =                currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].FindTokenAndGetText();
            }

            // "var foo"
            if (currentNode.ChildNodes[0].ChildNodes[0].Term.ToString() == "implicit_variable_declaration")
            {
                e.TypeName = "var";
                e.Name = currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].FindTokenAndGetText();
            }

            // int [] foo
            if (currentNode.ChildNodes[0].ChildNodes[0].Term.ToString() == "array_explicit_variable_declaration")
            {
                e.TypeName = parser.CheckAlias(CheckAlias(currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].FindTokenAndGetText())) + "[]";
                e.Name = parser.CheckAlias(currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[3].FindTokenAndGetText());
            }
            
            // List{int} foo
            if(currentNode.ChildNodes[0].ChildNodes[0].Term.ToString() == "generic_explicit_variable_declaration")
            {
                e.TypeName = CheckAlias(currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].FindTokenAndGetText());

                foreach (ParseTreeNode n in currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes)
                {
                    e.GenericTypes.Add(parser.CheckAlias(n.FindTokenAndGetText()));
                }

                e.Name = currentNode.ChildNodes[0].ChildNodes[0].ChildNodes[4].FindTokenAndGetText();
            }

            
        }

        // Build a simple return statement that does not return a value.
        void BuildReturn(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            Return e = new Return(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);
            e.ParentExpression = parentExpression;
        }

        // Build a return statement that returns a value
        void BuildReturnValue(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            Return e = new Return(parentExpression, currentNode.FindToken().Convert());
            parentExpression.ChildExpressions.Add(e);
            e.ParentExpression = parentExpression;

            ConsumeParseTree(root, e, currentNode.ChildNodes[1]);
        }

        // Build a variable reference (the "v" in "v = 1"
        void BuildVariableReference(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            var e = new VariableReference(parentExpression, currentNode.FindToken().Convert());
            e.Name = currentNode.FindTokenAndGetText();
            e.ParentExpression = parentExpression;
            parentExpression.ChildExpressions.Add(e);
        }

        // Build an argument with a direction: out or ref
        void BuildDirectionedArgument(IronyParser parser, Root root, Expression parentExpression, ParseTreeNode currentNode)
        {
            DirectionedArgument o = new DirectionedArgument(parentExpression, currentNode.Token.Convert());
            parentExpression.ChildExpressions.Add(o);
            switch(currentNode.ChildNodes[0].FindTokenAndGetText())
            {
                case "out":
                    o.Direction = ParameterDirection.Out;
                    break;
                case "ref":
                    o.Direction = ParameterDirection.Ref;
                    break;
            }
            o.Name = currentNode.ChildNodes[1].FindTokenAndGetText();
        }


        // Interpret a list of identifiers
        public static List<string> InterpretList(ParseTreeNode node)
        {
            var list = new List<String>();
            foreach (var n in node.ChildNodes)
                list.Add(n.FindTokenAndGetText());

            return list;
        }
    }
}
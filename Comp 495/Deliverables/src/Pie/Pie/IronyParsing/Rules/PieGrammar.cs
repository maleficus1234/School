using System;

using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Irony requires that you implement a class derived from it's "Grammar" class
    // and define the grammatical rules with it.
    public class PieGrammar
        : Grammar
    {
        internal NonTerminal Assignable;
        internal IdentifierTerminal Identifier;
        internal NonTerminal Return;
        internal NonTerminal ExplicitVariableDeclaration;
        internal NonTerminal ImplicitVariableDeclaration;
        internal NonTerminal ReturnValue;
        internal NonTerminal MethodInvocation;
        internal NonTerminal GenericMethodInvocation;
        internal NonTerminal ValidConditionals;
        internal Terminal OpenParenthese;
        internal Terminal CloseParenthese;
        internal NonTerminal expression;
        internal KeywordRules Keywords;
        internal ClassRules Classes;
        internal LiteralRules Literals;
        internal NamespaceRules Namespaces;
        internal OperatorRules Operators;
        internal MethodDeclarationRules MethodDeclarations;
        internal ListRules Lists;
        internal IfBlockRules IfBlocks;
        internal EnumRules Enums;
        internal ForLoopRules ForLoops;
        internal WhileLoopRules WhileLoops;
        internal ConstructorRules Constructors;
        internal PropertyRules Properties;
        internal ExceptionRules Exceptions;
        internal SwitchRules Switches;
        internal DelegateRules Delegates;
        internal InterfaceRules Interfaces;
        internal NonTerminal TwoIdentifiers;
        internal NonTerminal Instantiation;
        internal NonTerminal ArgumentList;
        internal NonTerminal TypeName;
        internal NonTerminal GenericIdentifier;

        public PieGrammar()
        {
            

            // Module is the root of it all
            var root = new NonTerminal("root");
            this.Root = root;

            // Containers for various sets of related rules.
            Keywords = new Rules.KeywordRules(this);
            Operators = new Rules.OperatorRules(this);
            Literals = new Rules.LiteralRules(this);
            Namespaces = new Rules.NamespaceRules(this);
            Classes = new ClassRules(this);
            MethodDeclarations = new Rules.MethodDeclarationRules(this);
            Lists = new ListRules(this);
            IfBlocks = new IfBlockRules(this);
            Enums = new EnumRules(this);
            ForLoops = new ForLoopRules(this);
            WhileLoops = new WhileLoopRules(this);
            Constructors = new ConstructorRules(this);
            Properties = new PropertyRules(this);
            Exceptions = new ExceptionRules(this);
            Switches = new SwitchRules(this);
            Delegates = new DelegateRules(this);
            Interfaces = new InterfaceRules(this);

            // A type name, which my be simple, generic, or an array.
            TypeName = new NonTerminal("type_name");

            // Use to signify an identifer: don't create others, just use this one.
            Identifier = new IdentifierTerminal("identifier");
            Identifier.AllChars += '.';

            // An identifier with generic type names "List{int}"
            GenericIdentifier = new NonTerminal("generic_identifier");
            GenericIdentifier.Rule = Identifier + ToTerm("{") + Lists.PlusIdentifierList + ToTerm("}");

            // A typename and an identifier. This needs to be in a unique rule
            // or the parser gets confused between variable declarations and method declarations.
            TwoIdentifiers = new NonTerminal("twoidentifiers");
            TwoIdentifiers.Rule = TypeName + Identifier;

            // An expression that can be assigned a value.
            Assignable = new NonTerminal("Assignable");

            // Explicit variable declaration: "int i"
            ExplicitVariableDeclaration = new NonTerminal("explicit_variable_declaration");

            // A method invocation
            MethodInvocation = new NonTerminal("method_invocation");

            // A method invocation with generic type names.
            GenericMethodInvocation = new NonTerminal("generic_method_invocation");

            // Expression: pretty much anything found in a method body.
            expression = new NonTerminal("expression");

            // Implicit variable declaration: "var i = 0"
            ImplicitVariableDeclaration = new NonTerminal("implicit_variable_declaration");

            // List of valid conditional expressions
            ValidConditionals = new NonTerminal("valid_conditionals");

            // new foo()
            Instantiation = new NonTerminal("instantiation");

            // The list of valid arguments for a method invocation
            var validArguments = new NonTerminal("valid_arguments");
            // List of arguments for a method invocation
            ArgumentList = new NonTerminal("argument_list");

            // A return expression that does not return a value.
            Return = new NonTerminal("return");
            // Return expression that does return a value.
            ReturnValue = new NonTerminal("return_value");

            OpenParenthese = ToTerm("(");
            CloseParenthese = ToTerm(")");

            // A continue expression for continuing loops.
            var Continue = ToTerm("continue", "continue_keyword");
            Continue.Flags = TermFlags.IsReservedWord;

            // List of valid conditional expressions (for now, any expression).
            ValidConditionals.Rule = expression;

            // A directioned method argument "ref int i" or "out int i"
            var outArgument = new NonTerminal("out_argument");
            outArgument.Rule = (Keywords.Out | Keywords.Ref) + expression;

            // Comma separated list of method invocation arguments.
            ArgumentList.Rule = MakeStarRule(ArgumentList, ToTerm(","), validArguments);
            // List of valid method invocation arguments.
            validArguments.Rule = expression | outArgument;

            // An array: identifier []
            var array = new NonTerminal("array");
            array.Rule =  Identifier +ToTerm("[")+ ToTerm("]");

            // Identifier that is indexed: identifier[1]
            var indexedIdentifier = new NonTerminal("indexed_identifier");
            indexedIdentifier.Rule = Identifier + ToTerm("[") +expression+ ToTerm("]");

            // A type name can be a simple name, an array, or a generic.
            TypeName.Rule = Identifier | array | GenericIdentifier;

            // int i
            var simpleExplicitVariableDeclaration = new NonTerminal("simple_explicit_variable_declaration");
            simpleExplicitVariableDeclaration.Rule = Identifier + Identifier;

            // int [] i
            var arrayExplicitVariableDeclaration = new NonTerminal("array_explicit_variable_declaration");
            arrayExplicitVariableDeclaration.Rule = Identifier + ToTerm("[") + ToTerm("]") + Identifier;

            // List{int} i
            var genericExplicitVariableDeclaration = new NonTerminal("generic_explicit_variable_declaration");
            genericExplicitVariableDeclaration.Rule = Identifier + ToTerm("{") + Lists.StarIdentifierList + ToTerm("}") + Identifier;

            // var i
            ImplicitVariableDeclaration.Rule = Keywords.Var + Identifier;

            ExplicitVariableDeclaration.Rule = (simpleExplicitVariableDeclaration 
                | arrayExplicitVariableDeclaration
                | genericExplicitVariableDeclaration
                | ImplicitVariableDeclaration
                ) 
                + (Empty | Eos);

            // methodname(arguments)
            MethodInvocation.Rule = Identifier + OpenParenthese + ArgumentList + CloseParenthese + (Eos | Empty);
            GenericMethodInvocation.Rule = Identifier + ToTerm("{") + Lists.StarIdentifierList + ToTerm("}") + OpenParenthese + ArgumentList + CloseParenthese + (Eos | Empty);

            // Expressions that are "terminals" they don't go further, such as literals.
            var terms = new NonTerminal("terminals");
            var parExp = new NonTerminal("par_exp");

            expression.Rule = terms 
                | Operators.Assignment 
                | Operators.BinaryOperator 
                | Operators.UnaryOperator 
                | IfBlocks.SimpleIfBlock
                | IfBlocks.BodiedIfBlock
                | ForLoops.SimpleForLoop
                | ForLoops.BodiedForLoop
                | WhileLoops.SimpleWhileLoop
                | WhileLoops.BodiedWhileLoop
                | Delegates.EventMember
                | Return
                | ReturnValue
                | Instantiation
                | indexedIdentifier
                | Exceptions.ExceptionHandlingBlock
                | Exceptions.ThrowStatement
                | Switches.SwitchBlock;

            terms.Rule = Identifier
                | Literals.NumberLiteral
                | Literals.StringLiteral
                | Literals.BoolLiteral
                | Literals.Null
                | Literals.CharLiteral
                | ExplicitVariableDeclaration
                | MethodInvocation
                | GenericMethodInvocation
                | Keywords.Value
                | parExp;

            MarkTransient(terms);

            // An expression can be an expression nested in parenthese.
            parExp.Rule = ToTerm("(") + expression + ToTerm(")");
            
            // Explicit variable declarations and identifiers can be assigned to.
            Assignable.Rule = ExplicitVariableDeclaration | Identifier | indexedIdentifier;

            // continue to keep running a loop
            var continueKeyword = ToTerm("continue", "continue_keyword");
            continueKeyword.Flags = TermFlags.IsReservedWord;

            // Return keyword can simply return, or return a value.
            var returnKeyword = ToTerm("return", "return_keyword");
            returnKeyword.Flags = TermFlags.IsReservedWord;
            Return.Rule = returnKeyword + (Eos | Empty);
            ReturnValue.Rule = returnKeyword + expression + (Eos | Empty);

            // new foo() or new foo{int}()
            var newInstantiate = new NonTerminal("new_instantiate");
            newInstantiate.Rule = Identifier + Classes.GenericTypeListOpt + OpenParenthese + ArgumentList + CloseParenthese;

            // new foo[2]
            var arrayInstantiate = new NonTerminal("array_instantiate");
            arrayInstantiate.Rule = Identifier + ToTerm("[") + expression + ToTerm("]");

            // The different ways of instantiating.
            Instantiation.Rule = Keywords.New 
                + (newInstantiate | arrayInstantiate)
                + (Empty | Eos);

            Namespaces.Define();
            Classes.Define();
            MethodDeclarations.Define();
            Lists.Define();
            Operators.Define();
            IfBlocks.Define();
            Enums.Define();
            ForLoops.Define();
            WhileLoops.Define();
            Constructors.Define();
            Properties.Define();
            Exceptions.Define();
            Switches.Define();
            Delegates.Define();
            Interfaces.Define();

            // Comments defining comment tokens.
            var singleLineComment = new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            var delimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");

            NonGrammarTerminals.Add(singleLineComment);
            NonGrammarTerminals.Add(delimitedComment);
            NonGrammarTerminals.Add(ToTerm(@"\"));

            RegisterBracePair("(", ")");
            RegisterBracePair("{", "}");

            AddToNoReportGroup("(");
            AddToNoReportGroup(Eos); 

            MarkPunctuation(OpenParenthese, CloseParenthese, ToTerm(","), ToTerm(":"));

            root.Rule = Namespaces.NamespaceMembersList;

            this.LanguageFlags = LanguageFlags.NewLineBeforeEOF | LanguageFlags.SupportsBigInt;
        }

        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            // Make the language a whitespaced one: this instructs the parse to insert whitespace tokens.
            var outlineFilter = new CodeOutlineFilter(language.GrammarData,
              OutlineOptions.ProduceIndents | OutlineOptions.CheckOperator | OutlineOptions.CheckBraces, ToTerm(@"\"));
            filters.Add(outlineFilter);
        }
    }
}

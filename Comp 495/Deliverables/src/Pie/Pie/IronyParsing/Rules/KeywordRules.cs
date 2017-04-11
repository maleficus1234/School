
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Definition of all the keywords of the language.
    internal class KeywordRules
    {
        public Terminal Partial { get; private set; }
        public Terminal Import { get; private set; }
        public Terminal Public { get; private set; }
        public Terminal Private { get; private set; }
        public Terminal Internal { get; private set; }
        public Terminal Protected { get; private set; }
        public Terminal Virtual { get; private set; }
        public Terminal Final { get; private set; }
        public Terminal Const { get; private set; }
        public Terminal NamespaceKeyword { get; private set; }
        public Terminal Module { get; private set; }
        public Terminal Class { get; private set; }
        public Terminal Struct { get; private set; }
        public Terminal Enum { get; private set; }
        public Terminal As { get; private set; }
        public Terminal Of { get; private set; }
        public Terminal Abstract { get; private set; }

        public Terminal Override { get; private set; }
        public Terminal And { get; private set; }
        public Terminal Or { get; private set; }
        public Terminal If { get; private set; }
        public Terminal Else { get; private set; }
        public Terminal Shared { get; private set; }
        public Terminal Def { get; private set; }

        public Terminal For { get; private set; }
        public Terminal Step { get; private set;}
        public Terminal While { get; private set; }
        public Terminal In { get; private set; }
        public Terminal To { get; private set; }
        public Terminal True { get; private set; }
        public Terminal False { get; private set; }
        public Terminal Ref { get; private set; }
        public Terminal Out { get; private set; }
        public Terminal New { get; private set; }
        public Terminal Super { get; private set; }

        public Terminal Get { get; private set; }
        public Terminal Set { get; private set; }

        public Terminal Value { get; private set; }

        public Terminal Try { get; private set; }
        public Terminal Catch { get; private set; }
        public Terminal Finally { get; private set; }
        public Terminal Throw { get; private set; }

        public Terminal Switch { get; private set; }
        public Terminal Case { get; private set; }
        public Terminal Break { get; private set; }

        public Terminal Delegate { get; private set; }
        public Terminal Event { get; private set; }
        public Terminal Interface { get; private set; }

        public Terminal Var { get; private set; }

        PieGrammar grammar;

        public KeywordRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            Partial = grammar.ToTerm("partial");

            Import = grammar.ToTerm("import");

            Public = grammar.ToTerm("public");

            Private = grammar.ToTerm("private");

            Internal = grammar.ToTerm("internal");

            Protected = grammar.ToTerm("protected");

            Final = grammar.ToTerm("final");

            NamespaceKeyword = grammar.ToTerm("namespace");

            Module = grammar.ToTerm("module");

            Class = grammar.ToTerm("class");

            Struct = grammar.ToTerm("struct");

            Enum = grammar.ToTerm("enum");

            As = grammar.ToTerm("as");

            Of = grammar.ToTerm("of");

            Abstract = grammar.ToTerm("abstract");

            And = grammar.ToTerm("&&");

            Or = grammar.ToTerm("||");

            If = grammar.ToTerm("if");

            Else = grammar.ToTerm("else");

            Shared = grammar.ToTerm("shared");

            Def = grammar.ToTerm("def");

            Virtual = grammar.ToTerm("virtual");

            Override = grammar.ToTerm("override");

            For = grammar.ToTerm("for");
            Step = grammar.ToTerm("step");

            While = grammar.ToTerm("while");

            In = grammar.ToTerm("in");
            To = grammar.ToTerm("to");

            True = grammar.ToTerm("true");
            False = grammar.ToTerm("false");

            Ref = grammar.ToTerm("ref");
            Out = grammar.ToTerm("out");

            New = grammar.ToTerm("new");
            Super = grammar.ToTerm("super");

            Get = grammar.ToTerm("get");
            Set = grammar.ToTerm("set");

            Value = grammar.ToTerm("value");

            Try = grammar.ToTerm("try");
            Catch = grammar.ToTerm("catch");
            Finally = grammar.ToTerm("finally");
            Throw = grammar.ToTerm("throw");

            Switch = grammar.ToTerm("switch");
            Case = grammar.ToTerm("case");
            Break = grammar.ToTerm("break");

            Delegate = grammar.ToTerm("delegate");
            Event = grammar.ToTerm("event");

            Interface = grammar.ToTerm("interface");

            Var = grammar.ToTerm("var");

            // Mark them as "reserved" so that the parser can tell the syntax highlighter how to color them in the IDE.
            grammar.MarkReservedWords("partial", "import", "public", "private", "internal", "protected", "def", "shared", "virtual", "enum");
            grammar.MarkReservedWords("final", "namespace", "class", "module", "struct", "as", "of", "abstract", "and", "or", "if", "else");
            grammar.MarkReservedWords("for", "step", "true", "false", "in", "to", "while", "const", "new", "base", "this");
            grammar.MarkReservedWords("override", "get", "set", "value", "try", "catch", "finally", "throw", "switch", "case", "break");
            grammar.MarkReservedWords("delegate", "event", "interface", "var");
        }
    }
}


using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules for delegate and event declaration.
    internal class DelegateRules
    {
        PieGrammar grammar;

        // Rule defining a delegate declaration.
        public NonTerminal DelegateDeclaration { get; private set; }
        // Rule defining an event.
        public NonTerminal EventMember { get; private set; }

        public DelegateRules(PieGrammar grammar)
        {
            this.grammar = grammar;
            DelegateDeclaration = new NonTerminal("delegate_declaration");
            EventMember = new NonTerminal("event_member");
        }

        public void Define()
        {
            // public delegate void foo(int i)
            DelegateDeclaration.Rule =
                grammar.Classes.Modifier
                + grammar.Keywords.Delegate
                + grammar.TwoIdentifiers
                + grammar.OpenParenthese
                + grammar.MethodDeclarations.ParameterList
                + grammar.CloseParenthese
                + grammar.Eos;

            // public event somedelegate someevent
            EventMember.Rule =
                grammar.Classes.ClassVariableModifierList
                + grammar.Keywords.Event
                + grammar.TwoIdentifiers
                + grammar.Eos;
        }
    }
}

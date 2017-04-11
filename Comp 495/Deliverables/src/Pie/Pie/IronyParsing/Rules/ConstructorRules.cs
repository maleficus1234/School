
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Declare and define constructor grammatical rules.
    internal class ConstructorRules
    {
        public PieGrammar grammar;

        // The overall constructor declaration rule.
        public NonTerminal ConstructorDeclaration { get; private set; }

        public ConstructorRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            ConstructorDeclaration = new NonTerminal("constructor_declaration");
        }

        public void Define()
        {
            // The valid modifiers for a constructor: public, proteted, or private.
            var ctorValidModifiers = new NonTerminal("constructor_modifiers");
            ctorValidModifiers.Rule = grammar.Keywords.Public | grammar.Keywords.Protected | grammar.Keywords.Private;

            // The call to the base constructor.
            var ctorBase = new NonTerminal("constructor_base");
            ctorBase.Rule = grammar.ToTerm("base") 
                + grammar.OpenParenthese
                + grammar.ArgumentList 
                + grammar.CloseParenthese
                + grammar.ToTerm(":");

            // The call to another constructor in the same class.
            var ctorThis = new NonTerminal("constructor_this");
            ctorThis.Rule = grammar.ToTerm("this")
                + grammar.OpenParenthese
                + grammar.ArgumentList
                + grammar.CloseParenthese
                + grammar.ToTerm(":");

            // Optional call to base or this constructors.
            var ctorSubOpt = new NonTerminal("constructor_sub_opt");
            ctorSubOpt.Rule = ctorBase | ctorThis | grammar.Empty;

            /* new(int foo):base(foo):*/
            ConstructorDeclaration.Rule = grammar.MethodDeclarations.Modifier 
                + grammar.Keywords.New
                + grammar.OpenParenthese
                + grammar.MethodDeclarations.ParameterList
                + grammar.CloseParenthese
                + grammar.ToTerm(":")
                + ctorSubOpt
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;
        }
    }
}

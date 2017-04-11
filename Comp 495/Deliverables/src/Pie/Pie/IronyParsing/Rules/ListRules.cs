
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Just star and plus identifier rules definined in one place for re-use.
    internal class ListRules
    {
        // A list of zero or more comma separated identifiers.
        public NonTerminal StarIdentifierList { get; private set; }
        // A list of one or more comma separated identifiers.
        public NonTerminal PlusIdentifierList { get; private set; }

        PieGrammar grammar;

        public ListRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            StarIdentifierList = new NonTerminal("star_identifer_list");
            PlusIdentifierList = new NonTerminal("plus_identfier_list");
        }

        public void Define()
        {


            StarIdentifierList.Rule = grammar.MakeStarRule(StarIdentifierList, grammar.ToTerm(","), grammar.Identifier);
            PlusIdentifierList.Rule = grammar.MakePlusRule(PlusIdentifierList, grammar.ToTerm(","), grammar.Identifier);
            grammar.MarkTransient(StarIdentifierList);
        }


    }
}

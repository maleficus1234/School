
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules representing a switch block declaration.
    internal class SwitchRules
    {
        PieGrammar grammar;

        public NonTerminal SwitchBlock { get; private set; }

        public SwitchRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            SwitchBlock = new NonTerminal("switch_block");
        }

        public void Define()
        {
            /* case <expression>:
             *      <expression>
             */
            var caseBlock = new NonTerminal("case_block");
            caseBlock.Rule = grammar.Keywords.Case
                + grammar.expression
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;

            // List of case block expressions.
            var caseBlockList = new NonTerminal("case_block_list");
            caseBlockList.Rule = grammar.MakeStarRule(caseBlockList, caseBlock);

            // The list of case block expressions is optional.
            var caseBlockListOpt = new NonTerminal("case_block_list_opt");
            caseBlockListOpt.Rule = (grammar.Indent + caseBlockList +grammar.Dedent) | grammar.Empty;

            /* switch <expression>:
             *      case <expression>:
             *              <expression>
             */
            SwitchBlock.Rule = grammar.Keywords.Switch
                + grammar.expression
                + grammar.ToTerm(":")
                + grammar.Eos
                + caseBlockListOpt;
        }
    }
}

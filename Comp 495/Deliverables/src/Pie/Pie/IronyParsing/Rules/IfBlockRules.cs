
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // The grammatical rules for an if conditional.
    internal class IfBlockRules
    {
        // if true <expression>
        public NonTerminal SimpleIfBlock { get; private set; }
        /* if true:
         * <expression>
         */
        public NonTerminal BodiedIfBlock { get; private set; }
        // else <expression>
        public NonTerminal SimpleElseBlock { get; private set; }
        /* else:
         *      <expression>
         */
        public NonTerminal BodiedElseBlock { get; private set; }

        public PieGrammar grammar;

        public IfBlockRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            SimpleIfBlock = new NonTerminal("simple_if_block");
            BodiedIfBlock = new NonTerminal("bodied_if_block");
            SimpleElseBlock = new NonTerminal("simple_else_block");
            BodiedElseBlock = new NonTerminal("bodied_else_block");
        }

        public void Define()
        {
            // The else block is optional.
            var elseBlockOpt = new NonTerminal("else_block_opt");
            elseBlockOpt.Rule = grammar.Empty | SimpleElseBlock | BodiedElseBlock;

            SimpleElseBlock.Rule = grammar.Keywords.Else + grammar.expression;
            BodiedElseBlock.Rule = grammar.Keywords.Else 
                + grammar.ToTerm(":") 
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;

            SimpleIfBlock.Rule = grammar.Keywords.If
                + grammar.ValidConditionals
                + grammar.expression 
                + elseBlockOpt;
            BodiedIfBlock.Rule = grammar.Keywords.If
                + grammar.ValidConditionals
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody
                + elseBlockOpt;


        }
    }
}

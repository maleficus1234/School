
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules for property declarations.
    internal class PropertyRules
    {

        PieGrammar grammar;

        // The overall property grammatical rule.
        public NonTerminal PropertyDeclaration { get; private set; }

        public PropertyRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            PropertyDeclaration = new NonTerminal("property_declaration");
        }

        public void Define()
        {
            // get:
            //      <expression>
            var getBlock = new NonTerminal("get_block");
            getBlock.Rule = 
                 grammar.Keywords.Get
                 + grammar.ToTerm(":")
                 + grammar.Eos
                 + grammar.MethodDeclarations.OptionalMethodBody;

            // set:
            //      <expression>
            var setBlock = new NonTerminal("set_block");
            setBlock.Rule =
                  grammar.Keywords.Set
                 + grammar.ToTerm(":")
                 + grammar.Eos
                 + grammar.MethodDeclarations.OptionalMethodBody;

            // The set block is optional.
            var setBlockOpt = new NonTerminal("set_block_opt");
            setBlockOpt.Rule = setBlock | grammar.Empty;

            /* public int foo:
             *      get:
             *          return i
             *      set:
             *          i = value
             */
            PropertyDeclaration.Rule = grammar.MethodDeclarations.MemberHeader
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.Indent
                + getBlock
                + setBlockOpt
                + grammar.Dedent;
        }
    }
}


using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules for a while loop statement.
    internal class WhileLoopRules
    {
        // A while loop with an expression on the same line.
        public NonTerminal SimpleWhileLoop { get; private set; }
        // A while loop with a body of expressions on the following line.
        public NonTerminal BodiedWhileLoop { get; private set; }

        PieGrammar grammar;

        public WhileLoopRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            SimpleWhileLoop = new NonTerminal("simple_while_loop");
            BodiedWhileLoop = new NonTerminal("bodied_while_loop");
        }

        public void Define()
        {
            // while <condition> <expression>
            SimpleWhileLoop.Rule = grammar.Keywords.While 
                + grammar.ValidConditionals 
                + grammar.expression;

            /* while <condition>:
             *      <expression>
             */
            BodiedWhileLoop.Rule = grammar.Keywords.While
                + grammar.ValidConditionals
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;
        }
    }
}

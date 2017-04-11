
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    internal class ForLoopRules
    {
        // A simple for loop (for int i in 10 dosomething())
        public NonTerminal SimpleForLoop { get; private set; }
        /* A for loop with a body:
        *   for int i in 10:
         *      Console.WriteLine(i)
         */
        public NonTerminal BodiedForLoop { get; private set; }

        PieGrammar grammar;

        public ForLoopRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            SimpleForLoop = new NonTerminal("simple_for_loop");
            BodiedForLoop = new NonTerminal("bodied_for_loop");
        }

        public void Define()
        {
            // The increment/decrement statement "step 1"
            var step = new NonTerminal("for_loop_step");
            step.Rule = grammar.Keywords.Step + grammar.expression;

            // The step is optional (defaults to step 1)
            var step_opt = new NonTerminal("for_loop_step_opt");
            step_opt.Rule = grammar.Empty | step;

            // The initialization statement
            var for_init = new NonTerminal("for_init");
            for_init.Rule = grammar.ExplicitVariableDeclaration | grammar.Identifier;

            // The range of values to iterate over
            var for_range = new NonTerminal("for_range");
            for_range.Rule = grammar.expression + grammar.Keywords.To + grammar.expression;

            // The conditional of the for loop is either some expression, or the range of values being looped over.
            var for_condition = new NonTerminal("for_condition");
            for_condition.Rule = grammar.expression | for_range;

            SimpleForLoop.Rule = grammar.Keywords.For
                + for_init
                + grammar.Keywords.In
                + for_condition
                + step_opt
                + grammar.expression;

            BodiedForLoop.Rule = grammar.Keywords.For
                + for_init
                + grammar.Keywords.In
                + for_condition
                + step_opt
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;
        }
    }
}

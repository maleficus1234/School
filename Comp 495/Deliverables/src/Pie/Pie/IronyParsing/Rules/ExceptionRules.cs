
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Exception handling and throwing grammatical rules.
    internal class ExceptionRules
    {
        PieGrammar grammar;

        // Exception handling grammatical rule
        public NonTerminal ExceptionHandlingBlock { get; private set; }
        // Exception throwing grammatical rule
        public NonTerminal ThrowStatement { get; private set; }

        public ExceptionRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            ExceptionHandlingBlock = new NonTerminal("exception_handling_block");
            ThrowStatement = new NonTerminal("throw_statement");
        }

        public void Define()
        {
            // throw <expression>
            ThrowStatement.Rule = grammar.Keywords.Throw
                + grammar.expression + (grammar.Eos | grammar.Empty);

            // try:
            var tryBlock = new NonTerminal("try_block");
            tryBlock.Rule =
                grammar.Keywords.Try
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;

            // catch Exception e:
            var catchBlock = new NonTerminal("catch_block");
            catchBlock.Rule =
                grammar.Keywords.Catch
                + grammar.Identifier
                + grammar.Identifier
                + grammar.ToTerm(":")
                +grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;

            // The catch blocks are optional
            var catchBlockOpt = new NonTerminal("catch_block_opt");
            catchBlockOpt.Rule = grammar.MakeStarRule(catchBlockOpt, catchBlock);

            // finally:
            var finallyBlock = new NonTerminal("finally_block");
            finallyBlock.Rule =
                grammar.Keywords.Finally
                + grammar.ToTerm(":")
                + grammar.Eos
                + grammar.MethodDeclarations.OptionalMethodBody;

            // The finally block is optional
            var finallyBlockOpt = new NonTerminal("finally_block_opt");
            finallyBlockOpt.Rule = finallyBlock | grammar.Empty;

            /*
             * try:
             * catch Exception ex:
             * finally:
             */
            ExceptionHandlingBlock.Rule =
                tryBlock
                + catchBlockOpt
                + finallyBlockOpt;
        }
    }
}

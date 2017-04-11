
namespace Pie.Expressions
{
    // Represents an if block
    public class IfBlock
        : Expression
    {
        // The conditional that deterines the truthfullness of the if block.
        public Expression Conditional { get; private set; }
        // The expressions to execute if the condition is true.
        public Expression TrueBlock { get; private set; }
        // the expressions to execute if the condition is false.
        public Expression FalseBlock { get; private set; }

        public IfBlock(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Conditional = new Expression(this, null);
            TrueBlock = new Expression(this, null);
            FalseBlock = new Expression(this, null);
        }
    }
}

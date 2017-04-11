
namespace Pie.Expressions
{
    // Represents a while loop (internally, just a for loop without the initializer and step).
    public class WhileLoop
        : Expression
    {
        // The condition to test to determine whether to continue looping.
        public Expression Condition { get; set; }

        public WhileLoop(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Condition = new Expression(this, null);
        }
    }
}

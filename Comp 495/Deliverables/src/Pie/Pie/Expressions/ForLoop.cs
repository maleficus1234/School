
namespace Pie.Expressions
{
    // Represents a for loop
    public class ForLoop
        : Expression
    {
        // The expression used to initialize the for loop
        public Expression Initialization { get; set; }
        // The conditional expresion used to terminate the for loop
        public Expression Condition { get; set; }
        // The expression that executes every loop to increase or
        // decrease the initialized value.
        public Expression Step { get; set; }

        public ForLoop(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Initialization = new Expression(this, null);
            Condition = new Expression(this, null);
            Step = new Expression(this, null);
        }
    }
}

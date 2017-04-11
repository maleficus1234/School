namespace Pie.Expressions
{
    // A binary operation expression: child 0 is the left operand, child 1 is the
    // right operand.
    public class BinaryOperator
        : Expression
    {
        // The type of operation: +, -, ==, !=, >> etc.
        public BinaryOperatorType OperatorType { get; set; }

        public BinaryOperator(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }
}

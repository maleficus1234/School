
namespace Pie.Expressions
{
    /* NOTE: unary operators are not supported by CodeDOM, or some reason?? */

    // The possible unary operators.
    public enum UnaryOperatorType
    {
        Not,
        Negate
    }

    // Represents a unary operator expression.
    public class UnaryOperator
        : Expression
    {
        public UnaryOperatorType OperatorType { get; set; }

        public UnaryOperator(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }
}

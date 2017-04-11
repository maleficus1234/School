
namespace Pie.Expressions
{
    // Represents a "throw" expression.
    public class Throw
        : Expression
    {
        public Throw(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }
}

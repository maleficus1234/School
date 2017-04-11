

namespace Pie.Expressions
{
    // A "return" expression
    public class Return
        : Expression
    {
        public Return(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }
}

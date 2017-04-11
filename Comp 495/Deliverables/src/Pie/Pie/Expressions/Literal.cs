using System;

namespace Pie.Expressions
{
    // Represents a literal expression: a number, a string, etc.
    public class Literal
        : Expression
    {
        // The literal itself
        public Object Value { get; set; }

        public Literal(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }

}

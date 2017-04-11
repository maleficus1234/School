using System.Collections.Generic;

namespace Pie.Expressions
{
    // Represents a class or struct constructor.
    class Constructor
                : Expression
    {
        // The list of parameters to pass to the constructor.
        public List<Expression> Parameters { get; private set; }
        // The accessibilty of the constructor: internal, public, protected, or private.
        public Accessibility Accessibility { get; set; }
        // The base or this constructor invoked before entering the body.
        public bool Sub { get; set; }
        // The parameters to pass to the sub constructor.
        public Expression SubParameters { get; private set; }

        public Constructor(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Parameters = new List<Expression>();
            SubParameters = new Expression(this, null);
            Accessibility = Expressions.Accessibility.Public;
            // Default to no sub constructor.
            Sub = false;
        }
    }
}

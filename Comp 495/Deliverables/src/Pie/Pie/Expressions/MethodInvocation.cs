
using System.Collections.Generic;

namespace Pie.Expressions
{
    // Represents a method invocation expression.
    public class MethodInvocation
        : Expression
    {
        // The variable owning this method, if any.
        public string Prefix { get; set; }
        // The name of the method to invocation.
        public string Name { get; set; }
        // The list of expressions to pass as parameters to the method invocation.
        public Expression Parameters { get; private set; }
        // The list of generic type names to use for this method invocation.
        public List<string> GenericTypes { get; private set; }

        public MethodInvocation(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Prefix = "";

            Parameters = new Expression(this, null);
            GenericTypes = new List<string>();
        }
    }
}

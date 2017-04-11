
using System.Collections.Generic;

namespace Pie.Expressions
{
    // Represents instantiation of a new type ("new foo()")
    public class Instantiation
        : Expression
    {
        // The name of the type being instantiated
        public string Name { get; set; }
        // The parameters to pass to the constructor of the type
        public Expression Parameters { get; private set; }
        // Is this an instantiation of an array?
        public bool IsArray { get; set; }
        // Generic type names for the instantiated type.
        public List<string> GenericTypes { get; set; }

        public Instantiation(Expression parentExpression, Token token)
            :base(parentExpression, token)
        {
            Parameters = new Expression(null, null);
            GenericTypes = new List<string>();
            IsArray = false;
        }
    }
}

using System.Collections.Generic;

namespace Pie.Expressions
{
    // An explicit variable declaration: variable's type name followed by it's identifier.
    public class ExplicitVariableDeclaration
        : Expression
    {
        public string TypeName { get; set; }
        public string Name { get; set; }
        // THe list of generic types to use in the generic declaration.
        public List<string> GenericTypes { get; private set; }

        public ExplicitVariableDeclaration(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            GenericTypes = new List<string>();
        }
    }
}

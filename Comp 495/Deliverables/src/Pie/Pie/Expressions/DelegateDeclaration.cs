using System.Collections.Generic;

namespace Pie.Expressions
{
    // Represents the declaration of a delegate
    public class DelegateDeclaration
        : Expression
    {
        // The type returned by the delegate.
        public string ReturnTypeName { get; set; }
        // The name of the delegate
        public string Name { get; set; }
        // Accessibility of the delegate declaration: public or internal
        public Accessibility Accessibility { get; set; }
        // The generic types used by the delegate.
        public List<string> GenericTypeNames { get; private set; }
        // The parameters that any function assigned to this delegate must match.
        public List<Expression> Parameters { get; private set; }
        // Shared = static
        public bool IsShared { get; set; }

        public DelegateDeclaration(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            ReturnTypeName = "";
            Name = "";
            Accessibility = Expressions.Accessibility.Public;
            GenericTypeNames = new List<string>();
            Parameters = new List<Expression>();
            IsShared = false;
        }
    }
}

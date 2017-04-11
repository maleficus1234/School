
using System.Collections.Generic;

namespace Pie.Expressions
{
    // Represents a method declaration expression.
    public class MethodDeclaration
        : Expression
    {
        // The return type of the method.
        public string ReturnTypeName { get; set; }
        // The name of the method.
        public string Name { get; set; }
        // The accessibility of the method declaration: public, protected, private, or internal
        public Accessibility Accessibility { get; set; }
        // shared = static
        public bool IsShared { get; set; }
        // overrides a virtual method
        public bool IsOverride { get; set; }
        // virtual method
        public bool IsVirtual { get; set; }
        // The list of generic type names for a generic method.
        public List<string> GenericTypeNames { get; private set; }
        // The list of method parameters.
        public List<Expression> Parameters { get; private set; }
        // final = sealed
        public bool IsFinal { get; set; }
        // An abstract method.
        public bool IsAbstract { get; set; }

        public MethodDeclaration(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            GenericTypeNames = new List<string>();
            Parameters = new List<Expression>();

            ReturnTypeName = null;
            Accessibility = Accessibility.Public;
            if(parentExpression is Class)
                if ((parentExpression as Class).IsModule)
                    IsShared = true;
                else
                    IsShared = false;
            IsOverride = false;
            IsAbstract = false;
            IsFinal = false;
            IsVirtual = false;
        }
    }
}

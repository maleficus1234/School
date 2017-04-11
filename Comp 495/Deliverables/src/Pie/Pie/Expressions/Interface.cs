
using System.Collections.Generic;

namespace Pie.Expressions
{
    public class Interface
        : Expression
    {
        // The unqualified name of the interface: it's name without the namespace.
        public string UnqualifiedName { get; set; }

        // Public or internal
        public Accessibility Accessibility { get; set; }

        // Base type names of the interface
        public List<string> BaseTypeNames { get; private set; }

        // Generic type names of the interface.
        public List<string> GenericTypeNames { get; private set; }

        public Interface(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            UnqualifiedName = "";
            Accessibility = Accessibility.Public;
            BaseTypeNames = new List<string>();
            GenericTypeNames = new List<string>();
        }

        // Return the fully qualified name: the name of then namespace plus the interface name.
        public string GetQualifiedName()
        {
            // If there is no parent namespace, this interface is in the global namespace.
            // Return it's unqualified name as the fully qualified name.
            if (ParentExpression == null)
            {
                return UnqualifiedName;
            }

            // If the parent is a namespace, we need to get it's qualifed name.
            if (ParentExpression is Namespace)
            {
                return ((Namespace)ParentExpression).Name + "." + UnqualifiedName;
            }
            else
                return UnqualifiedName; // An interface could be nested in another class: need to account for this when it's implemented.
        }


    }
}

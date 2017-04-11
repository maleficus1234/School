
using System.Collections.Generic;

namespace Pie.Expressions
{
    // Represents an enum: a blocked list of constants.
    public class Enum
        : Expression
    {
        // Returns the name of the enum sans namespace.
        public string UnqualifiedName { get; set; }
        // Can be public or internal accessibility.
        public Accessibility Accessibility { get; set; }
        // The list of constants.
        public List<EnumConstant> Constants { get; private set; }

        public Enum(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Constants = new List<EnumConstant>();
            UnqualifiedName = "";
            Accessibility = Accessibility.Public;
        }

        // Return the fully qualified name: the name of then namespace plus the class name.
        public string GetQualifiedName()
        {
            // If there is no parent namespace, this class is in the global namespace.
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
                return UnqualifiedName; // A class could be nested in another class: need to account for this when it's implemented.
        }
    }
}

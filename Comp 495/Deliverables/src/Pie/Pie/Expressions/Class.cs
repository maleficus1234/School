using System.Collections.Generic;

namespace Pie.Expressions
{
    /* Represents classes, modules, and structs. Ideally, it should have a generic name like "type",
     * but we use System.Type enough that it would get confusing and annoying */
    public class Class
        : Expression
    {
        // The unqualified name of the class: it's name without the namespace.
        public string UnqualifiedName { get; set; }

        // "sealed" in c#
        public bool IsFinal { get; set; }

        // Public or internal
        public Accessibility Accessibility { get; set; }

        // This class is a module: similar to a static class in c#. Is partial and sealed.
        public bool IsModule { get; set; }

        // This is a struct: value type, and unable to use inheritance
        public bool IsStruct { get; set; }

        // Classes declared as partial can be distributed across multiple files.
        public bool IsPartial { get; set; }

        // Abstract type can not be instantiated: one must create a subclass
        public bool IsAbstract { get; set; }

        // Base type names of the class
        public List<string> BaseTypeNames { get; private set; }

        // Generic type names of the class.
        public List<string> GenericTypeNames { get; private set; }

        public Class(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            IsFinal = false;
            UnqualifiedName = "";
            Accessibility = Accessibility.Public;
            IsModule = false;
            IsStruct = false;
            IsPartial = false;
            IsAbstract = false;
            BaseTypeNames = new List<string>();
            GenericTypeNames = new List<string>();
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

        public string GetNamespace()
        {
            if (ParentExpression is Namespace)
                return (ParentExpression as Namespace).GetFullName();
            return "";
        }
    }
}

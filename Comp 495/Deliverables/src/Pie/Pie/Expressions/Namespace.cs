
namespace Pie.Expressions
{
    // Represents a namespace expression.
    public class Namespace
        : Expression
    {
        // The namespace name
        public string Name { get; set; }

        public Namespace(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Name = "";
        }

        // Return the full name of the namespace (ie, parent namespaces plus this one).
        public string GetFullName()
        {
            if (ParentExpression is Namespace)
                return (ParentExpression as Namespace).GetFullName() + "." + Name;
            else
                return Name;
        }
    }
}

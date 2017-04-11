

namespace Pie.Expressions
{
    // Represents a property declaration expression.
    public class Property
        : Expression
    {
        // The name of the property.
        public string Name { get; set; }
        // The return type of the property.
        public string TypeName { get; set; }
        // The accessibility of the property: public, protected, private, internal.
        public Accessibility Accessibility { get; set; }
        // The expressions found in the get block.
        public Expression GetBlock { get; private set; }
        // The expressions found in the set block.
        public Expression SetBlock { get; private set; }
        // shared = static
        public bool IsShared { get; set; }
        // Overrides a virtual function.
        public bool IsOverride { get; set; }
        // virtual function.
        public bool IsVirtual { get; set; }
        // final = sealed
        public bool IsFinal { get; set; }
        // Is an abstract property.
        public bool IsAbstract { get; set; }

        public Property(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Accessibility = Expressions.Accessibility.Public;
            GetBlock = new Expression(this, null);
            SetBlock = new Expression(this, null);

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

    // Represents the "value" expression used by the setter in a property.
    public class Value
        :Expression
    {
        public Value(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }
}

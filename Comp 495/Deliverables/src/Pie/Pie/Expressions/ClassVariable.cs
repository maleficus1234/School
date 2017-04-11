namespace Pie.Expressions
{
    // Represents a class variable: essentially an explicit variable declaration with modifiers.
    public class ClassVariable
        : Expression
    {
        // The type of the variable
        public string TypeName { get; set; }

        // The name of the variable
        public string Name { get; set; }

        // Accessibility fo the variable: public, protected, private, internal
        public Accessibility Accessibility { get; set; }
        // final = const
        public bool IsFinal { get; set; }
        // shared = static
        public bool IsShared { get; set; }

        public ClassVariable(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            IsFinal = false;
            if ((parentExpression as Class).IsModule)
                IsShared = true;
            else
                IsShared = false;

            // Have class variables default to private.
            Accessibility = Expressions.Accessibility.Private;
        }
    }
}

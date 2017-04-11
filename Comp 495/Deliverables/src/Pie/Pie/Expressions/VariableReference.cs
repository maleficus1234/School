
namespace Pie.Expressions
{
    // Represents a variable reference expression: the "v" in "v = 0"
    public class VariableReference
        : Expression
    {
        public string Name { get; set; }

        public VariableReference(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    } 
}

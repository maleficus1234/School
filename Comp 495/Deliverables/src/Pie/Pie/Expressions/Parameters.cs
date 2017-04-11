
namespace Pie.Expressions
{
    // Describes an argument in the argument list of a method declaration.
    // This will be needed to be adjusted to be able to handle generic types
    public class SimpleParameter
        : Expression
    {
        public string TypeName { get; set; }
        public string Name { get; set; }

        public SimpleParameter(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }

    // Describes an argument in the argument list of a method declaration.
    // This will be needed to be adjusted to be able to handle generic types
    public class DirectionedParameter
    : Expression
    {
        public string TypeName { get; set; }
        public string Name { get; set; }
        public ParameterDirection Direction {get; set;}

        public DirectionedParameter(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            
        }
    }
}

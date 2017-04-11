
namespace Pie.Expressions
{
    // Names the direction of a parameter:
    public enum ParameterDirection
    {
        Out, // out = an uninitialized variable can be passed by reference
        Ref  // ref = an initialized variable can be passed by reference.
    }

    // Describes a parameter that is directioned (ex "out int i")
    public class DirectionedArgument
        : Expression
    {
        public string Name { get; set; }
        public ParameterDirection Direction { get; set; }

        public DirectionedArgument(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Name = "";
            Direction = ParameterDirection.Ref;
        }
    }
}

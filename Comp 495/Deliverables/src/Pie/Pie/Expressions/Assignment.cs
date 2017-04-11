namespace Pie.Expressions
{
    // An assignment expression: child 0 is the expression being assigned to, 
    // child 1 is the expression being assigned.
    public class Assignment
        : Expression
    {
        // The type of assignment: =, +=, -=, *=, /=, |=, &=
        public AssignmentType AssignmentType { get; set; }

        public Assignment(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            // Default to direct assignment.
            AssignmentType = Expressions.AssignmentType.Equal;
        }
    }
}


using System.Collections.Generic;

namespace Pie.Expressions
{
    // Describes a catch clause of an exception handling block
    public class CatchClause
        : Expression
    {
        // The exception type caught by this clause
        public string Type { get; set; }
        // The name of the exception variable caught.
        public string Name { get; set; }

        public CatchClause(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {

        }
    }

    // Describes a try/catch/finally block
    public class ExceptionHandler
        : Expression
    {
        // Parent of the list of statements in the try block
        public Expression Try { get; private set; }
        // Parent of the list of statements in the finally block
        public Expression Finally { get; private set; }
        // List of the catch clauses
        public List<CatchClause> CatchClauses { get; private set; }

        public ExceptionHandler(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Try = new Expression(this, null);
            CatchClauses = new List<CatchClause>();
            Finally = new Expression(this, null);
        }
    }
}

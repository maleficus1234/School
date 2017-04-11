
namespace Pie.Expressions
{
    // The switch block expression
    public class SwitchBlock
        : Expression
    {
        // The expression being tested by the switch.
        public Expression Variable { get; private set; }
        public SwitchBlock(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Variable = new Expression(this, null);
        }
    }

    // The case block expression.
    public class CaseBlock
    : Expression
    {
        // The expression being tested by the case.
        public Expression Variable { get; private set; }
        public CaseBlock(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Variable = new Expression(this, null);
        }
    }
}


using System.Collections.Generic;

namespace Pie.Expressions
{
    /* The base class for all expressions in the parse tree.
     * It also defines functions needed to walk through the tree during validation. */
    public class Expression
    {
        // The parent expression that this one is a child of.
        public Expression ParentExpression { get; set; }
        // The list of child expressions of this child.
        public List<Expression> ChildExpressions { get; set; }
        // The token from the parse tree that was used to create this expression
        public Token Token { get; set; }
        // Has this expression been validated by the validator?
        public bool Validated { get; set; }
        // An expression can have a list of imports: the imports for the source file 
        // in which it is found. The validator distributes the imports to each expression
        // below that import for convenience.
        public List<Import> Imports { get; private set; }

        public Expression(Expression parentExpression, Token token)
        {
            ChildExpressions = new List<Expression>();
            ParentExpression = parentExpression;
            Token = token;
            Imports = new List<Import>();
            Validated = false;
        }
    }
}

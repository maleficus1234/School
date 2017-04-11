
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace Pie.Expressions
{
    // The root of the expression tree.
    public class Root
        : Expression
    {
        public List<CompilerError> CompilerErrors { get; private set; }

        public Root()
            : base(null, null)
        {
            CompilerErrors = new List<CompilerError>();
        }
    }
}


using System.CodeDom;
using System.CodeDom.Compiler;
using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class LiteralEmitter
    {
        // Emit a codedom literal expression: int, long, bool, string, etc.
        public static CodeExpression Emit(Literal literal)
        {
            return new CodePrimitiveExpression(literal.Value);
        }
    }
}

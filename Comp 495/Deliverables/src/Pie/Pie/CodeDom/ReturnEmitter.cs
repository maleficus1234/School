using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class ReturnEmitter
    {
        // Generate a codedom return statement.
        public static CodeStatement Emit(Return r)
        {
            var codeMethodReturnStatement = new CodeMethodReturnStatement();

            // Attach the expression to return, if any.
            if(r.ChildExpressions.Count > 0)
                codeMethodReturnStatement.Expression = CodeDomEmitter.EmitCodeExpression(r.ChildExpressions[0]);

            return codeMethodReturnStatement;
        }
    }
}

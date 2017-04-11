using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class ExceptionHandlerEmitter
    {
        // Generate a codedom exception handler statement
        public static CodeStatement Emit(ExceptionHandler ex)
        {
            // Create the codedom exception handler statement
            var codeTry = new CodeTryCatchFinallyStatement();

            // Add the statements in the try block
            foreach (var e in ex.Try.ChildExpressions)
                codeTry.TryStatements.Add(CodeDomEmitter.EmitCodeStatement(e));

            // Add all the catch clauses.
            foreach (var c in ex.CatchClauses)
            {
                // Create the codedom catch statement.
                var catchClause = new CodeCatchClause();
                // To do: replace with non-test code
                catchClause.CatchExceptionType = new CodeTypeReference("System.Exception");
                catchClause.LocalName = "ex";
                codeTry.CatchClauses.Add(catchClause);

                // Add the statemnts in the catch block
                foreach(var e in c.ChildExpressions)
                    catchClause.Statements.Add(CodeDomEmitter.EmitCodeStatement(e));
            }

            // Add the statements in the finally block.
            foreach (var e in ex.Finally.ChildExpressions)
                codeTry.FinallyStatements.Add(CodeDomEmitter.EmitCodeStatement(e));

            return codeTry;
        }

        // Generate a codedom throw statement
        public static CodeStatement Emit(Throw th)
        {
            var codeThrow = new CodeThrowExceptionStatement();

            codeThrow.ToThrow = CodeDomEmitter.EmitCodeExpression(th.ChildExpressions[0]);

            return codeThrow;
        }
    }
}

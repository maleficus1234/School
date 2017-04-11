using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class IfBlockEmitter
    {
        // Emits the codedom statement for an if conditional block.
        public static CodeStatement Emit(IfBlock ifBlock)
        {
            // Create the codedom if statement.
            var i = new CodeConditionStatement();

            // Emit the conditional statements for the if block.
            i.Condition = CodeDomEmitter.EmitCodeExpression(ifBlock.Conditional.ChildExpressions[0]);

            // Emit the lists of statements for the true and false bodies of the if block.

            // Comments need to be added in case the bodies are empty: these two properties
            // of the CodeConditionStatement can't be null.
            i.FalseStatements.Add(new CodeCommentStatement("If condition is false, execute these statements."));
            i.TrueStatements.Add(new CodeCommentStatement("If condition is true, execute these statements."));

            // Emit the statements for the true block
            foreach (var e in ifBlock.TrueBlock.ChildExpressions)
                i.TrueStatements.Add(CodeDomEmitter.EmitCodeStatement(e));

            // Emit the statements for the false block.
            foreach (var e in ifBlock.FalseBlock.ChildExpressions)
                i.FalseStatements.Add(CodeDomEmitter.EmitCodeStatement(e));

            return i;
        }
    }
}


using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class SwitchEmitter
    {
        // Generate a series of codedom if statements representing a switch block.
        // (Codedom doesn't support switch)
        public static CodeStatement Emit(SwitchBlock switchBlock)
        {
            // Create the expression for whatever we're switching on.
            var codeVar = CodeDomEmitter.EmitCodeExpression(switchBlock.Variable.ChildExpressions[0]);

            // Store the first and previous if statements here, since it's needed on future loops.
            CodeConditionStatement codeIf = null;
            CodeConditionStatement lastIf = null;
            foreach(var e in switchBlock.ChildExpressions)
            {
                // Get the value being compared to: the right operand.
                var right = CodeDomEmitter.EmitCodeExpression((e as CaseBlock).Variable.ChildExpressions[0]);

                // Create the next if statement.
                var nestedIf = new CodeConditionStatement();

                // Each case block will compare codeVar (left) to the right operand.
                nestedIf.Condition = new CodeBinaryOperatorExpression(codeVar, CodeBinaryOperatorType.ValueEquality, right);
                nestedIf.TrueStatements.Add(new CodeCommentStatement("If condition is true, execute these statements."));
                foreach (var s in e.ChildExpressions)
                    nestedIf.TrueStatements.Add(CodeDomEmitter.EmitCodeStatement(s));
                nestedIf.FalseStatements.Add(new CodeCommentStatement("If condition is false, execute these statements."));
                // if codeIf is null, this is the first if statement.
                if (codeIf == null)
                {
                    codeIf = nestedIf;
                }
                else // It's not the first if statement, so attach it to the previous one.
                {
                    lastIf.FalseStatements.Add(nestedIf);                   
                }


                // Store the last if block for the next iteration.
                lastIf = nestedIf;
            }

            return codeIf;
        }
    }
}

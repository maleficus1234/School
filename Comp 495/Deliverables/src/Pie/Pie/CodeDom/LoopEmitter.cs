using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class LoopEmitter
    {
        // Emit a codedome expression representing a while loop.
        public static CodeStatement Emit(WhileLoop loop)
        {
            // A while loop is a for loop with no initializer or incrementor, only a condition.
            var i = new CodeIterationStatement(new CodeSnippetStatement(""),
                CodeDomEmitter.EmitCodeExpression(loop.Condition.ChildExpressions[0]),
                new CodeSnippetStatement(""));

            // Emit the statements found in the body of the while loop.
            foreach (var e in loop.ChildExpressions)
                i.Statements.Add(CodeDomEmitter.EmitCodeStatement(e));

            return i;
        }

        // Emit a codedome expression representing a for loop.
        public static CodeStatement Emit(ForLoop loop)
        {
            // The expression describing how the for loop starts.
            CodeExpression startEx = null;
            // The expression describing how the for loop ends.
            CodeExpression endEx = null;

            // The for loop has both a start and end defined. For example: for int i in 0 to 10
            if (loop.Condition.ChildExpressions.Count > 1)
            {
                startEx = CodeDomEmitter.EmitCodeExpression(loop.Condition.ChildExpressions[0]);
                endEx = CodeDomEmitter.EmitCodeExpression(loop.Condition.ChildExpressions[1]);
            }
            else // The for loop has only the end defined. For examle: for int i in 10
            {
                startEx = new CodePrimitiveExpression(0);
                endEx = CodeDomEmitter.EmitCodeExpression(loop.Condition.ChildExpressions[0]);
            }
            
            // The statement that initializes the for loop (int i, i, etc).
            CodeStatement varInit = null;
            // The variable reference from the initialization: we need this when the initialization is "int i".
            // The variable reference is "i".
            CodeExpression varRef = null;

            // An intializer that is an explicit variable declaration must be handled differently.
            if(loop.Initialization.ChildExpressions[0] is ExplicitVariableDeclaration)
            {
                // Get the explicit variable declaration
                var explicitVariable = loop.Initialization.ChildExpressions[0] as ExplicitVariableDeclaration;
                varInit = new CodeVariableDeclarationStatement(explicitVariable.TypeName, explicitVariable.Name, // The declaration
                    startEx);                                                                   // The assignment
                varRef = new CodeVariableReferenceExpression((loop.Initialization.ChildExpressions[0] as ExplicitVariableDeclaration).Name);
            }
            else // It's a variable reference
            {
                // Get the variable reference.
                var variable = loop.Initialization.ChildExpressions[0] as VariableReference;
                varRef = new CodeVariableReferenceExpression((loop.Initialization.ChildExpressions[0] as VariableReference).Name);
                varInit = new CodeAssignStatement(varRef, // Left operand
                    startEx);
            }

            // Pie's for loop is limited to less than or equal conditional.
            var op = new CodeBinaryOperatorExpression(varRef,
                CodeBinaryOperatorType.LessThanOrEqual,
                endEx);

            // The step: the amount incremented or decremented.
            CodeStatement stepSt = null;
            if(loop.Step.ChildExpressions[0] is Literal)
            {
                var literal = loop.Step.ChildExpressions[0] as Literal;
                stepSt = new CodeAssignStatement(varRef, new CodeBinaryOperatorExpression( 
                    varRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(literal.Value) ));
            }
            var i = new CodeIterationStatement(varInit,
                op,
                stepSt);

            // Emit the statements found in the body of the while loop.
            foreach (var e in loop.ChildExpressions)
                i.Statements.Add(CodeDomEmitter.EmitCodeStatement(e));

            return i;
        }
    }
}

using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class AssignmentEmitter
    {
        /* Returns a CodeDOM statement representing an Assignment expression. */
        public static CodeStatement Emit(Assignment assignment)
        {
            // The expression that is being assigned.
            var assignedExpression = CodeDomEmitter.EmitCodeExpression(assignment.ChildExpressions[1]);

            // Translate the assignment type to a CodeDOM one.
            var codeOperator = CodeBinaryOperatorType.Add;
            switch(assignment.AssignmentType)
            {
                case AssignmentType.Add:
                    codeOperator = CodeBinaryOperatorType.Add;
                    break;
                case AssignmentType.And:
                    codeOperator = CodeBinaryOperatorType.BitwiseAnd;
                    break;
                case AssignmentType.Or:
                    codeOperator = CodeBinaryOperatorType.BitwiseOr;
                    break;
                case AssignmentType.Divide:
                    codeOperator = CodeBinaryOperatorType.Divide;
                    break;
                case AssignmentType.Multiply:
                    codeOperator = CodeBinaryOperatorType.Multiply;
                    break;
                case AssignmentType.Subtract:
                    codeOperator = CodeBinaryOperatorType.Subtract;
                    break;
            }

            /* Variables that are immediately assigned a value upon explicit declaration such as:
             *      int i = 10
             * require a different CodeDOM statement than the normal assignment statement. */
            if (assignment.ChildExpressions[0] is ExplicitVariableDeclaration)
            {
                var variable = assignment.ChildExpressions[0] as ExplicitVariableDeclaration;
                var codeType = new CodeTypeReference();
                codeType.BaseType = variable.TypeName;
                foreach (String e in variable.GenericTypes)
                    codeType.TypeArguments.Add(new CodeTypeReference(e));
                if (assignment.AssignmentType == AssignmentType.Equal)
                {
                    return new CodeVariableDeclarationStatement(codeType, variable.Name, // The declaration
                        assignedExpression);       // The assignment
                }
                else
                {
                    // If we got this far, the validator missed that int i += x is not valid.
                    // Since this should have been caught prior to reaching the generator, this is a failure state.
                    throw new NotImplementedException();
                }
            }
            else // Normal assignment (ex i = 1)
            {
                var left = CodeDomEmitter.EmitCodeExpression(assignment.ChildExpressions[0]);
                if (assignment.AssignmentType == AssignmentType.Equal)
                {
                    // A simple assignment: left = assignedExpression
                    return new CodeAssignStatement(left, // Left operand
                        assignedExpression);             // Right operand
                }
                else
                {
                    // Some other type of assignment, such as left += assignedExpression
                    // We need to create the appropriate codedom binary expression first.
                    var addOp = new CodeBinaryOperatorExpression();
                    addOp.Left = left;
                    addOp.Right = assignedExpression;
                    addOp.Operator = codeOperator;
                    return new CodeAssignStatement(left, addOp);
                }
            }
        }
    }
}

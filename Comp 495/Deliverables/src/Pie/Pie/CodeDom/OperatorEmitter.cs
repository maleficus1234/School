using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class OperatorEmitter
    {
        // Emit a codedom binary operation expression
        public static CodeExpression Emit(BinaryOperator op)
        {
            // Translate the operator type to a codedom one.
            CodeBinaryOperatorType opType = CodeBinaryOperatorType.Add;
            switch (op.OperatorType)
            {
                case BinaryOperatorType.Subtract:
                    opType = CodeBinaryOperatorType.Subtract;
                    break;
                case BinaryOperatorType.Divide:
                    opType = CodeBinaryOperatorType.Divide;
                    break;
                case BinaryOperatorType.Multiply:
                    opType = CodeBinaryOperatorType.Multiply;
                    break;
                case BinaryOperatorType.Less:
                    opType = CodeBinaryOperatorType.LessThan;
                    break;
                case BinaryOperatorType.LessOrEqual:
                    opType = CodeBinaryOperatorType.LessThanOrEqual;
                    break;
                case BinaryOperatorType.Equal:
                    opType = CodeBinaryOperatorType.IdentityEquality;
                    break;
                case BinaryOperatorType.NotEqual:
                    opType = CodeBinaryOperatorType.IdentityInequality;
                    break;
                case BinaryOperatorType.Greater:
                    opType = CodeBinaryOperatorType.GreaterThan;
                    break;
                case BinaryOperatorType.GreaterOrEqual:
                    opType = CodeBinaryOperatorType.GreaterThanOrEqual;
                    break;
                case BinaryOperatorType.BitwiseAnd:
                    opType = CodeBinaryOperatorType.BitwiseAnd;
                    break;
                case BinaryOperatorType.BitwiseOr:
                    opType = CodeBinaryOperatorType.BitwiseOr;
                    break;
                case BinaryOperatorType.Modulo:
                    opType = CodeBinaryOperatorType.Modulus;
                    break;
                case BinaryOperatorType.LogicalAnd:
                    opType = CodeBinaryOperatorType.BooleanAnd;
                    break;
                case BinaryOperatorType.LogicalOr:
                    opType = CodeBinaryOperatorType.BooleanOr;
                    break;
                case BinaryOperatorType.As: // Casting requires a different set of codedom expressions.
                    var cast = new CodeCastExpression();
                    cast.TargetType = new CodeTypeReference((op.ChildExpressions[1] as VariableReference).Name);
                    cast.Expression = CodeDomEmitter.EmitCodeExpression(op.ChildExpressions[0]);
                    return cast;

            }

            var o = new CodeBinaryOperatorExpression(CodeDomEmitter.EmitCodeExpression(op.ChildExpressions[0]), // left operand
                opType, // operator
                CodeDomEmitter.EmitCodeExpression(op.ChildExpressions[1])); // right operand
            
            return o;
        }

        // CodeDOM doesn't support unary operators??
        public static CodeExpression Emit(UnaryOperator op)
        {
           // var opType = new Code
            return null;
        }
    }
}

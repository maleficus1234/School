using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class MethodEmitter
    {
        // Generate a codedom method declaration and attach it to the given codedom type.
        public static void Emit(CodeTypeDeclaration codeTypeDeclaration, MethodDeclaration methodDeclaration)
        {
            // Create the codedom member method and attach it to the codedom type.
            var codeMemberMethod = new CodeMemberMethod();
            codeTypeDeclaration.Members.Add(codeMemberMethod);

            // Assign the method name
            codeMemberMethod.Name = methodDeclaration.Name;

            // Assign the return type: make sure to check for null.
            if (methodDeclaration.ReturnTypeName == "void")
                codeMemberMethod.ReturnType = null;
            else
                codeMemberMethod.ReturnType = new CodeTypeReference(methodDeclaration.ReturnTypeName);

            // Translate the method parameters.
            foreach (Expression p in methodDeclaration.Parameters)
            {
                if(p is SimpleParameter) // For example "int i"             
                    codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression((p as SimpleParameter).TypeName, (p as SimpleParameter).Name));
                if(p is DirectionedParameter) // For example "ref int i"
                {
                    var codeParameter = new CodeParameterDeclarationExpression((p as DirectionedParameter).TypeName, (p as DirectionedParameter).Name);
                    switch((p as DirectionedParameter).Direction)
                    {
                        case ParameterDirection.Out:
                            codeParameter.Direction = FieldDirection.Out;
                            break;
                        case ParameterDirection.Ref:
                            codeParameter.Direction = FieldDirection.Ref;
                            break;
                    }
                    codeMemberMethod.Parameters.Add(codeParameter);
                }
            }

            // Translate the method's accessibility
            MemberAttributes memberAttributes = MemberAttributes.Public;
            switch (methodDeclaration.Accessibility)
            {
                case Accessibility.Internal:
                    memberAttributes = MemberAttributes.FamilyAndAssembly;
                    break;
                case Accessibility.Private:
                    memberAttributes = MemberAttributes.Private;
                    break;
                case Accessibility.Protected:
                    memberAttributes = MemberAttributes.Family;
                    break;
                case Accessibility.Public:
                    memberAttributes = MemberAttributes.Public;
                    break;
            }

            // Shared = static
            if (methodDeclaration.IsShared) memberAttributes |= MemberAttributes.Static;
            if (methodDeclaration.IsAbstract) memberAttributes |= MemberAttributes.Abstract;
            if(!methodDeclaration.IsVirtual && !methodDeclaration.IsAbstract)
                if (methodDeclaration.IsOverride)
                    memberAttributes |= MemberAttributes.Override;
                else
                    memberAttributes |= MemberAttributes.Final;
            
            codeMemberMethod.Attributes = memberAttributes;

            // Add the statements found in the method body.
            foreach (var e in methodDeclaration.ChildExpressions)
                codeMemberMethod.Statements.Add(CodeDomEmitter.EmitCodeStatement(e));
        }

        // Generate the codedom expression for a method invocation.
        public static CodeExpression Emit(MethodInvocation methodInvocation)
        {
            CodeTypeReferenceExpression t = null;

            // Check if it's a method owned by a variable.
            if (methodInvocation.Prefix != "")
                t = new CodeTypeReferenceExpression(methodInvocation.Prefix);

            // Create the codedom method invcation.
            var mi = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(t, methodInvocation.Name));

            // Add the parameters of the method invocation.
            foreach (var a in methodInvocation.Parameters.ChildExpressions)
                mi.Parameters.Add(CodeDomEmitter.EmitCodeExpression(a));
           
            return mi;
        }

        // Emit a codedom directioned variable expression (ref int foo or out int foo)
        public static CodeExpression Emit(DirectionedArgument p)
        {
            var dir = FieldDirection.Out;
            if (p.Direction == ParameterDirection.Ref)
                dir = FieldDirection.Ref;
            var codeDirection = new CodeDirectionExpression(dir,
                new CodeVariableReferenceExpression(p.Name));
            return codeDirection;
        }
    }
}

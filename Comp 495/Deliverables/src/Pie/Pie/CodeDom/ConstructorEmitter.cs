using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class ConstructorEmitter
    {
        // Generates a codedom constructor expression and attaches it to the given type.
        public static void Emit(CodeTypeDeclaration codeTypeDeclaration, Constructor ctor)
        {
            // Create the codedom constructor
            var codeCtor = new CodeConstructor();
            codeTypeDeclaration.Members.Add(codeCtor);

            // Translate accessibility of the constructor
            MemberAttributes memberAttributes = MemberAttributes.Public;
            switch (ctor.Accessibility)
            {
                case Accessibility.Internal:
                    memberAttributes |= MemberAttributes.FamilyAndAssembly;
                    break;
                case Accessibility.Private:
                    memberAttributes |= MemberAttributes.Private;
                    break;
                case Accessibility.Protected:
                    memberAttributes |= MemberAttributes.Family;
                    break;
                case Accessibility.Public:
                    memberAttributes |= MemberAttributes.Public;
                    break;
            }
            codeCtor.Attributes = memberAttributes;

            // Translate the parameters of the constructor
            foreach (Expression p in ctor.Parameters)
            {
                if (p is SimpleParameter) // ex "int i"
                    codeCtor.Parameters.Add(new CodeParameterDeclarationExpression((p as SimpleParameter).TypeName, (p as SimpleParameter).Name));
                if (p is DirectionedParameter) // ex "ref int i"
                {
                    var codeParameter = new CodeParameterDeclarationExpression((p as DirectionedParameter).TypeName, (p as DirectionedParameter).Name);
                    switch ((p as DirectionedParameter).Direction)
                    {
                        case ParameterDirection.Out:
                            codeParameter.Direction = FieldDirection.Out;
                            break;
                        case ParameterDirection.Ref:
                            codeParameter.Direction = FieldDirection.Ref;
                            break;
                    }
                    codeCtor.Parameters.Add(codeParameter);
                }
            }

            // Add call to a constructor of the base class or another in the same class.
            foreach (var a in ctor.SubParameters.ChildExpressions)
            {
                if (ctor.Sub)
                    codeCtor.ChainedConstructorArgs.Add(CodeDomEmitter.EmitCodeExpression(a));
                else
                    codeCtor.BaseConstructorArgs.Add(CodeDomEmitter.EmitCodeExpression(a));
            }

            // Add all the statements in the body of the constructor
            foreach (var e in ctor.ChildExpressions)
                codeCtor.Statements.Add(CodeDomEmitter.EmitCodeStatement(e));
        }
    }
}

using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class DelegateEmitter
    {
        // Builds a codedom delegate expression and attaches it to the given codedom namespace.
        public static void Emit(CodeNamespace codeNamespace, DelegateDeclaration del)
        {
            // Create the codedom delegate and attach it to the namespace.
            var codeDelegate = new CodeTypeDelegate();
            codeNamespace.Types.Add(codeDelegate);

            // Assign the name of the delegate
            codeDelegate.Name = del.Name;

            // Set the type of the delegate: make sure to check for null
            if (del.ReturnTypeName == "void")
                codeDelegate.ReturnType = null;
            else
                codeDelegate.ReturnType = new CodeTypeReference(del.ReturnTypeName);

            // Translate the accessibililty of the delegate
            MemberAttributes attributes = MemberAttributes.Public;
            switch(del.Accessibility)
            {
                case Accessibility.Public:
                    attributes = MemberAttributes.Public;
                    break;
                case Accessibility.Protected:
                    attributes = MemberAttributes.Family;
                    break;
                case Accessibility.Private:
                    attributes = MemberAttributes.Private;
                    break;
                case Accessibility.Internal:
                    attributes = MemberAttributes.FamilyAndAssembly;
                    break;
            }

            // Shared = static
            if (del.IsShared)
                attributes |= MemberAttributes.Static;

            codeDelegate.Attributes = attributes;
            
            // Translate the parameters of the delegate.
            foreach (Expression p in del.Parameters)
            {
                if (p is SimpleParameter) // ex "int i"
                    codeDelegate.Parameters.Add(new CodeParameterDeclarationExpression((p as SimpleParameter).TypeName, (p as SimpleParameter).Name));
                if (p is DirectionedParameter) // ex "ref int t"
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
                    codeDelegate.Parameters.Add(codeParameter);
                }
            }
        }
    }
}

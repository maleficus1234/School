using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class ClassVariableEmitter
    {
        // Creates the codedom expression for a class variable, and attaches it to the given type.
        public static void Emit(CodeTypeDeclaration codeTypeDeclaration, ClassVariable classVariable)
        {
            var codeField = new CodeMemberField();
            codeTypeDeclaration.Members.Add(codeField);
            codeField.Name = classVariable.Name;
            codeField.Type = new CodeTypeReference(classVariable.TypeName);

            // Translate it's accessibility
            var attr = MemberAttributes.Public;
            switch(classVariable.Accessibility)
            {
                case Accessibility.Internal:
                    attr = MemberAttributes.FamilyAndAssembly;
                    break;
                case Accessibility.Private:
                    attr = MemberAttributes.Private;
                    break;
                case Accessibility.Protected:
                    attr = MemberAttributes.Family;
                    break;
            }

            // shared = static
            if (classVariable.IsShared)
                attr |= MemberAttributes.Static;

            // Final = const
            if (classVariable.IsFinal)
                attr |= MemberAttributes.Const;

            codeField.Attributes = attr;

        }
    }
}

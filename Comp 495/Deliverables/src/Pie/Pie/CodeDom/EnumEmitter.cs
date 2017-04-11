using System;
using System.CodeDom;
using System.Reflection;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class EnumEmitter
    {
        // Generates a codedom enumeration and attaches it to the given namespace.
        public static void Emit(CodeNamespace codeNamespace, Pie.Expressions.Enum e)
        {
            // CodeTypeDeclaration is the CodeDOM representation of a
            // class, struct, or enum.
            var codeTypeDeclaration = new CodeTypeDeclaration();
            codeNamespace.Types.Add(codeTypeDeclaration);

            // Assign the unqualified name (without namespace).
            codeTypeDeclaration.Name = e.UnqualifiedName;

            // Flag the type as an enum.
            codeTypeDeclaration.IsEnum = true;

            // Set the accessibility of the enum.
            switch (e.Accessibility)
            {
                case Accessibility.Internal:
                    codeTypeDeclaration.TypeAttributes = TypeAttributes.NestedAssembly;
                    break;
                case Accessibility.Public:
                    codeTypeDeclaration.TypeAttributes = TypeAttributes.Public;
                    break;
            }

            // Translate the list of constants in the enum
            foreach(var c in e.Constants)
            {
                var f = new CodeMemberField(e.UnqualifiedName, c.Name);
                f.InitExpression = new CodePrimitiveExpression(c.Value);
                codeTypeDeclaration.Members.Add(f);
            }
        }
    }
}

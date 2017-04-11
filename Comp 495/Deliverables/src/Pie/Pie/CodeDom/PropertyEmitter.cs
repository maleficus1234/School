using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class PropertyEmitter
    {
        // Generate a codedom property expression and attach it to the codedom type.
        public static void Emit(CodeTypeDeclaration codeTypeDeclaration, Property property)
        {
            // Create the codedom property and attach it to the codedom type.
            var codeProperty = new CodeMemberProperty();
            codeTypeDeclaration.Members.Add(codeProperty);

            // Assign the name.
            codeProperty.Name = property.Name;

            // Assign the return type, making sure to check for null.
            if (property.TypeName == "void")
                codeProperty.Type = null;
            else
                codeProperty.Type = new CodeTypeReference(property.TypeName);

            // Translate the accessibility.
            MemberAttributes memberAttributes = MemberAttributes.Public;
            switch (property.Accessibility)
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
            if (property.IsShared) memberAttributes |= MemberAttributes.Static;
            if (property.IsAbstract) memberAttributes |= MemberAttributes.Abstract;
            if (property.IsOverride) memberAttributes |= MemberAttributes.Override;

            codeProperty.Attributes = memberAttributes;

            // Add the statements for the get block.
            if (property.GetBlock.ChildExpressions.Count > 0)
                foreach (var e in property.GetBlock.ChildExpressions)
                    codeProperty.GetStatements.Add(CodeDomEmitter.EmitCodeStatement(e));
            else
                codeProperty.GetStatements.Add(new CodeCommentStatement("Placeholder statement"));

            // Add the statements for the set block.
            if (property.SetBlock.ChildExpressions.Count > 0)
                foreach (var e in property.SetBlock.ChildExpressions)
                    codeProperty.SetStatements.Add(CodeDomEmitter.EmitCodeStatement(e));
            else
                codeProperty.SetStatements.Add(new CodeCommentStatement("Placeholder statement"));
        }
    }
}

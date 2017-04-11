using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class EventEmitter
    {
        // Builds a codedom event and attaches it to the given type.
        // It seems that there is a codedom bug: you can't make events static.
        public static void Emit(CodeTypeDeclaration codeType, Event e)
        {
            // Create the codedom event and attach to the codedom type.
            var codeEvent = new CodeMemberEvent();
            codeType.Members.Add(codeEvent);

            // Assign a name
            codeEvent.Name = e.Name;

            // Assign the type.
            codeEvent.Type = new CodeTypeReference(e.DelegateName);

            // Translate the accessibility
            MemberAttributes memberAttributes = MemberAttributes.Public;
            switch (e.Accessibility)
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

            // This is bugged in codedom: no effect.
            if (e.IsShared)
                memberAttributes |= MemberAttributes.Static;

            codeEvent.Attributes = memberAttributes;
        }
    }
}

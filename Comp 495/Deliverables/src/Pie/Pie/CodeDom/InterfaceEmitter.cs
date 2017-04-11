using System;
using System.CodeDom;
using System.Reflection;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class InterfaceEmitter
    {
        public static void Emit(CodeNamespace codeNamespace, Interface inter)
        {
            var codeType = new CodeTypeDeclaration();
            codeNamespace.Types.Add(codeType);

            codeType.Name = inter.UnqualifiedName;

            var typeAttr = TypeAttributes.Interface;

            // Add the list of generic type names of the class.
            foreach (string s in inter.GenericTypeNames)
                codeType.TypeParameters.Add(new CodeTypeParameter(s));

            // Add the list of base type names of the class.
            foreach (string s in inter.BaseTypeNames)
                codeType.BaseTypes.Add(s);

            // Set the accessibility of the class.
            switch (inter.Accessibility)
            {
                case Accessibility.Internal:
                    typeAttr |= TypeAttributes.NestedAssembly;
                    break;
                case Accessibility.Public:
                    typeAttr |= TypeAttributes.Public;
                    break;
            }
            codeType.TypeAttributes = typeAttr;

            // Emit interface methods and properties.
            foreach (var e in inter.ChildExpressions)
            {
                if (e is MethodDeclaration)
                    MethodEmitter.Emit(codeType, (MethodDeclaration)e);
                if (e is Property)
                    PropertyEmitter.Emit(codeType, (e as Property));
            }
        }
    }
}

using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class NamespaceEmitter
    {
        // Generates a codedom namespace and attaches it to the compile unit (the root of the tree)
        public static void Emit(CodeCompileUnit codeCompileUnit, Namespace ns)
        {
            // Create the codedom namespace expression
            var codeNamespace = new CodeNamespace();

            // Assign the namespace name.
            codeNamespace.Name = ns.GetFullName();

            // Attach it to the root of the codedom tree.
            codeCompileUnit.Namespaces.Add(codeNamespace);

            // Create and attach the children of the namespace: classes, delegates, other namespaces, etc.
            foreach (var e in ns.ChildExpressions)
            {
                if (e is Namespace)
                    Emit(codeCompileUnit, (Namespace)e);
                if (e is Class)
                    ClassEmitter.Emit(codeNamespace, (Class)e);
                if (e is Import)
                {
                    var i = e as Import;
                    if (i.IsType) // Are we importing a class, enum, interface, struct, module?
                        codeNamespace.Imports.Add(new CodeNamespaceImport((e as Import).GetNamespace()));
                    else
                        codeNamespace.Imports.Add(new CodeNamespaceImport((e as Import).Name));
                }
                if (e is Pie.Expressions.Enum)
                    EnumEmitter.Emit(codeNamespace, e as Pie.Expressions.Enum);
                if (e is DelegateDeclaration)
                    DelegateEmitter.Emit(codeNamespace, e as DelegateDeclaration);
                if (e is Interface)
                    InterfaceEmitter.Emit(codeNamespace, e as Interface);
            }
        }
    }
}

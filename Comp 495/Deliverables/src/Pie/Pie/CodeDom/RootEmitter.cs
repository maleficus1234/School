using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class RootEmitter
    {
        // Emit a codedom compile unit: this is the root of the codedom tree.
        public static CodeCompileUnit Emit(Root root)
        {
            var codeCompileUnit = new CodeCompileUnit();

            // Create an global namespace with no name, for any classes, delegates, etc
            // that are attached to root and hence in the global namespace.
            var globalNamespace = new Namespace(root, null);

            foreach(var e in root.ChildExpressions)
            {
                if (e is Namespace)
                    NamespaceEmitter.Emit(codeCompileUnit, (Namespace)e);
                else // If it's not a namespace, transfer it to the global namespace.
                    globalNamespace.ChildExpressions.Add(e);
            }

            // Generate the global namespace codedom-fu.
            NamespaceEmitter.Emit(codeCompileUnit, globalNamespace);

            return codeCompileUnit;
        }
    }
}

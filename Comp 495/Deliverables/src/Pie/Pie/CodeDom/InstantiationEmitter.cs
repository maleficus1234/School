using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal class InstantiationEmitter
    {
        // Generates a codedom instantiation expression: new foo() or new foo[x].
        public static CodeExpression Emit(Instantiation instantiation)
        {
            // Array instantiation needs a different treatment.
            if (instantiation.IsArray)
            {
                var c = new CodeArrayCreateExpression();

                c.CreateType = new CodeTypeReference(instantiation.Name);
                c.SizeExpression = CodeDomEmitter.EmitCodeExpression(instantiation.Parameters.ChildExpressions[0]);
                return c;
            }
            else // Non-array instantiation
            {
                var c = new CodeObjectCreateExpression();

                // The type that is being created
                var createType = new CodeTypeReference(instantiation.Name);

                // Apply the generic type names, if any.
                foreach (var g in instantiation.GenericTypes)
                {
                    createType.TypeArguments.Add(new CodeTypeReference(g));
                }
                c.CreateType = createType;

                // Translate the instantiation parameters.
                foreach (var a in instantiation.Parameters.ChildExpressions)
                    c.Parameters.Add(CodeDomEmitter.EmitCodeExpression(a));

                return c;
            }
        }
    }
}

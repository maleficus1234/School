using System;
using System.CodeDom;
using System.Reflection;

using Pie.Expressions;

namespace Pie.CodeDom
{
    internal static class ClassEmitter
    {
        /* Builds the CodeDOM statement for a class, and attaches it to a 
         * CodeDOM namespace statement. */
        public static void Emit(CodeNamespace codeNamespace, Class c)
        {
            // CodeTypeDeclaration is the CodeDOM representation of a
            // class, struct, or enum.
            var codeTypeDeclaration = new CodeTypeDeclaration();
            codeNamespace.Types.Add(codeTypeDeclaration);

            // Assign the unqualified name (without namespace).
            codeTypeDeclaration.Name = c.UnqualifiedName;

            if(c.IsStruct)
                codeTypeDeclaration.IsStruct = true;
            else
            // Flag the type as a class.
                codeTypeDeclaration.IsClass = true;

            // Create the enum that sets attributes of the class.
            var typeAttr = TypeAttributes.Class;

            // If the class is "final", it is "sealed" in C# terms.
            if (c.IsFinal)
                typeAttr |= TypeAttributes.Sealed;

            if (c.IsAbstract)
                typeAttr |= TypeAttributes.Abstract;

            // Assign the partial state of the class.
            codeTypeDeclaration.IsPartial = c.IsPartial;

            // Add the list of generic type names of the class.
            foreach (string s in c.GenericTypeNames)
                codeTypeDeclaration.TypeParameters.Add(new CodeTypeParameter(s));

            // Add the list of base type names of the class.
            foreach(string s in c.BaseTypeNames)
                codeTypeDeclaration.BaseTypes.Add(s);
            
            // Set the accessibility of the class.
            switch(c.Accessibility)
            {
                case Accessibility.Internal:
                    typeAttr |= TypeAttributes.NestedAssembly;
                    break;
                case Accessibility.Public:
                    typeAttr |= TypeAttributes.Public;
                    break;
            }
            codeTypeDeclaration.TypeAttributes = typeAttr;

            /* Iterate through and emit the children */
            foreach (var e in c.ChildExpressions)
            {
                if (e is ClassVariable)
                    ClassVariableEmitter.Emit(codeTypeDeclaration, e as ClassVariable);
                if (e is MethodDeclaration)
                    MethodEmitter.Emit(codeTypeDeclaration, (MethodDeclaration)e);
                if (e is Constructor)
                    ConstructorEmitter.Emit(codeTypeDeclaration, (e as Constructor));
                if (e is Property)
                    PropertyEmitter.Emit(codeTypeDeclaration, (e as Property));
                if (e is Event)
                    EventEmitter.Emit(codeTypeDeclaration, (e as Event));
            }
        }
    }
}

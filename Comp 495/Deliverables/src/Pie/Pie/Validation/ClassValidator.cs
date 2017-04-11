using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pie.Expressions;

namespace Pie.Validation
{
    static class ClassValidator
    {
        public static void Validate(Validator validator, Root root, Class c)
        {
            bool validated = true;

            // As a module, it can't have base types
            if(c.IsModule)
            {
                if(c.BaseTypeNames.Count > 0)
                    root.CompilerErrors.Add(new ModuleInheritanceCompilerError("", c.Token.Line, c.Token.Position));

                foreach (var e in c.ChildExpressions)
                    if (e is Constructor)
                        root.CompilerErrors.Add(new ModuleConstructorCompilerError("", c.Token.Line, c.Token.Position));

                if (c.IsAbstract)
                    root.CompilerErrors.Add(new ModuleModifierCompilerError("", c.Token.Line, c.Token.Position));
            }


            c.Validated = validated;
        }
    }
}

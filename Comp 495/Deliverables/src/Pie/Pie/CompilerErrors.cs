
using System.CodeDom.Compiler;

namespace Pie
{
    // Compiler error produced when there are more than one access modifiers: ex public private class foo:
    public class MultipleAccessModifiersCompilerError
        : CompilerError
    {
        public MultipleAccessModifiersCompilerError(string fileName, int line, int column)
            : base(fileName, line, column, "PIE0001", "Multiple access modifiers." )
        {

        }
    }

    // Compiler error produced when there are duplicate access modifiers: ex public private class foo:
    public class DuplicateModifiersCompilerError
        : CompilerError
    {
        public DuplicateModifiersCompilerError(string fileName, int line, int column)
            : base(fileName, line, column, "PIE0002", "Duplicate modifiers." )
        {

        }
    }

    // Compiler error representing an Irony parser error
    public class ParserCompilerError
        : CompilerError
    {
        public ParserCompilerError(string fileName, int line, int column, string message)
            : base(fileName, line, column, "PIE0003", message )
        {

        }
    }
    
    // A modifier that is inappropriate for a module: for example, "abstract"
    public class ModuleModifierCompilerError
        : CompilerError
    {
        public ModuleModifierCompilerError(string fileName, int line, int column)
            : base(fileName, line, column, "PIE0004", "Invalid module modifier.")
        {

        }
    }

    // Error produced if a module has base types
    public class ModuleInheritanceCompilerError
        : CompilerError
    {
        public ModuleInheritanceCompilerError(string fileName, int line, int column)
            : base(fileName, line, column, "PIE0006", "Modules are unable to inherit.")
        {

        }
    }

    // Incompatible modifiers: ex abstract and final
    public class IncompatibleModifiersCompilerError
        : CompilerError
    {
        public IncompatibleModifiersCompilerError(string fileName, int line, int column)
            : base(fileName, line, column, "PIE0007", "Incompatible modifiers.")
        {

        }
    }

    // Error produced when a module has a constructor.
    public class ModuleConstructorCompilerError
        : CompilerError
    {
        public ModuleConstructorCompilerError(string fileName, int line, int column)
            : base(fileName, line, column, "PIE0008", "Modules can not have constructors.")
        {

        }
    }
}

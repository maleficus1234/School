using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

using Pie.Expressions;
using Pie.CodeDom;

namespace Pie
{
    // .NET compilers extend CodeDomProvider to provide the compiler service
    public class PieCodeProvider
        : CodeDomProvider
    {
        Parser parser;
        Validator validator;

        public PieCodeProvider()
        {
            parser = Parser.Create();
            validator = new Validator();
        }

        public PieCodeProvider(Parser parser)
        {
            if(parser == null)
            {
                throw new NullReferenceException();
            }

            this.parser = parser;
        }

        [Obsolete]
        public override ICodeCompiler CreateCompiler()
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override ICodeGenerator CreateGenerator()
        {
            throw new NotImplementedException();
        }

        // Build an assembly from a list of source strings.
        public override CompilerResults CompileAssemblyFromSource(CompilerParameters options, params string[] sources)
        {
            var root = new Root();

            foreach(string code in sources)
                parser.Parse(root, code);

            if(root.CompilerErrors.Count > 0)
            {
                var results = new CompilerResults(null);
                foreach(var e in root.CompilerErrors)
                    results.Errors.Add(e);
                return results;
            }

            validator.Validate(options, root);

            if (root.CompilerErrors.Count > 0)
            {
                var results = new CompilerResults(null);
                foreach (var e in root.CompilerErrors)
                    results.Errors.Add(e);
                return results;
            }

            var codeDomEmitter = new CodeDomEmitter();
            return codeDomEmitter.Emit(options, root);
        }

        // Build an assembly from a list of source file names.
        public override CompilerResults CompileAssemblyFromFile(CompilerParameters options, params string[] fileNames)
        {
            var sources = new List<string>();
            
            foreach(string filename in fileNames)
            {
                var reader = new StreamReader(filename);
                sources.Add(reader.ReadToEnd());
                reader.Close();  
            }

            return CompileAssemblyFromSource(options, sources.ToArray());
        }
    }
}

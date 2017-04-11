using System;
using System.CodeDom;
using System.IO;
using System.CodeDom.Compiler;

using Pie.Expressions;

namespace Pie.CodeDom
{
    // The main entry class for CodeDOM generation.
    internal class CodeDomEmitter
    {
        // Emit byte code for the expression tree owned by this root.
        // Accepts the compiler parameters that were given to PieCodeProvider
        // since it will be needed for the CSharpCodeProvider.
        public CompilerResults Emit(CompilerParameters compilerParams, Root root)
        {
            // Emit the code compile unit, the top of the codedom tree.
            // This method will cal emit method for all child expressions
            // until all expressions have had byte code emitted.
            var codeCompileUnit = RootEmitter.Emit(root);

            // Create the C# compiler.
            var csProvider = new Microsoft.CSharp.CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            // Compile the codedom tree into an assembly
            var sw = new StringWriter();
            csProvider.GenerateCodeFromCompileUnit(codeCompileUnit, sw, options);

            // Display the C# code for debugging purposes.
            string ccode = sw.GetStringBuilder().ToString();
            Console.WriteLine(ccode);

            // Get the results of the compilation: the assembly and error list
            CompilerResults results = csProvider.CompileAssemblyFromSource(compilerParams, ccode);

            // Store all C# compiler errors, so that they can be included with Pie compiler errors..
            foreach (CompilerError e in root.CompilerErrors)
                results.Errors.Add(e);
            root.CompilerErrors.Clear();

            return results;
        }

        // Emits expressions: in codedom terms, those that can't stand on their own.
        public static CodeExpression EmitCodeExpression(Expression expression)
        {
            if (expression is Literal)
                return LiteralEmitter.Emit((Literal)expression);
            if (expression is MethodInvocation)
                return MethodEmitter.Emit((MethodInvocation)expression);
            if (expression is VariableReference)
                return new CodeVariableReferenceExpression((expression as VariableReference).Name);
            if (expression is BinaryOperator)
                return OperatorEmitter.Emit((BinaryOperator)expression);
            if (expression is UnaryOperator)
                return OperatorEmitter.Emit(expression as UnaryOperator);
            if (expression is DirectionedArgument)
                return MethodEmitter.Emit(expression as DirectionedArgument);
            if (expression is Instantiation)
                return InstantiationEmitter.Emit(expression as Instantiation);
            if (expression is Value)
                return new CodeSnippetExpression("value");
            if (expression is IndexedIdentifier)
                return IndexedIdentifierEmitter.Emit(expression as IndexedIdentifier);

            throw new NotImplementedException();
        }

        // Emits statements: in codedom terms: those that can stand on their own.
        public static CodeStatement EmitCodeStatement(Expression expression)
        {
            if (expression is Literal)
                return new CodeExpressionStatement(EmitCodeExpression(expression));
            if (expression is IfBlock)
                return IfBlockEmitter.Emit((IfBlock)expression);
            if (expression is Return)
                return ReturnEmitter.Emit((Return)expression);
            if (expression is MethodInvocation)
                return new CodeExpressionStatement(MethodEmitter.Emit((MethodInvocation)expression));
            if (expression is Assignment)
                return AssignmentEmitter.Emit((Assignment)expression);
            if (expression is ExplicitVariableDeclaration)
            {
                var e = expression as ExplicitVariableDeclaration;
                var codeType = new CodeTypeReference();
                codeType.BaseType = e.TypeName;
                foreach(string s in e.GenericTypes)
                {
                    codeType.TypeArguments.Add(new CodeTypeReference(s));
                }
                var codeVar = new CodeVariableDeclarationStatement(codeType, e.Name);
                return codeVar;
            }
            if (expression is WhileLoop)
                return LoopEmitter.Emit(expression as WhileLoop);
            if (expression is ForLoop)
                return LoopEmitter.Emit(expression as ForLoop);
            if (expression is ExceptionHandler)
                return ExceptionHandlerEmitter.Emit(expression as ExceptionHandler);
            if (expression is Throw)
                return ExceptionHandlerEmitter.Emit(expression as Throw);
            if (expression is SwitchBlock)
                return SwitchEmitter.Emit(expression as SwitchBlock);
            throw new NotImplementedException();
        }
    }
}

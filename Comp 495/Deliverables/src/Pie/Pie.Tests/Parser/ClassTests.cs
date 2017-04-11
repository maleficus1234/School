using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    // Tests for the parser component of the compiler: Classes.

    [TestClass]
    public class ClassTests
    {
        /* All classes produced during a test should be
         * valiated with this method. Scenarios to test:
         * 
         * Class with body
         * Name
         * Accessibility
         * Generic parameters
         * Class inheritance
         * Interface inheritance
         * Final (sealed)
         * Module (static class)
         * Correct parent expression (root, namespace, class, or module)
         * If a compiler error is supposed to be thrown, validate it is the correct error.
         * 
         * */

        public static void ValidateClass(Class c, 
            Expression parentExpression, 
            string name, 
            string fullName, 
            Accessibility access, 
            bool module, 
            bool final,
            bool partial,
            bool isAbstract,
            bool isStruct)
        {
            // Validate the class
            Assert.AreEqual(name, c.UnqualifiedName, "class Name");
            Assert.AreEqual(module, c.IsModule, "IsModule");
            Assert.AreEqual(fullName, c.GetQualifiedName(), "FullName");
            Assert.AreEqual(c.Accessibility, access, "Accessibility");
            Assert.AreEqual(parentExpression, c.ParentExpression, "ParentExpresion");
            Assert.AreEqual(final, c.IsFinal, "IsFinal");
            Assert.AreEqual(partial, c.IsPartial, "IsPartial");
            Assert.AreEqual(isAbstract, c.IsAbstract, "IsAbstract");
            Assert.AreEqual(isStruct, c.IsStruct);
        }

        // Test that a compiler error is generated when a class is declared with no name.
        [TestMethod]
        public void ParseClassNoName()
        {
            string code = @"
                            class";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that one error was produced.
            Assert.AreEqual(root.CompilerErrors.Count, 1, "1 error");

            // Check that the correct error was generated
            Assert.IsTrue(root.CompilerErrors[0] is ParserCompilerError, "correct error");
        }

        // Test that a compiler error is generated when a module is declared with no name.
        [TestMethod]
        public void ParseModuleNoName()
        {
            string code = @"
                            module";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that one error was produced.
            Assert.AreEqual(root.CompilerErrors.Count, 1, "1 error");

            // Check that the correct error was generated
            Assert.IsTrue(root.CompilerErrors[0] is ParserCompilerError, "correct error");
        }

        // Test that no compiler errors are generated with a basic, correct, class declaration.
        [TestMethod]
        public void ParseClassSimple()
        {
            string code = @"
                            class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a class
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Class, "child is class");

            ValidateClass((Class)root.ChildExpressions[0],
                root,
                "foo",
                "foo",
                Accessibility.Public,
                false,
                false,
                false,
                false,
                false);
        }

        // Test that no compiler errors are generated with a basic, correct, class declaration.
        [TestMethod]
        public void ParseStructSimple()
        {
            string code = @"
                            struct foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a class
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Class, "child is class");

            ValidateClass((Class)root.ChildExpressions[0],
                root,
                "foo",
                "foo",
                Accessibility.Public,
                false,
                false,
                false,
                false,
                true);
        }

        // Test that no compiler errors are generated with a basic, correct, class declaration.
        [TestMethod]
        public void ParseModuleSimple()
        {
            string code = @"
                            module foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a class
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Class, "child is class");

            ValidateClass((Class)root.ChildExpressions[0],
                root,
                "foo",
                "foo",
                Accessibility.Public,
                true,
                true,
                true,
                false,
                false);
        }

        // Check that an internal class declaration parses correctly.
        [TestMethod]
        public void ParseClassInternal()
        {
            string code = @"
                            internal class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a class
            Assert.AreEqual(root.ChildExpressions.Count, 1, "1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Class, "Child is class");
            ValidateClass((Class)root.ChildExpressions[0],
                root,
                "foo",
                "foo",
                Accessibility.Internal,
                false,
                false,
                false,
                false,
                false);
        }

        // Check that a final class declaration parses correctly
        [TestMethod]
        public void ParseClassFinal()
        {
            string code = @"
                            final class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "Compiler errors");

            // Check that the only child of root is a class
            Assert.AreEqual(root.ChildExpressions.Count, 1, "Root: 1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Class, "Root: child is class");

            ValidateClass((Class)root.ChildExpressions[0],
                root,
                "foo",
                "foo",
                Accessibility.Public,
                false,
                true,
                false,
                false,
                false);
        }

        // Check that a final class declaration parses correctly
        [TestMethod]
        public void ParseClassAbstract()
        {
            string code = @"
                            abstract class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "Compiler errors");

            // Check that the only child of root is a class
            Assert.AreEqual(root.ChildExpressions.Count, 1, "Root: 1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Class, "Root: child is class");

            ValidateClass((Class)root.ChildExpressions[0],
                root,
                "foo",
                "foo",
                Accessibility.Public,
                false,
                false,
                false,
                true,
                false);
        }

        // Check that a class with duplicate final modifiers correctly generates an error.
        [TestMethod]
        public void ParseClassDuplicateFinal()
        {
            string code = @"
                            final final class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that an error was produced
            Assert.AreEqual(root.CompilerErrors.Count, 1, "Compiler errors");

            // Check that it's the correct error
            Assert.IsTrue(root.CompilerErrors[0] is DuplicateModifiersCompilerError, "Correct error");
        }

        // Check that a class with duplicate final modifiers correctly generates an error.
        [TestMethod]
        public void ParseClassDuplicateAbstract()
        {
            string code = @"
                            abstract abstract class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that an error was produced
            Assert.AreEqual(root.CompilerErrors.Count, 1, "Compiler errors");

            // Check that it's the correct error
            Assert.IsTrue(root.CompilerErrors[0] is DuplicateModifiersCompilerError, "Correct error");
        }

        // Check that a class with duplicate public modifiers correctly generates an error.
        [TestMethod]
        public void ParseClassDuplicateModifiers()
        {
            string code = @"
                            public public class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that an error was produced
            Assert.AreEqual(root.CompilerErrors.Count, 1, "Compiler errors");

            // Check that it's the correct error
            Assert.IsTrue(root.CompilerErrors[0] is MultipleAccessModifiersCompilerError, "Correct error");
        }

        // Check that a class with duplicate internal keywords correctly generates an error.
        [TestMethod]
        public void ParseClassDuplicateInternal()
        {
            string code = @"
                            internal internal class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that an error was produced
            Assert.AreEqual(root.CompilerErrors.Count, 1, "Compiler errors");

            // Check that it's the correct error
            Assert.IsTrue(root.CompilerErrors[0] is MultipleAccessModifiersCompilerError, "Correct error");
        }

        // Check that a class with multiple, but different, access modifiers correctly generates an error.
        [TestMethod]
        public void ParseClassMultipleAccessModifiers()
        {
            string code = @"
                            public internal class foo:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that an error was produced
            Assert.AreEqual(1, root.CompilerErrors.Count, "Compiler errors");

            // Check that it's the correct error
            Assert.IsTrue(root.CompilerErrors[0] is MultipleAccessModifiersCompilerError, "Correct error");           
        }

        [TestMethod]
        public void ParseClassInheritance()
        {
            string code = @"
                            class foo (bar, bar2):";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");

            Class c = (Class)root.ChildExpressions[0];

            ValidateClass(c,
                    root,
                    "foo",
                    "foo",
                    Accessibility.Public,
                    false,
                    false,
                    false,
                    false,
                false);

            // Validate base types
            Assert.AreEqual("bar", c.BaseTypeNames[0], "base type 1");
            Assert.AreEqual("bar2", c.BaseTypeNames[1], "base type 2");
        }

        [TestMethod]
        public void ParseClassInheritanceList()
        {
            string code = @"
                            class foo(bar, 
                                    bar2, bar3):";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");

            Class c = (Class)root.ChildExpressions[0];

            ValidateClass(c,
                    root,
                    "foo",
                    "foo",
                    Accessibility.Public,
                    false,
                    false,
                    false,
                    false,
                false);

            // Validate base types
            Assert.AreEqual("bar", c.BaseTypeNames[0], "base type 1");
            Assert.AreEqual("bar2", c.BaseTypeNames[1], "base type 2");
            Assert.AreEqual("bar3", c.BaseTypeNames[2], "base type 3");
        }

        [TestMethod]
        public void ParseClassGeneric()
        {
            string code = @"
                            class foo{T, F}:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");

            Class c = (Class)root.ChildExpressions[0];

            ValidateClass(c,
                    root,
                    "foo",
                    "foo",
                    Accessibility.Public,
                    false,
                    false,
                    false,
                    false,
                false);

            // Validate base types
            Assert.AreEqual("T", c.GenericTypeNames[0], "generic type 1");
            Assert.AreEqual("F", c.GenericTypeNames[1], "generic type 2");
        }

        [TestMethod]
        public void ParseClassGenericInherited()
        {
            string code = @"
                            class foo{T}(bar2):";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");

            Class c = (Class)root.ChildExpressions[0];

            ValidateClass(c,
                    root,
                    "foo",
                    "foo",
                    Accessibility.Public,
                    false,
                    false,
                    false,
                    false,
                false);

            // Validate base types
            Assert.AreEqual("T", c.GenericTypeNames[0], "generic type");
            Assert.AreEqual("bar2", c.BaseTypeNames[0], "base type");
        }

        [TestMethod]
        public void ParseClassGenericInheritedIndented()
        {
            string code = @"
                            class foo{T}
                                (bar2):";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");

            Class c = (Class)root.ChildExpressions[0];

            ValidateClass(c,
                    root,
                    "foo",
                    "foo",
                    Accessibility.Public,
                    false,
                    false,
                    false,
                    false,
                    false);

            // Validate base types
            Assert.AreEqual("T", c.GenericTypeNames[0], "generic type");
            Assert.AreEqual("bar2", c.BaseTypeNames[0], "base type");
        }

        [TestMethod]
        public void ParseClassGenericInheritedBothIndented()
        {
            string code = @"
                            class foo{T, a}
                                (bar2, b):";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no error was produced
            Assert.AreEqual(0, root.CompilerErrors.Count, "Compiler errors");

            Class c = (Class)root.ChildExpressions[0];

            ValidateClass(c,
                    root,
                    "foo",
                    "foo",
                    Accessibility.Public,
                    false,
                    false,
                    false,
                    false,
                false);

            // Validate base types
            Assert.AreEqual("T", c.GenericTypeNames[0], "generic type");
            Assert.AreEqual("bar2", c.BaseTypeNames[0], "base type");
        }


        // Check that a simple class inside a namespace parses correctly.
        [TestMethod]
        public void ParseNamespaceClassSimple()
        {
            string code = @"
                            namespace foo:
                                class bar:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check that the only child of root is a namespace
            Assert.AreEqual(root.ChildExpressions.Count, 1, "root 1 child");
            Assert.IsTrue(root.ChildExpressions[0] is Namespace, "root child namespace");

            // Check that the only child of the namespace is a class.
            Assert.AreEqual(root.ChildExpressions[0].ChildExpressions.Count, 1, "namespace 1 child");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is Class, "namespace child class");

            ValidateClass((Class)root.ChildExpressions[0].ChildExpressions[0],
                root.ChildExpressions[0],
                "bar",
                "foo.bar",
                Accessibility.Public,
                false,
                false,
                false,
                false,
                false);
        }


    }
}

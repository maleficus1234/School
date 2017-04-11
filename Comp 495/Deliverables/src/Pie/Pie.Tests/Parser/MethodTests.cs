using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie;
using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class MethodTests
    {

        [TestMethod]
        public void ParseMethodReturn()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the return
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is Return, "return");
        }

        [TestMethod]
        public void ParseMethodAbstract()
        {
            string code = @"
                                class bar:
                                    abstract void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the abstract
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).IsAbstract);
        }

        [TestMethod]
        public void ParseMethodShared()
        {
            string code = @"
                                class bar:
                                    shared void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the abstract
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).IsShared);
        }

        [TestMethod]
        public void ParseMethodPublic()
        {
            string code = @"
                                class bar:
                                    public void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the abstract
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).Accessibility == Accessibility.Public);
        }

        [TestMethod]
        public void ParseMethodInternal()
        {
            string code = @"
                                class bar:
                                    internal void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the abstract
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).Accessibility == Accessibility.Internal);
        }

        [TestMethod]
        public void ParseMethodProtected()
        {
            string code = @"
                                class bar:
                                    protected void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the abstract
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).Accessibility == Accessibility.Protected);
        }

        [TestMethod]
        public void ParseMethodPrivate()
        {
            string code = @"
                                class bar:
                                    private void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the abstract
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).Accessibility == Accessibility.Private);
        }

        [TestMethod]
        public void ParseMethodVirtual()
        {
            string code = @"
                                class bar:
                                    virtual void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the virtual
            Assert.IsTrue((root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).IsVirtual);
        }

        [TestMethod]
        public void ParseMethodDuplicatePublic()
        {
            string code = @"
                                class bar:
                                    public public def void boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            Assert.AreEqual(root.CompilerErrors.Count, 1, "no errors");
        }

        [TestMethod]
        public void ParseMethodReturnValue()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        return 1";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the return
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is Return, "return");
            var r = root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as Return;
            Assert.IsTrue(r.ChildExpressions[0] is Literal, "return int");
            Assert.AreEqual(1, (r.ChildExpressions[0] as Literal).Value, "return value");
        }

        [TestMethod]
        public void ParseMethodInvocation()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        foo(1)
                                    ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the invocation
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is MethodInvocation, "invocation");
            var i = root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as MethodInvocation;
         //   Assert.AreEqual("foo", i.Name, "invoke name");
//            Assert.AreEqual(1, (i.Parameters.ChildExpressions[0] as Literal).Value, "invoke value");
        }

        [TestMethod]
        public void ParseMethodArgs()
        {
            string code = @"
                                module bar:
                                    void boo(int i, int b, out int a, ref int b):
                                        foo(1)
                                    ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the invocation
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is MethodInvocation, "invocation");
            var i = root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as MethodInvocation;
            //   Assert.AreEqual("foo", i.Name, "invoke name");
            //            Assert.AreEqual(1, (i.Parameters.ChildExpressions[0] as Literal).Value, "invoke value");
        }

        [TestMethod]
        public void ParseMethodNestedInvocation()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        foo(rawr(1))";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the invocation
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is MethodInvocation, "invocation");
            var i = root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as MethodInvocation;
///            Assert.AreEqual("foo", i.Name, "invoke name");
//            Assert.AreEqual("rawr", (i.Parameters.ChildExpressions[0] as MethodInvocation).Name, "invoke value");
        }

        [TestMethod]
        public void ParseMethodDirectedInvocation()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        foo(out i, ref i, i)
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the invocation
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] is MethodInvocation, "invocation");
            var i = root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as MethodInvocation;
            ///            Assert.AreEqual("foo", i.Name, "invoke name");
            //            Assert.AreEqual("rawr", (i.Parameters.ChildExpressions[0] as MethodInvocation).Name, "invoke value");
        }

        [TestMethod]
        public void ParseTypedArgsMethod()
        {
            string code = @"
                                module bar:
                                    int boo(int i, int b):";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            var m = root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration;
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(m is MethodDeclaration, "child is method");

            // Check the arguments
            Assert.AreEqual("System.Int32", (m.Parameters[0] as SimpleParameter).TypeName, "arg 1 type");
            Assert.AreEqual("i", (m.Parameters[0] as SimpleParameter).Name, "arg 1 name");
            Assert.AreEqual("System.Int32", (m.Parameters[1] as SimpleParameter).TypeName, "arg 2 type");
            Assert.AreEqual("b", (m.Parameters[1] as SimpleParameter).Name, "arg 2 name");
        }

        [TestMethod]
        public void ParseUntypedGenericMethod()
        {
            string code = @"
                                module bar:
                                    void boo{T,F}():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check that it has the generic type
            Assert.AreEqual("T", (root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).GenericTypeNames[0], "generic1");
            Assert.AreEqual("F", (root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).GenericTypeNames[1], "generic2");

        }

        [TestMethod]
        public void ParseTypedMethod()
        {
            string code = @"
                                module bar:
                                    int boo():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");
        }

        [TestMethod]
        public void ParseTypedMethodMember()
        {
            string code = @"
module bar:
    int boo():
        int boo
";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check for the variable
            var variable = (root.ChildExpressions[0].ChildExpressions[0].ChildExpressions[0] as ExplicitVariableDeclaration);
            Assert.IsTrue(variable is ExplicitVariableDeclaration, "is variable");
            Assert.AreEqual("System.Int32", variable.TypeName, "type name");
            Assert.AreEqual("boo", variable.Name, "variable name");
        }

        [TestMethod]
        public void ParseTypedGenericMethod()
        {
            string code = @"
                                module bar:
                                    int boo{T,F}():";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

            // Check that it has the generic type
            Assert.AreEqual("T", (root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).GenericTypeNames[0], "generic1");
            Assert.AreEqual("F", (root.ChildExpressions[0].ChildExpressions[0] as MethodDeclaration).GenericTypeNames[1], "generic2");

        }

        [TestMethod]
        public void ParseSimpleMethodComplexClass()
        {
            string code = @"
                                final class bar{t, x}
                                    (y, f):

                                    void boo():
                    ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

            // Check  that the class has one member: a method
            Assert.AreEqual(1, root.ChildExpressions[0].ChildExpressions.Count, " child count");
            Assert.IsTrue(root.ChildExpressions[0].ChildExpressions[0] is MethodDeclaration, "child is method");

        }


        [TestMethod]
        public void ParseSimpleMethodInvocation()
        {
            string code = @"
                                module bar:

                                    void boo():
                                        boo()
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

        }

        [TestMethod]
        public void ParseGenericMethodInvocation()
        {
            string code = @"
                                module bar:

                                    void boo():
                                        boo{foo}()
                            ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

        }
    }
}

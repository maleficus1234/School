using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pie.Expressions;

namespace Pie.Tests.Parser
{
    [TestClass]
    public class LoopTests
    {
        [TestMethod]
        public void ParseSimpleForLoop()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 10 return
";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");

        }

        [TestMethod]
        public void ParseSimpleForLoop2()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 0 to 10 return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseSimpleForLoop3()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in x return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedForLoop4()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 10:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedForLoop5()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in x:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedForLoop6()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 0 to 10:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedForLoop7()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 10 to x step a:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedForLoop8()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 10 to x step a:";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseSimpleForLoop8()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        for i in 10 step 1 return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseSimpleWhileLoop1()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        while true return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseSimpleWhileLoop2()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        while i < 2 return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseSimpleWhileLoop3()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        while v return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedWhileLoop1()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        while true:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedWhileLoop2()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        while i < 2: 
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedWhileLoop3()
        {
            string code = @"
                                module bar:
                                   void boo():
                                        while v:
                                            return";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }

        [TestMethod]
        public void ParseBodiedWhileLoop4()
        {
            string code = @"
                                module bar:
                                    void boo():
                                        while v:
                              ";

            var parser = Pie.Parser.Create();
            var root = new Root();
            parser.Parse(root, code);

            // Check that no errors were produced.
            Assert.AreEqual(root.CompilerErrors.Count, 0, "no errors");
        }
    }
}

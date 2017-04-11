
using System.CodeDom.Compiler;

using Pie.Expressions;
using Pie.Validation;

namespace Pie
{
    // Base class for Validators: doesn't do a much of anything for this iteration.
    public class Validator
    {
        CompilerParameters options;

        public Validator()
        {

        }

        public void Validate(CompilerParameters options, Root root)
        {
            this.options = options;

            PrepareValidation(root, root, null);

            ValidateRecursive(root, root);
        }

        void PrepareValidation(Root root, Expression expression, Import import)
        {
            if (import != null)
            {
                expression.Imports.Add(import);
                foreach (var c in expression.ChildExpressions)
                    PrepareValidation(root, c, import);
            }
        }

        void ValidateRecursive(Root root, Expression expression)
        {
            if (expression is Class)
                ClassValidator.Validate(this, root, expression as Class);

            foreach (var child in expression.ChildExpressions)
                ValidateRecursive(root, child);
        }
    }
}

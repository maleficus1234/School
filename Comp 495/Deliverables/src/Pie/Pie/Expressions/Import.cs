
namespace Pie.Expressions
{
    // Represents a namespace import
    public class Import
        : Expression
    {
        // The name to import
        public string Name { get; set; }
        // Is this import a tyep or namespace?
        public bool IsType { get; set; }

        public Import(Expression parentExpression, Token token)
            :base(parentExpression, token)
        {
            IsType = false;
        }

        public string GetNamespace()
        {
            if (!IsType)
                return Name;

            var names = Name.Split('.');
            string ns = names[0];
            if (ns.Length > 1)
                for (int i = 1; i < names.Length - 1; i++)
                    ns += "." + names[i];

            return ns;
        }

        public string GetTypeName()
        {
            if (!IsType) return null;

            var names = Name.Split('.');
            return names[names.Length - 1];
        }
    }
}

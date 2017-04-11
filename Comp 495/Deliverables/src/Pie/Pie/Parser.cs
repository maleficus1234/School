
using Pie.Expressions;

namespace Pie
{
    // Base type for the parser component
    public abstract class Parser
    {
        public abstract void Parse(Root root, string text);

        public static Parser Create()
        {
            return new Pie.IronyParsing.IronyParser();
        }
    }
}

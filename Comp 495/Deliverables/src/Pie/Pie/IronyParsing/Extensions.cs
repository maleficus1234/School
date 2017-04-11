

namespace Pie.IronyParsing
{
    internal static class Extensions
    {
        // Extension to convert an Irony token to one independent of that library.
        public static Pie.Expressions.Token Convert(this Irony.Parsing.Token ironyToken)
        {
            if (ironyToken == null) return null;

            var t = new Pie.Expressions.Token();
            t.Column = ironyToken.Location.Column;
            t.Line = ironyToken.Location.Line;
            t.Position = ironyToken.Location.Position;
            return t;
        }
    }
}

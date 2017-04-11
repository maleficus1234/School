
namespace Pie.Expressions
{
    // Representation of a "token": for example a keyword, or identifier,
    // along with it's position in the source code.
    public class Token
    {
        public int Line;
        public int Column;
        public int Position;
    }
}

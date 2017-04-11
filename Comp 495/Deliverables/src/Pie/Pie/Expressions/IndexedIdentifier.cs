
namespace Pie.Expressions
{
    // Represents an identifier followed by an indexer: for example, foo[1]
    public class IndexedIdentifier
        :Expression
    {
        public string Name { get; set; }

        public IndexedIdentifier(Expression parentExpression, Token token)
            :base(parentExpression, token)
        {

        }
    }
}

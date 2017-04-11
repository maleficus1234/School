
namespace Pie.Expressions
{
    // Describes the declaration of an event (ex public event FooDelegate Foo)
    public class Event
        : Expression
    {
        // public, protected, private, or internal
        public Accessibility Accessibility { get; set; }
        // The name of the delegate: the event's type
        public string DelegateName { get; set; }
        // The name of the event.
        public string Name { get; set; }
        // Shared = static
        public bool IsShared { get; set; }

        public Event(Expression parentExpression, Token token)
            : base(parentExpression, token)
        {
            Accessibility = Expressions.Accessibility.Public;

            // Make sure to make the event static if it's in a module.
            if ((parentExpression as Class).IsModule)
                IsShared = true;
            else
                IsShared = false;
        }
    }
}

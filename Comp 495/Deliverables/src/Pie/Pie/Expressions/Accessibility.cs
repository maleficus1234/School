
namespace Pie.Expressions
{
    // Names the four different levels of accessibility for classes and their members.
    public enum Accessibility
    {
        Public,         // Visible anywhere: applies to classes and members
        Private,        // Not visible outside the class or by subclasses: applies to members
        Protected,      // Not visible outside the class but is by subclasses: applies to members
        Internal        // Not visible outside of the assembly: applies to classes and members.
    }
}

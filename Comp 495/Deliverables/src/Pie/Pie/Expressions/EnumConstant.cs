
namespace Pie.Expressions
{
    // Describes a cosntant in an enumeration
    public class EnumConstant
    {
        public string Name { get; private set; }
        public int Value { get; private set; }

        public EnumConstant(string name, int Value)
        {
            this.Name = name;
            this.Value = Value;
        }
    }
}

namespace Pie.Expressions
{
    // Names all of the binary operator types
    public enum BinaryOperatorType
    {
        Add,                        // +
        Subtract,                   // -
        Divide,                     // /
        Multiply,                   // *
        Modulo,                     // %
        LessOrEqual,                // <=
        Less,                       // <
        Equal,                      // ==
        NotEqual,                   // !=
        Greater,                    // >
        GreaterOrEqual,             // >=
        BitwiseShiftLeft,           // <<
        BitwiseShiftRight,          // >>
        BitwiseAnd,                 // &
        BitwiseOr,                  // |
        BitwiseXor,                 // ^
        LogicalOr,                  // ||
        LogicalAnd,                  // &&
        As                          // type case
    }
}

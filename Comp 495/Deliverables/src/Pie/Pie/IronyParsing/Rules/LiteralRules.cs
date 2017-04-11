using System;

using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules for all the literals
    internal class LiteralRules
    {
        // A number: a byte, short, int, long, float, double, decimal, ushort, uint, ulong
        public NumberLiteral NumberLiteral {get; private set;}
        // A character literal: 'c'
        public Terminal CharLiteral { get; private set; }
        // A string literal: "s"
        public Terminal StringLiteral { get; private set; }
        // A boolean literal: true or false
        public NonTerminal BoolLiteral { get; private set; }
        // A null literal
        public Terminal Null {get; private set;}

        PieGrammar grammar;

        public LiteralRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            NumberLiteral  = new NumberLiteral("number_literal", NumberOptions.AllowStartEndDot)
            {
                // Define types to assign if the parser can't determine.
                DefaultIntTypes = new[] { TypeCode.Int32 },
                DefaultFloatType = TypeCode.Double
            };

            // The pre and post fixes for different number types and formats.
            NumberLiteral.AddPrefix("0x", NumberOptions.Hex);
            NumberLiteral.AddSuffix("b", TypeCode.Byte);
            NumberLiteral.AddSuffix("B", TypeCode.Byte);
            NumberLiteral.AddSuffix("s", TypeCode.Int16);
            NumberLiteral.AddSuffix("S", TypeCode.Int16);
            NumberLiteral.AddSuffix("u", TypeCode.UInt32);
            NumberLiteral.AddSuffix("U", TypeCode.UInt32);
            NumberLiteral.AddSuffix("us", TypeCode.UInt16);
            NumberLiteral.AddSuffix("US", TypeCode.UInt16);
            NumberLiteral.AddSuffix("l", TypeCode.Int64);
            NumberLiteral.AddSuffix("L", TypeCode.Int64);
            NumberLiteral.AddSuffix("ul", TypeCode.UInt64);
            NumberLiteral.AddSuffix("UL", TypeCode.UInt64);
            NumberLiteral.AddSuffix("F", TypeCode.Single);
            NumberLiteral.AddSuffix("f", TypeCode.Single);
            NumberLiteral.AddSuffix("d", TypeCode.Double);
            NumberLiteral.AddSuffix("D", TypeCode.Double);
            NumberLiteral.AddSuffix("m", TypeCode.Decimal);
            NumberLiteral.AddSuffix("M", TypeCode.Decimal);

            StringLiteral = new StringLiteral("string_literal", "\"", StringOptions.AllowsAllEscapes);

            CharLiteral = new StringLiteral("char_literal", "'", StringOptions.IsChar);
            
            BoolLiteral = new NonTerminal("bool_literal");
            BoolLiteral.Rule = grammar.Keywords.True | grammar.Keywords.False;

            Null = grammar.ToTerm("null", "null_literal");
        }
    }
}

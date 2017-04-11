
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // The grammatical rules for operators.
    internal class OperatorRules
    {
        // Grammatical rules for the operators.

        public  NonTerminal ValidBinaryOperators { get; private set; }
        public  NonTerminal BinaryOperator { get; private set; }
        public NonTerminal ValidUnaryOperators { get; private set; }
        public NonTerminal UnaryOperator {get; private set;}
        public  Terminal Multiply { get; private set; }
        public  Terminal Divide { get; private set; }
        public Terminal Modulo { get; private set; }
        public  Terminal Add { get; private set; }
        public  Terminal Subtract { get; private set; }
        public  Terminal LessOrEqual { get; private set; }
        public Terminal Less { get; private set; }
        public Terminal Equal { get; private set; }
        public Terminal NotEqual { get; private set; }
        public Terminal Greater { get; private set; }
        public Terminal GreaterOrEqual { get; private set; }
        public NonTerminal Assignment { get; private set; }
        public Terminal EqualsAssignment { get; private set; }
        public Terminal BitwiseAnd { get; private set; }
        public Terminal BitwiseXor { get; private set; }
        public Terminal BitwiseOr { get; private set; }
        public Terminal BitwiseShiftLeft { get; private set; }
        public Terminal BitwiseShiftRight { get; private set; }
        public Terminal Not { get; private set; }
        public Terminal Negate { get; private set; }
        public Terminal As { get; private set; }
        public Terminal AddAssignment { get; private set; }
        public Terminal DivideAssignment { get; private set; }
        public Terminal MultiplyAssignment { get; private set; }
        public Terminal SubtractAssignment { get; private set; }
        public Terminal OrAssignment { get; private set; }
        public Terminal AndAssignment { get; private set; }
        public Terminal XorAssignment { get; private set; }

        PieGrammar grammar;

        public OperatorRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            ValidBinaryOperators = new NonTerminal("binary_operators");
            BinaryOperator = new NonTerminal("binary_operator");
            ValidUnaryOperators = new NonTerminal("valid_unary_operators");
            UnaryOperator = new NonTerminal("unary_operator");
            Multiply = grammar.ToTerm("*");
            Modulo = grammar.ToTerm("%");
            Divide = grammar.ToTerm("/");
            Add = grammar.ToTerm("+");
            Subtract = grammar.ToTerm("-");
            LessOrEqual = grammar.ToTerm("<=");
            GreaterOrEqual = grammar.ToTerm(">=");
            Greater = grammar.ToTerm(">");
            Less = grammar.ToTerm("<");
            Equal = grammar.ToTerm("==");
            NotEqual = grammar.ToTerm("!=");
            Assignment = new NonTerminal("assignment");
            EqualsAssignment = grammar.ToTerm("=");
            BitwiseAnd = grammar.ToTerm("&");
            BitwiseXor = grammar.ToTerm("^");
            BitwiseOr = grammar.ToTerm("|");
            BitwiseShiftLeft = grammar.ToTerm("<<");
            BitwiseShiftRight = grammar.ToTerm(">>");
            Not = grammar.ToTerm("!");
            Negate = grammar.ToTerm("-");
            As = grammar.ToTerm("as");
            AddAssignment = grammar.ToTerm("+=");
            SubtractAssignment = grammar.ToTerm("-=");
            DivideAssignment = grammar.ToTerm("/=");
            MultiplyAssignment = grammar.ToTerm("*=");
            OrAssignment = grammar.ToTerm("|=");
            AndAssignment = grammar.ToTerm("&=");
            XorAssignment = grammar.ToTerm("^=");
        }

        public  void Define()
        {
            // The rule describing all valid binary operators.
            ValidBinaryOperators.Rule = Add
                | Modulo
                | Divide
                | LessOrEqual
                | Less
                | Greater
                | GreaterOrEqual
                | Equal
                | NotEqual
                | grammar.Keywords.And
                | grammar.Keywords.Or
                | BitwiseShiftRight
                | BitwiseShiftLeft
                | BitwiseOr
                | BitwiseAnd
                | BitwiseXor
                | Subtract
                | Multiply
                | As;
            grammar.MarkTransient(ValidBinaryOperators);

            // Set operators precedence and associativity, in increasing order of precedence.
            int precedence = 1;          
            grammar.RegisterOperators(precedence += 1, Associativity.Right, "=", "+=", "-=", "/=", "*=", "|=", "&=", "^=");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "||");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "&&");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "|");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "^");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "&");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "==", "!=");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "<", ">", ">=", "<=");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "<<", ">>");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "+", "-");
            grammar.RegisterOperators(precedence += 1, Associativity.Left, "*", "/", "%");
            grammar.RegisterOperators(precedence += 1, Associativity.Right, "!", "-");
            grammar.RegisterOperators(precedence += 1, As); // Casting
           
            // Set comma and closing brace as operators so that lists can continue on the next line.
            grammar.RegisterOperators(0, grammar.ToTerm("}"));
            grammar.RegisterOperators(0, grammar.ToTerm(","));

            // An operator expression: two expressions with the operator between
            BinaryOperator.Rule = grammar.expression + ValidBinaryOperators + grammar.expression;

            // The list of valid assignment operators: +=, -=, *=, /=, |*, &=, ^=
            var validAssigments = new NonTerminal("valid_assigments");
            validAssigments.Rule = EqualsAssignment 
                | AddAssignment
                | SubtractAssignment
                | MultiplyAssignment
                | DivideAssignment
                | OrAssignment
                | AndAssignment
                | XorAssignment;
            grammar.MarkTransient(validAssigments);

            // Assignment expression is an expression that can be assigned to, the operator, and the assigned expression.
            Assignment.Rule = grammar.Assignable + validAssigments + grammar.expression + (grammar.Empty | grammar.Eos);

            // Valid unary operators.
            ValidUnaryOperators.Rule = Not | Negate;

            // Unary operator + expression
            UnaryOperator.Rule = ValidUnaryOperators + grammar.expression + (grammar.Empty | grammar.Eos);

            grammar.MarkReservedWords("As");
        }
    }
}

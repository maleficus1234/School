
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules describing definition of an enum.
    internal class EnumRules
    {
        // The overall enum grammatical rule.
        internal NonTerminal EnumDeclaration { get; private set; }

        PieGrammar grammar;

        public EnumRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            EnumDeclaration = new NonTerminal("enum_declaration");
        }

        public void Define()
        {
            // the valid modifiers for an enum: public or internal
            var validModifiers = new NonTerminal("enum_valid_modifiers");
            validModifiers.Rule = grammar.Keywords.Public
                | grammar.Keywords.Internal;

            grammar.MarkTransient(validModifiers);

            // Enums can have no or multiple modifiers
            var modifier = new NonTerminal("enum_modifiers");
            modifier.Rule = grammar.MakeStarRule(modifier, validModifiers);

            // A constant that simply a name, and that will have a value automatically assigned.
            var identifierConstant = new NonTerminal("identifier_constant");
            identifierConstant.Rule = grammar.Identifier + grammar.Eos;

            // A constant that is assigned a value by the programmer
            var assignmentConstant = new NonTerminal("assignment_constant");
            assignmentConstant.Rule = grammar.Identifier + grammar.ToTerm("=") + grammar.Literals.NumberLiteral + grammar.Eos;

            // List of valid members of an enum: the previous two constant rules.
            var enumValidMembers = new NonTerminal("enum_valid_members");
            enumValidMembers.Rule = identifierConstant | assignmentConstant;

            // The list of members in the enum.
            var enumMemberList = new NonTerminal("enum_member_list");
            enumMemberList.Rule = grammar.MakeStarRule(enumMemberList, enumValidMembers);

            // The enum body is an indent, the list of members, and a dedent.
            var enumBody = new NonTerminal("enum_body");
            enumBody.Rule = grammar.Indent + enumMemberList + grammar.Dedent;
            grammar.MarkTransient(enumBody);

            // The enum body is optional
            var enumListOpt = new NonTerminal("enum_list_opt");
            enumListOpt.Rule = grammar.Empty | enumBody;

            /* enum foo:
            *       bar
             *      bar2 = 24
             */
            EnumDeclaration.Rule = modifier
                + grammar.Keywords.Enum
                + grammar.Identifier
                + grammar.ToTerm(":")
                + grammar.Eos
                + enumListOpt;
        }
    }
}

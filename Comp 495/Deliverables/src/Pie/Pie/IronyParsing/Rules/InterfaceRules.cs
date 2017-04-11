
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Grammatical rules for an interface.
    internal class InterfaceRules
    {
        PieGrammar grammar;

        // The overall interface declaration rule.
        public NonTerminal InterfaceDeclaration { get; private set; }

        public InterfaceRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            InterfaceDeclaration = new NonTerminal("interface_declaration");
        }

        public void Define()
        {
            // A method interface contracted by the interface
            var methodInterface = new NonTerminal("method_interface");
            methodInterface.Rule = grammar.TwoIdentifiers
                + grammar.Classes.GenericTypeListOpt
                + grammar.OpenParenthese
                + grammar.MethodDeclarations.ParameterList
                + grammar.CloseParenthese
                + grammar.Eos;

            // A property interface contracted by the interface
            var propertyInterface = new NonTerminal("property_interface");
            propertyInterface.Rule = grammar.TwoIdentifiers
                + grammar.Eos;

            // Valid inteface members are the method and property interfaces
            var validInterfaceMembers = new NonTerminal("valid_interface_members");
            validInterfaceMembers.Rule = methodInterface | propertyInterface;

            // The member list can be zero or more of the previous
            var interfaceMembersList = new NonTerminal("interface_members");
            interfaceMembersList.Rule = grammar.MakeStarRule(interfaceMembersList, validInterfaceMembers);

            // The member body is an indent, the member list, and a dedent.
            var interfaceBody = new NonTerminal("interface_body");
            interfaceBody.Rule = grammar.Indent + interfaceMembersList + grammar.Dedent;

            // The body is optional.
            var interfaceBodyOpt = new NonTerminal("interface_body_opt");
            interfaceBodyOpt.Rule = interfaceBody | grammar.Empty;

            InterfaceDeclaration.Rule = grammar.Classes.Modifier
                + grammar.Keywords.Interface
                + grammar.Identifier
                + grammar.Classes.GenericTypeListOpt
                + grammar.Classes.BaseTypeListOpt
                + grammar.ToTerm(":")
                + grammar.Eos
                + interfaceBodyOpt;



        }
    }
}

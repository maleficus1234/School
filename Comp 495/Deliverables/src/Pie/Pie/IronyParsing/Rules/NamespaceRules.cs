
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // The grammatical rules defining a namespace.
    internal class NamespaceRules
    {
        // The import grammatical rule.
        public  NonTerminal Import { get; private set; }
        // The overall namespace declaration rule.
        public  NonTerminal NamespaceDeclaration { get; private set; }
        // The grammatical rules for the body of the namespace.
        public  NonTerminal NamespaceBody { get; private set; }
        // The list of valid members for a namespace.
        public  NonTerminal NamespaceValidMembers { get; private set; }
        // The list of members of a namespace.
        public  NonTerminal NamespaceMembersList { get; private set; }

        PieGrammar grammar;
        public NamespaceRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            Import = new NonTerminal("import");
            
            NamespaceDeclaration = new NonTerminal("namespace");
            NamespaceBody = new NonTerminal("namespace_body");
            NamespaceValidMembers = new NonTerminal("namespace_valid_members");
            NamespaceMembersList = new NonTerminal("namespace_members_list");
        }

        public  void Define()
        {
            // import System, System.IO
            Import.Rule = grammar.Keywords.Import + grammar.Lists.StarIdentifierList + grammar.Eos;

            // The namespace body is optional.
            var namespaceBodyOpt = new NonTerminal("namespace_body_opt");
            namespaceBodyOpt.Rule = (NamespaceBody | grammar.Empty);
            grammar.MarkTransient(NamespaceBody);

            // namespace foo:
            NamespaceDeclaration.Rule = grammar.Keywords.NamespaceKeyword 
                + grammar.Identifier
                + grammar.ToTerm(":")
                + grammar.Eos 
                + namespaceBodyOpt;

            // Valid namespace members are: namespaces, imports, enums, delegates, interfaces, classes
            NamespaceValidMembers.Rule = NamespaceDeclaration
                | grammar.Classes.ClassDeclaration
                | Import
                | grammar.Enums.EnumDeclaration
                | grammar.Delegates.DelegateDeclaration
                | grammar.Interfaces.InterfaceDeclaration;
            grammar.MarkTransient(NamespaceValidMembers);

            // The list of members in the namespace.
            NamespaceMembersList.Rule = grammar.MakeStarRule(NamespaceMembersList, NamespaceValidMembers);
            grammar.MarkTransient(NamespaceMembersList);
            
            // The body of the namespace is an indent, a member list, and a dedent.
            NamespaceBody.Rule =
                grammar.Indent
                + NamespaceMembersList
                + grammar.Dedent;
        }
    }
}

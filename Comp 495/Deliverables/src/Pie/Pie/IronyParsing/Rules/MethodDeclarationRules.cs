
using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // The grammatical rules for method declarations.
    internal class MethodDeclarationRules
    {
        // The overall grammatical rule defining a method.
        public NonTerminal MethodDeclaration { get; private set; }
        // The optional method body (Used elsewhere by thing like constuctors and if blocks)
        public NonTerminal OptionalMethodBody { get; private set; }
        // The modifiers for a method declaration
        public NonTerminal Modifier { get; private set; }
        // The parameter list for the method
        internal NonTerminal ParameterList;
        // A "member header": used to avoid confustion between class variables and methods.
        public NonTerminal MemberHeader { get; private set; }

        PieGrammar grammar;

        public MethodDeclarationRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            MethodDeclaration = new NonTerminal("method_declaration");
            OptionalMethodBody = new NonTerminal("optional_method_body");                      
            Modifier = new NonTerminal("method_modifiers");
            MemberHeader = new NonTerminal("member_header");            
            ParameterList = new NonTerminal("parameter_list");            
        }

        public void Define()
        {
            var validParameters = new NonTerminal("valid_parameters");
            var simpleParameter = new NonTerminal("simple_parameter");
            var outParameter = new NonTerminal("out_parameter");
            var arrayParameter = new NonTerminal("array_parameter");
            var genericParameter = new NonTerminal("generic_parameter");
            var methodStatementsList = new NonTerminal("method_statements_list");
            var methodBody = new NonTerminal("method_body");
            var genericTypeListEos = new NonTerminal("generic_type_list_eos");

            // Comma separated list of zero or more valid method parameters
            ParameterList.Rule = grammar.MakeStarRule(ParameterList, grammar.ToTerm(","), validParameters);
            // Valid parameters are simple ones: "int i", arrays "int [] i", directioned "ref int i" or generic "List{int} i"
            validParameters.Rule = simpleParameter | outParameter | arrayParameter | genericParameter;
            simpleParameter.Rule = grammar.Identifier + grammar.Identifier;
            outParameter.Rule = (grammar.Keywords.Out | grammar.Keywords.Ref) + grammar.Identifier + grammar.Identifier;
            arrayParameter.Rule = grammar.Identifier + grammar.ToTerm("[") + grammar.ToTerm("]") + grammar.Identifier;
            genericParameter.Rule = grammar.GenericIdentifier + grammar.Identifier;

            var validModifiers = new NonTerminal("valid_method_modifiers");
            validModifiers.Rule = grammar.Keywords.Public
                | grammar.Keywords.Internal
                | grammar.Keywords.Abstract
                | grammar.Keywords.Shared
                | grammar.Keywords.Protected
                | grammar.Keywords.Private
                | grammar.Keywords.Virtual
                | grammar.Keywords.Final
                | grammar.Keywords.Override;
            grammar.MarkTransient(validModifiers);

            // Modifiers are zero or more from the list of valid ones.
            Modifier.Rule = grammar.MakeStarRule(Modifier, validModifiers);

            // "member header" is a Modifier followed by two identifiers.
            MemberHeader.Rule = Modifier + grammar.TwoIdentifiers;

            // Method body is a list of statements between an indent and a dedent.
            methodBody.Rule = grammar.Indent + methodStatementsList + grammar.Dedent;
            OptionalMethodBody.Rule = grammar.Empty | methodBody;

            // The list of statements in a method is a list of zero or more expressions.
            methodStatementsList.Rule = grammar.MakeStarRule(methodStatementsList, grammar.expression);

            MethodDeclaration.Rule =
                 MemberHeader
                + grammar.Classes.GenericTypeListOpt
                + grammar.OpenParenthese 
                + ParameterList
                + grammar.CloseParenthese
                + grammar.ToTerm(":")
                + grammar.Eos
                + OptionalMethodBody;
        }
    }
}


using Irony.Parsing;

namespace Pie.IronyParsing.Rules
{
    // Wraps up class-specific grammatical rules. Those that are public are used elsewhere.
    internal class ClassRules
    {
        // The class modifiers rule: zero or more of public, private, protected, internal, shared, final, partial, abstract
        public  NonTerminal Modifier { get; private set; }
        // Generic type list rule: "{T,K}"
        public  NonTerminal GenericTypeList { get; private set; }
        // Optional generic type list.
        public NonTerminal GenericTypeListOpt { get; private set; }
        // The valid modifiers for a class variables (also used by delegates): public, private, protected, shared, final
        public NonTerminal ClassVariableModifierList { get; private set; }
        // Optional base type list
        public NonTerminal BaseTypeListOpt { get; private set; }
        // The root rule for class declarations.
        public NonTerminal ClassDeclaration;

        PieGrammar grammar;

        // Declare the public rules.
        public ClassRules(PieGrammar grammar)
        {
            this.grammar = grammar;

            
            Modifier = new NonTerminal("class_modifier");

            ClassVariableModifierList = new NonTerminal("ClassVariableModifier");
            GenericTypeListOpt = new NonTerminal("generic_list_opt");
            ClassDeclaration = new NonTerminal("class_declaration");

            GenericTypeList = new NonTerminal("generic_type_list");
            
            BaseTypeListOpt = new NonTerminal("base_list_opt");
        }

        // Define the rules (declaration and definition must be separate).
        public  void Define() 
        {
            // List of base types
            var baseTypeList = new NonTerminal("base_type_list");
            // List of members in the class body
            var membersList = new NonTerminal("class_members_list");
            // The members that may go into a member list.
            var validMembers = new NonTerminal("class_valid_members");
            // The optional body of the class
            var memberBodyOpt = new NonTerminal("member_body_opt");
            // The modifiers that may make up a class.
            var validClassModifiers = new NonTerminal("valid_class_modifiers");
            

            // The rule describing which modifiers can be applied to a class variable.
            var classVariableValidModifers = new NonTerminal("classVariableValidModifiers");
            classVariableValidModifers.Rule = grammar.Keywords.Public
                | grammar.Keywords.Private
                | grammar.Keywords.Protected
                | grammar.Keywords.Internal
                | grammar.Keywords.Final
                | grammar.Keywords.Shared;
            
            // A list rule is needed as there can be more than one modifier.
            ClassVariableModifierList.Rule = grammar.MakeStarRule(ClassVariableModifierList, classVariableValidModifers);

            // The rule to declare a class varible: public int foo
            var classVariable = new NonTerminal("class_variable");
            classVariable.Rule = grammar.MethodDeclarations.MemberHeader + grammar.Eos;

            // The rule describing which modifiers can be used for a class declaration
            validClassModifiers.Rule = grammar.Keywords.Public 
                | grammar.Keywords.Internal 
                | grammar.Keywords.Final 
                | grammar.Keywords.Partial 
                | grammar.Keywords.Abstract;
            grammar.MarkTransient(validClassModifiers);

            // A list rule since there can be more than on modifier.
            Modifier.Rule = grammar.MakeStarRule(Modifier, validClassModifiers);

            // Could be a class, struct, or module
            var classStructOrModule = new NonTerminal("class_or_module");
            classStructOrModule.Rule = grammar.Keywords.Class | grammar.Keywords.Module | grammar.Keywords.Struct;
            grammar.MarkTransient(classStructOrModule);

            // (basetype1, basetype2)
            baseTypeList.Rule = grammar.OpenParenthese + grammar.Lists.PlusIdentifierList + grammar.CloseParenthese;
            
            // Base type list is optional: it may be empty
            BaseTypeListOpt.Rule = grammar.Empty | baseTypeList;

            // {T,L}
            GenericTypeList.Rule = grammar.ToTerm("{") + grammar.Lists.PlusIdentifierList + grammar.ToTerm("}");
            
            // Generic type list is optional.
            GenericTypeListOpt.Rule = grammar.Empty | GenericTypeList;

            // Class body is a list of members between an indent and a dedent
            var memberBody = new NonTerminal("member_body");
            memberBody.Rule = (grammar.Indent + membersList + grammar.Dedent);
            grammar.MarkTransient(memberBody);

            // member list is optional: it may be empty.
            var optionalMembersList = new NonTerminal("optional_members_list");
            optionalMembersList.Rule = grammar.Empty | membersList;

            // Body of the class is optional
            memberBodyOpt.Rule = grammar.Empty | memberBody;

            /* class foo{T,K}(basetype1,basetype2}:
             *      memberlist
             */
            ClassDeclaration.Rule = Modifier
                + classStructOrModule
                + grammar.Identifier
                + GenericTypeListOpt
                + BaseTypeListOpt
                + grammar.ToTerm(":")
                + grammar.Eos 
                + memberBodyOpt;
            
            // Valid class members are methods, constructors, variables, properties, and events.
            validMembers.Rule = grammar.MethodDeclarations.MethodDeclaration
                | classVariable
                | grammar.Constructors.ConstructorDeclaration
                | grammar.Properties.PropertyDeclaration
                | grammar.Delegates.EventMember;
            membersList.Rule = grammar.MakeStarRule(membersList, validMembers);
        }
    }
}

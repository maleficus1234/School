using System;
using System.CodeDom;

using Pie.Expressions;

namespace Pie.CodeDom
{ 
    internal class IndexedIdentifierEmitter
    {
        // Generates a codedom indexed identifier: one that is an identifier followed by an indexer: ex foo[1].
        public static CodeExpression Emit(IndexedIdentifier indexedIdentifier)
        {
            // Create the codedom indexer expression
            var codeIndex = new CodeIndexerExpression();

            // Set the object that is being indexed.
            codeIndex.TargetObject = new CodeVariableReferenceExpression(indexedIdentifier.Name);

            // Set the expression that is generating the index.
            codeIndex.Indices.Add(CodeDomEmitter.EmitCodeExpression(indexedIdentifier.ChildExpressions[0]));

            return codeIndex;
        }
    }
}

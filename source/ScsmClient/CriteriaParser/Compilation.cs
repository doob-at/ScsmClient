using System.Collections.Immutable;
using System.Linq;
using ScsmClient.CriteriaParser.Binding;
using ScsmClient.CriteriaParser.Syntax;

namespace ScsmClient.CriteriaParser
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }

        public EvaluationResult Evaluate()
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(SyntaxTree.Root);

            var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate().ToString();

            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}
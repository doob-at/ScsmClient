using System.Collections.Immutable;
using System.Linq;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.CriteriaParser.Binding;
using ScsmClient.CriteriaParser.Syntax;

namespace ScsmClient.CriteriaParser
{
    public sealed class Compilation
    {
        private readonly SCSMClient _scsmClient;

        public Compilation(SyntaxTree syntaxTree, SCSMClient scsmClient)
        {
            _scsmClient = scsmClient;
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }

        public EvaluationResult Evaluate(ManagementPackTypeProjection managementPackTypeProjection)
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(SyntaxTree.Root);

            var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var evaluator = new Evaluator(boundExpression, _scsmClient);
            var value = evaluator.Evaluate(managementPackTypeProjection);

            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
        public EvaluationResult Evaluate(ManagementPackClass baseManagementPackClass)
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(SyntaxTree.Root);

            var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var evaluator = new Evaluator(boundExpression, _scsmClient);
            var value = evaluator.Evaluate(baseManagementPackClass);

            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}
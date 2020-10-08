using System.Collections.Immutable;
using ScsmClient.Helper;

namespace ScsmClient.CriteriaParser
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, SimpleXml value)
        {
            Diagnostics = diagnostics;
            Value = value;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public SimpleXml Value { get; }
    }
}
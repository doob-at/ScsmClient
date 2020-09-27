using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    public sealed class NullExpressionSyntax : ExpressionSyntax
    {

        public NullExpressionSyntax(SourceText sourceText, SyntaxToken literalToken): base(sourceText)
        {
            NullToken = literalToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NullExpression;
        public SyntaxToken NullToken { get; }
        //public object Value { get; }
    }
}
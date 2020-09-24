using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        internal LiteralExpressionSyntax(SourceText sourceText, SyntaxToken literalToken)
            : this(sourceText, literalToken, literalToken.Value)
        {
        }

        internal LiteralExpressionSyntax(SourceText sourceText, SyntaxToken literalToken, object value)
            : base(sourceText)
        {
            LiteralToken = literalToken;
            Value = value;
        }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }
    }
}
using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    public sealed class PropertyExpressionSyntax : ExpressionSyntax
    {
        internal PropertyExpressionSyntax(SourceText sourceText, SyntaxToken literalToken)
            : this(sourceText, literalToken, literalToken.Value)
        {
        }

        internal PropertyExpressionSyntax(SourceText sourceText, SyntaxToken literalToken, object value)
            : base(sourceText)
        {
            LiteralToken = literalToken;
            Value = value;
        }

        public override SyntaxKind Kind => SyntaxKind.PropertyExpression;
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }
    }
}
using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SourceText sourceText, SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken): base(sourceText)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParenthesisToken { get; }
    }
}
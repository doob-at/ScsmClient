using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        internal UnaryExpressionSyntax(SourceText sourceText, SyntaxToken operatorToken, ExpressionSyntax operand)
            : base(sourceText)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Operand { get; }
    }
}
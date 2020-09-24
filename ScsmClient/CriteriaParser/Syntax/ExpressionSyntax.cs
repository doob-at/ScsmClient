using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    public abstract class ExpressionSyntax : SyntaxNode
    {
        private protected ExpressionSyntax(SourceText sourceText)
            : base(sourceText)
        {
        }
    }
}
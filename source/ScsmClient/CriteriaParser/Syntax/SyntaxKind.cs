namespace ScsmClient.CriteriaParser.Syntax
{
    public enum SyntaxKind
    {
        
        BadToken,
        
        // Tokens
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        //PlusToken,
        MinusToken,
        //StarToken,
        //SlashToken,
        BangToken,
        EqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,
        PropertyToken,
        StringToken,
        LikeToken,
        BangLikeToken,
        GreaterToken,
        LessToken,
        GreaterOrEqualsToken,
        LessOrEqualsToken,

        // Keywords
        FalseKeyword,
        TrueKeyword,
        NullKeyword,

        // Expressions
        LiteralExpression,
        PropertyExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        NullExpression,
    }
}
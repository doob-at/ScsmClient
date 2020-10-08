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
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        NullExpression,
    }
}
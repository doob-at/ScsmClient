using System.Text;
using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser.Syntax
{
    internal sealed class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SourceText _text;

        private int _position;

        private int _start;
        private SyntaxKind _kind;
        private object _value;

        public Lexer(SourceText text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        private char Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _text.Length)
                return '\0';

            return _text[index];
        }

        public SyntaxToken Lex()
        {
            var tokenStart = _position;

            ReadToken();

            var tokenKind = _kind;
            var tokenValue = _value;
            var tokenLength = _position - _start;

            var tokenText = SyntaxFacts.GetText(tokenKind);
            if (tokenText == null)
                tokenText = _text.ToString(tokenStart, tokenLength);

            return new SyntaxToken(_text, _kind, _start, tokenText, _value);
        }

        private void ReadToken()
        {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    _kind = SyntaxKind.EndOfFileToken;
                    break;
                //case '+':
                //    _kind = SyntaxKind.PlusToken;
                //    _position++;
                //    break;
                case '-':
                    {
                        _position++;
                        if (ContinueWith("and"))
                        {
                            _kind = SyntaxKind.AmpersandAmpersandToken;
                        }
                        else if (ContinueWith("or"))
                        {
                            _kind = SyntaxKind.PipePipeToken;
                        }
                        else if (ContinueWith("eq"))
                        {
                            _kind = SyntaxKind.EqualsEqualsToken;
                        }
                        else if (ContinueWith("ne"))
                        {
                            _kind = SyntaxKind.BangEqualsToken;
                        }
                        else if (ContinueWith("gt"))
                        {
                            _kind = SyntaxKind.GreaterToken;
                        }
                        else if (ContinueWith("lt"))
                        {
                            _kind = SyntaxKind.LessToken;
                        }
                        else if (ContinueWith("ge"))
                        {
                            _kind = SyntaxKind.GreaterOrEqualsToken;
                        }
                        else if (ContinueWith("le"))
                        {
                            _kind = SyntaxKind.LessOrEqualsToken;
                        }
                        else if (ContinueWith("like"))
                        {
                            _kind = SyntaxKind.LikeToken;
                        }
                        else if (ContinueWith("notlike"))
                        {
                            _kind = SyntaxKind.BangLikeToken;
                        }
                        else
                        {
                            _kind = SyntaxKind.MinusToken;

                        }

                        break;
                    }

                //case '*':
                //    _kind = SyntaxKind.StarToken;
                //    _position++;
                //    break;
                //case '/':
                //    _kind = SyntaxKind.SlashToken;
                //    _position++;
                //    break;
                case '(':
                    _kind = SyntaxKind.OpenParenthesisToken;
                    _position++;
                    break;
                case ')':
                    _kind = SyntaxKind.CloseParenthesisToken;
                    _position++;
                    break;
                case '&':
                    if (Lookahead == '&')
                    {
                        _kind = SyntaxKind.AmpersandAmpersandToken;
                        _position += 2;
                        break;
                    }
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        _kind = SyntaxKind.PipePipeToken;
                        _position += 2;
                        break;
                    }
                    break;
                case '=':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.EqualsToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.EqualsEqualsToken;
                    }
                    break;
                case '!':
                    _position++;
                    if (ContinueWith("like"))
                    {
                        _kind = SyntaxKind.BangLikeToken;
                    }
                    else if (ContinueWith("="))
                    {
                        _kind = SyntaxKind.BangEqualsToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.BangToken;
                    }
                    break;
                case '>':
                    {
                        _position++;
                        if (ContinueWith("="))
                        {
                            _kind = SyntaxKind.GreaterOrEqualsToken;
                        }
                        else
                        {
                            _kind = SyntaxKind.GreaterToken;
                        }
                        
                        break;
                    }
                case '<':
                    {
                        _position++;
                        if (ContinueWith("="))
                        {
                            _kind = SyntaxKind.LessOrEqualsToken;
                        }
                        else
                        {
                            _kind = SyntaxKind.LessToken;
                        }

                        break;
                    }
                case '\'':
                case '"':
                    ReadString();
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumberToken();
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpace();
                    break;
                case '@':
                    ReadProperty();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    }
                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpace();
                    }
                    else
                    {
                        var span = new TextSpan(_position, 1);
                        var location = new TextLocation(_text, span);
                        _diagnostics.ReportBadCharacter(location, Current);
                        _position++;
                    }
                    break;
            }

        }

        private void ReadProperty()
        {
            _position++;

            while (!char.IsWhiteSpace(Current))
            {
                _position++;
            }
            //while (char.IsLetter(Current) ||char.IsNumber(Current) || Current == '.' || Current == ':' || Current == '!' || Current == '_')
            //    _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _kind = SyntaxKind.PropertyToken;
            _value = text;
        }

        private bool ContinueWith(string value)
        {
            var length = value.Length;

            for (int i = 0; i < length; i++)
            {
                var nextchar = _text[_position + i];
                if (nextchar != value[i])
                {
                    return false;
                }
            }

            _position += length;

            return true;
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            if (!int.TryParse(text, out var value))
            {
                var span = new TextSpan(_start, length);
                var location = new TextLocation(_text, span);
                _diagnostics.ReportInvalidNumber(location, text, typeof(int));

            }


            _value = value;
            _kind = SyntaxKind.NumberToken;
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current) || Current == '.' || Current == ':' || Current == '!')
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _kind = SyntaxFacts.GetKeywordKind(text);
            _value = text;
        }

        private void ReadString()
        {
            // Skip the current quote
            _position++;

            var sb = new StringBuilder();
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        var span = new TextSpan(_start, 1);
                        var location = new TextLocation(_text, span);
                        _diagnostics.ReportUnterminatedString(location);
                        done = true;
                        break;
                    case '"':
                        if (Lookahead == '"')
                        {
                            sb.Append(Current);
                            _position += 2;
                        }
                        else
                        {
                            _position++;
                            done = true;
                        }
                        break;
                    case '\'':
                        if (Lookahead == '\'')
                        {
                            sb.Append(Current);
                            _position += 2;
                        }
                        else
                        {
                            _position++;
                            done = true;
                        }
                        break;
                    default:
                        sb.Append(Current);
                        _position++;
                        break;
                }
            }

            _kind = SyntaxKind.StringToken;
            _value = sb.ToString();
        }

    }
}
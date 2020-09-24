using System;
using System.Collections;
using System.Collections.Generic;
using ScsmClient.CriteriaParser.Syntax;
using ScsmClient.CriteriaParser.Text;

namespace ScsmClient.CriteriaParser
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(IEnumerable<Diagnostic> diagnostics)
        {
            _diagnostics.AddRange(diagnostics);
        }

        private void ReportError(TextLocation location, string message)
        {
            var diagnostic = Diagnostic.Error(location, message);
            _diagnostics.Add(diagnostic);
        }

        private void ReportWarning(TextLocation location, string message)
        {
            var diagnostic = Diagnostic.Warning(location, message);
            _diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextLocation location, string text, Type type)
        {
            var message = $"The number {text} isn't valid {type}.";
            ReportError(location, message);
        }

        public void ReportBadCharacter(TextLocation location, char character)
        {
            var message = $"Bad character input: '{character}'.";
            ReportError(location, message);
        }

        public void ReportUnterminatedString(TextLocation location)
        {
            var message = "Unterminated string literal.";
            ReportError(location, message);
        }

        public void ReportUnterminatedMultiLineComment(TextLocation location)
        {
            var message = "Unterminated multi-line comment.";
            ReportError(location, message);
        }

        public void ReportUnexpectedToken(TextLocation location, SyntaxKind actualKind, SyntaxKind expectedKind)
        {
            var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
            ReportError(location, message);
        }

        public void ReportUndefinedUnaryOperator(TextLocation location, string operatorText, Type operandType)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type '{operandType}'.";
            ReportError(location, message);
        }

        public void ReportUndefinedBinaryOperator(TextLocation location, string operatorText, Type leftType, Type rightType)
        {
            var message = $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.";
            ReportError(location, message);
        }

      
    }
}

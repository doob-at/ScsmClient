using System;
using ScsmClient.CriteriaParser.Binding;

namespace ScsmClient.CriteriaParser
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        //private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root)
        {
            _root = root;
            //_variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)node);
                case BoundNodeKind.NullExpression:
                    return EvaluateNullExpression((BoundNullExpression)node);
                //case BoundNodeKind.VariableExpression:
                //    return this.EvaluateVariableExpression((BoundVariableExpression)node);
                //case BoundNodeKind.AssignmentExpression:
                //    return EvaluateAssignmentExpression((BoundAssignmentExpression)node);
                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression)node);
                case BoundNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((BoundBinaryExpression)node);
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }

        private static object EvaluateNullExpression(BoundNullExpression n)
        {
            return new NullValue();
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            switch (u.Op.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;
                default:
                    throw new Exception($"Unexpected unary operator {u.Op}");
            }
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            switch (b.Op.Kind)
            {
                //case BoundBinaryOperatorKind.Addition:
                //    return (int)left + (int)right;
                //case BoundBinaryOperatorKind.Subtraction:
                //    return (int)left - (int)right;
                //case BoundBinaryOperatorKind.Multiplication:
                //    return (int)left * (int)right;
                //case BoundBinaryOperatorKind.Division:
                    //return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return Expression(And(left, right));
                case BoundBinaryOperatorKind.LogicalOr:
                    return Expression(Or(left, right));
                case BoundBinaryOperatorKind.Equals:
                {
                    if (right is NullValue)
                    {
                        return Expression(UnaryExpression(left, "IsNull"));
                    }
                    return Expression(SimpleExpression(left, "Equal", right));
                }

                case BoundBinaryOperatorKind.NotEquals:
                {
                    if (right is NullValue)
                    {
                        return Expression(UnaryExpression(left, "IsNotNull"));
                    }
                    return Expression(SimpleExpression(left, "NotEqual", right));
                }
                    
                case BoundBinaryOperatorKind.Like:
                    return Expression(SimpleExpression(left, "Like", right));
                case BoundBinaryOperatorKind.NotLike:
                    return Expression(SimpleExpression(left, "NotLike", right));
                default:
                    throw new Exception($"Unexpected binary operator {b.Op}");
            }
        }


        private string And(object left, object right)
        {
            return $"<And>{left}{right}</And>";
        }

        private string Or(object left, object right)
        {
            return $"<Or>{left}{right}</Or>";
        }

        private string SimpleExpression(object left, string @operator, object right)
        {
            return $"<SimpleExpression><ValueExpressionLeft><Property>{left}</Property></ValueExpressionLeft><Operator>{@operator}</Operator><ValueExpressionRight><Value>{right}</Value></ValueExpressionRight></SimpleExpression>";
        }

        private string UnaryExpression(object left, string @operator)
        {
            return $"<UnaryExpression><ValueExpression><Property>{left}</Property></ValueExpression><Operator>{@operator}</Operator></UnaryExpression>";
        }

        private string Expression(string simpleExpression)
        {
            return $"<Expression>{simpleExpression}</Expression>";
        }
    }
}
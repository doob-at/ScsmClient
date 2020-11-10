using System;

namespace ScsmClient.CriteriaParser.Binding
{
    internal sealed class BoundPropertyExpression : BoundExpression
    {
        public BoundPropertyExpression(object value)
        {
            Value = value;
        }

        public override BoundNodeKind Kind => BoundNodeKind.PropertyExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
    }
}

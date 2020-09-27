using System;

namespace ScsmClient.CriteriaParser.Binding
{
    internal sealed class BoundNullExpression : BoundExpression
    {
        public BoundNullExpression()
        {
            
        }

        public override BoundNodeKind Kind => BoundNodeKind.NullExpression;
        public override Type Type => typeof(NullValue);

    }
}

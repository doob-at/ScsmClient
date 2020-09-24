using System;

namespace ScsmClient.CriteriaParser.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}

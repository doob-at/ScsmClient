using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScsmClient.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class AsIdAttribute: Attribute
    {
        public Guid Id { get; }

        public AsIdAttribute(string id)
        {
            Id = Guid.Parse(id);
        }
    }
}

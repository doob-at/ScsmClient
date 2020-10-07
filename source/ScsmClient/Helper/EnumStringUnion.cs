using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScsmClient.Helper
{
    public class EnumStringUnion
    {
        private string EnumString { get; }

        private EnumStringUnion(Enum @enum)
        {
            EnumString = @enum.ToString();
        }


        public static implicit operator EnumStringUnion(Enum @enum)
        {
            return new EnumStringUnion(@enum);
        }

        public static implicit operator String(EnumStringUnion enumString)
        {
            return enumString.EnumString;
        }
    }
}

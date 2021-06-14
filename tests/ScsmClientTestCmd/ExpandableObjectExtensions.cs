using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doob.Reflectensions;


namespace ScsmClientTestCmd
{
    public static class ExpandableObjectExtensions
    {
        public static Dictionary<string, object> CreateUpdateDictionary<T>(this T expandableObject, Action<T> update) where T: ExpandableObject
        {
            var before = expandableObject.AsDictionary();
            update(expandableObject);
            var after = expandableObject.AsDictionary();

            var nDict = new Dictionary<string, object>();
            foreach (var key in before.Keys)
            {
                if (after.ContainsKey(key))
                {
                    if (after[key] != before[key])
                    {
                        nDict.Add(key, after[key]);
                    }
                }
            }

            foreach (var key in after.Keys)
            {
                if (!before.ContainsKey(key))
                {
                    nDict[key] = after[key];
                }
            }

            return nDict;
        }
    }
}

using System.Collections.Generic;
using System.Threading;

namespace ScsmClient.SharedModels.Models
{
    public class CreateOptions
    {
        public int? BatchSize { get; set; }

        public Dictionary<string, string> BuildCacheForObjects { get; set; }

    }
}

using System.Collections.Generic;

namespace ScsmClient.SharedModels.Models
{
    public class RetrievalOptions
    {
        public int? MaxResultCount { get; set; }
        public List<string> PropertiesToLoad { get; set; }

        public int? ReferenceLevels { get; set; }
    }
}

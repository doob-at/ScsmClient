using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseIT.SCSM.Client.Model
{
    public class EnterpriseManagementObjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public List<Guid> ManagementPackClassIds { get; set; }
        public Guid LeastDerivedNonAbstractManagementPackClassId { get; set; }
        public DateTime TimeAdded { get; set; }
        public virtual Guid? LastModifiedBy { get; set; }
        public DateTime LastModified { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }
}

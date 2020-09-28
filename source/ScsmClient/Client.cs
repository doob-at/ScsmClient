using System.Net;
using Microsoft.EnterpriseManagement;
using ScsmClient.Helper;
using ScsmClient.Operations;

namespace ScsmClient
{
    public class SCSMClient
    {

        public string ServerName { get; }
        public NetworkCredential Credential { get; }

        public bool IsConnected => ManagementGroup?.IsConnected ?? false;

        public EnterpriseManagementGroup ManagementGroup { get; private set; }
        public SCSMClient(string serverName)
        {
            ServerName = serverName;
            Init();
        }

        public SCSMClient(string serverName, NetworkCredential credential)
        {
            ServerName = serverName;
            Credential = CredentialsHelper.BuildSecureCredential(credential);
            Init();
        }

        private void Init()
        {

            var settings = new EnterpriseManagementConnectionSettings(ServerName);

            if (Credential != null)
            {
                settings.UserName = Credential.UserName;
                settings.Domain = Credential.Domain;
                settings.Password = Credential.SecurePassword;
            }
            

            ManagementGroup = new EnterpriseManagementGroup(settings);
        }


        private ManagementPackOperations _managementPackOperations;

        public ManagementPackOperations ManagementPack()
        {
            return _managementPackOperations = _managementPackOperations ?? new ManagementPackOperations(this);
        }


        private CriteriaOperations _criteriaOperations;

        public CriteriaOperations Criteria()
        {
            return _criteriaOperations = _criteriaOperations ?? new CriteriaOperations(this);
        }

        private TypeProjectionOperations _typeProjectionOperations;
        public TypeProjectionOperations TypeProjection()
        {
            return _typeProjectionOperations = _typeProjectionOperations ?? new TypeProjectionOperations(this);
        }

        private ClassOperations _classOperations;
        public ClassOperations Class()
        {
            return _classOperations = _classOperations ?? new ClassOperations(this);
        }

        private ObjectOperations _objectOperations;
        public ObjectOperations Object()
        {
            return _objectOperations = _objectOperations ?? new ObjectOperations(this);
        }
    }
}

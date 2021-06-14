using System.Net;
using doob.Reflectensions;
using Microsoft.EnterpriseManagement;
using ScsmClient.Helper;
using ScsmClient.JsonConverters;
using ScsmClient.Operations;

namespace ScsmClient
{
    public class SCSMClient
    {

        public string ServerName { get; }
        public NetworkCredential Credential { get; }

        public bool IsConnected => _enterpriseManagementGroup?.IsConnected ?? false;

        private EnterpriseManagementGroup _enterpriseManagementGroup;

        public EnterpriseManagementGroup ManagementGroup
        {
            get
            {

                if (!IsConnected)
                {
                    _enterpriseManagementGroup?.Reconnect();
                }

                return _enterpriseManagementGroup;
            }
            private set => _enterpriseManagementGroup = value;
        }

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

        public SCSMClient(EnterpriseManagementGroup enterpriseManagementGroup)
        {
            ServerName = enterpriseManagementGroup.ConnectionSettings.ServerName;
            Json.Converter.RegisterJsonConverter<ManagementGroupConverter>();
            ManagementGroup = enterpriseManagementGroup;
        }

        private void Init()
        {
            Json.Converter.RegisterJsonConverter<ManagementGroupConverter>();

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

        public CriteriaOperations Criteria() => _criteriaOperations = _criteriaOperations ?? new CriteriaOperations(this);
        

        private TypeProjectionOperations _typeProjectionOperations;
        public TypeProjectionOperations TypeProjection() => _typeProjectionOperations = _typeProjectionOperations ?? new TypeProjectionOperations(this);
        

        private TypeOperations _typeOperations;
        public TypeOperations Types() => _typeOperations = _typeOperations ?? new TypeOperations(this);
        

        private ObjectOperations _objectOperations;
        public ObjectOperations Object() => _objectOperations = _objectOperations ?? new ObjectOperations(this);
        

        private TemplateOperations _templateOperations;
        public TemplateOperations Template() => _templateOperations = _templateOperations ?? new TemplateOperations(this);

        private EnumerationOperations _enumerationOperations;
        public EnumerationOperations Enumeration() => _enumerationOperations = _enumerationOperations ?? new EnumerationOperations(this);

        private IncidentOperations _incidentOperations;
        public IncidentOperations Incident() => _incidentOperations = _incidentOperations ?? new IncidentOperations(this);

        private ServiceRequestOperations _serviceRequestOperations;
        public ServiceRequestOperations ServiceRequest() => _serviceRequestOperations = _serviceRequestOperations ?? new ServiceRequestOperations(this);

        private ChangeRequestOperations _changeRequestOperations;
        public ChangeRequestOperations ChangeRequest() => _changeRequestOperations = _changeRequestOperations ?? new ChangeRequestOperations(this);

        private RelationsOperations _relationsOperations;
        public RelationsOperations Relations() => _relationsOperations = _relationsOperations ?? new RelationsOperations(this);

        private ScsmObjectOperations _scsmObjectOperations;
        public ScsmObjectOperations ScsmObject() => _scsmObjectOperations = _scsmObjectOperations ?? new ScsmObjectOperations(this);

        private AttachmentOperations _attachmentOperations;
        public AttachmentOperations Attachment() => _attachmentOperations = _attachmentOperations ?? new AttachmentOperations(this);

    }
}

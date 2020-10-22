using System;
using System.Linq;
using System.Net;
using ScsmClient;

namespace TestScsmCriteriaBuilder
{
    class Program
    {

        static void Main(string[] args)
        {
            //Main1(args);

            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.20", creds);

            //var filterobj = "zOKZ != null && zName like '%Polizei%'";

            //var okzs = scsmClient.Object().GetObjectsByClassName("zOrganisationseinheit", null).ToList();

            //var filter1 = "zFirstname == Bernhard && zLastname == Windisch";


           // var objs = scsmClient.TypeProjection().GetObjectProjectionObjects("zTP_zBenutzer_zAccount", filter1).ToList();

           //var incs = scsmClient.Incident().GetByCriteria(null);

           // var filter2 = "G:Id == '3cd8c933-995e-2e21-f145-43a3c06fd585'";
           // var filter3 = "G:LastModified > '19.03.2020 9:46' && G:LastModified < '19.03.2020 10:00'";
           // var filter4 = "G:LastModified == '19.03.2020 9:46:14.133'";
           // var filter5 = "G:LastModified -gt '19.03.2020 9:46:14'";
           // var filter6 = "";

            var objs2 = scsmClient.ScsmObject().GetObjectsByTypeName("zBenutzerBase", null).ToList();

        }



        static void Main1(string[] args)
        {

            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.20", creds);

            //var incidentClass = scsmClient.Types().GetClassByName("zOrganisationseinheit");

            var filter1 = "G:Id == 'd8e70ac7-3a63-8e80-5fca-4ebf6ab682de'";
            var filter2 = "Id == 658";
            var filter3 = "Workitem.Id == '658'";
            var filter4 = "System.WorkItem.TroubleTicket.Id == '658'";
            var filter5 = "Id == '658'";
            var filterByGenericId = "Id == 651";
            //var filter2 = "zOKZ like '%BWI%'";



            //var syntaxTree = SyntaxTree.Parse(filter);
            //var compilation = new Compilation(syntaxTree, scsmClient);
            //var result = compilation.Evaluate(incidentClass);
            //var resultValue =  result.Value;


            //var crit = resultValue.ToString();


            //ManagementPackTypeProjectionCriteria projectionSelectionCriteria = new ManagementPackTypeProjectionCriteria("Name = 'System.WorkItem.Incident.View.ProjectionType'");
            //var tp = scsmClient.TypeProjection().GetTypeProjection(WellKnown.Incident.ProjectionType);
            //ObjectProjectionCriteria objProjectionCriteria = new ObjectProjectionCriteria(crit, tp, scsmClient.ManagementGroup);


            var searchbyidcrit = @"<Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">
                <Expression>
    <SimpleExpression>
      <ValueExpressionLeft>
        <GenericProperty>Id</GenericProperty>
      </ValueExpressionLeft>
      <Operator>Equal</Operator>
      <ValueExpressionRight>
        <Value>d8e70ac7-3a63-8e80-5fca-4ebf6ab682de</Value>
      </ValueExpressionRight>
    </SimpleExpression>
  </Expression>
</Criteria>";


            var obj = scsmClient.ScsmObject().GetObjectById(Guid.Parse("d8e70ac7-3a63-8e80-5fca-4ebf6ab682de"));
            var inc = scsmClient.Incident().GetByCriteria(filter4).ToList().FirstOrDefault();

            //var found = scsmClient.TypeProjection().GetObjectProjectionReader(objProjectionCriteria, ObjectQueryOptions.Default);

            var incdisp = inc.DisplayName;

        }
    }
}

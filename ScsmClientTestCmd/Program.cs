using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Expandable.ExtensionMethods;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions;
using ScsmClient;
using ScsmClient.ExtensionMethods;
using ScsmClient.SharedModels;
using ScsmClient.SharedModels.Models;

namespace ScsmClientTestCmd
{
    class Program
    {

        static void Main(string[] args)
        {


            var creds = new NetworkCredential("BWLAB\\admin", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.121", creds);

            CreatePerson(scsmClient);
            CreateIncident(scsmClient);
            TestCriteria();

            return;
            
            


            var cl = scsmClient.Class().GetClassByName("zOrganisationseinheit");
            var crit = scsmClient.Criteria().BuildObjectCriteria("zOKZ = '' and zOKZ like '%Test%'", cl);

            var obs = scsmClient.Object().GetObjectsByClassName("zOrganisationseinheit", "zOKZ like '%Test%'").ToList();
            Main1(args);

        }

        static void CreatePerson(ScsmClient.SCSMClient scsmClient)
        {
            var person = new Dictionary<string, object>();
            person["Vorname"] = "Bernhard";

            var p = scsmClient.Object().CreateObjectByClassName("BMI.Person", person);

        }

        static void CreateIncident(SCSMClient scsmclient)
        {

            var objectTemplates = scsmclient.ManagementGroup.Templates.GetObjectTemplates().OrderBy(t => t.Name);
            var templates = scsmclient.ManagementGroup.Templates.GetTemplates().OrderBy(t => t.Name);


            var temp = objectTemplates.FirstOrDefault(t => t.DisplayName == "Networking Issue Incident Template");
            var tid = temp.TypeID.GetElement() as ManagementPackTypeProjection;
            var tt = tid.TargetType;
            var type = temp.GetType();

            var inc = new IncidentDto();
            inc.Title = "Test Incident 123";
            inc.Impact = WellKnown.Incident.Impact.High;
            inc["Description"] = "TestDescription";

            var dic = inc.AsDictionary();

            var json = Json.Converter.ToJson(inc, true);
            var jsondic = Json.Converter.ToJson(dic, true);

            var isEqual = json == jsondic;

            var inc2 = Json.Converter.ToObject<IncidentDto>(json);
            var in3 = Json.Converter.ToObject<IncidentDto>(jsondic);

            
            scsmclient.Incident().CreateFromTemplate("Networking Issue Incident Template", inc);


        }

        static void TestCriteria()
        {
            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.20", creds);

            //var list = scsmClient.ManagementPack().GetManagementPacks().OrderBy(m => m.Name).ToList();
            var cla = scsmClient.Class().GetClassByName("[zMP_zOrganisationseinheit]zOrganisationseinheit");

            //var crit = new ManagementPackClassCriteria($"Name='[zMP_zOrganisationseinheit]zOrganisationseinheit' and ManagementPack='zMP_zOrganisationseinheit'");
            //var foundClass = scsmClient.ManagementGroup.EntityTypes.GetClasses(crit).FirstOrDefault();

            var cla2 = scsmClient.Class().GetClassByName("zBenutzer");

            var enum1 = cla.PropertyCollection.Where(p => p.Type == ManagementPackEntityPropertyTypes.@enum).Select(p =>
                scsmClient.ManagementGroup.EntityTypes.GetChildEnumerations(p.EnumType.Id, TraversalDepth.Recursive)).ToList();

            var enum1s = cla.PropertyCollection
                .Where(p => p.Type == ManagementPackEntityPropertyTypes.@enum)
                .Select(p => scsmClient.Enumeration().GetEnumerationChildByName(p.EnumType.GetElement(), "Fachabteilung")).ToList();

            var enum2s = cla2.PropertyCollection.Where(p => p.Type == ManagementPackEntityPropertyTypes.@enum).Select(p =>
                scsmClient.ManagementGroup.EntityTypes.GetChildEnumerations(p.EnumType.Id, TraversalDepth.Recursive)).ToList();

           
            

        }

        static void Main1(string[] args)
        {

            var criteriaString = @"
<Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">
    <Reference Id=""zMP_zBenutzer"" PublicKeyToken=""3e4cbba3563626ae"" Version=""1.0.0.0"" Alias=""MP1"" />
    <Reference Id=""System.Library"" PublicKeyToken=""31bf3856ad364e35"" Version=""7.5.0.0"" Alias=""SystemLibrary"" />


            <Expression>
                <SimpleExpression>
                    <ValueExpressionLeft>
                        <Property>$Context/Property[Type='MP1!zBenutzer']/zFirstName$</Property>
                    </ValueExpressionLeft>
                    <Operator>Like</Operator>
                    <ValueExpressionRight>
                        <Value>%Harald%</Value>
                    </ValueExpressionRight>
                </SimpleExpression>
            </Expression>



</Criteria>
";
            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.20", creds);


            var res = scsmClient.TypeProjection()
                .GetObjectProjectionObjects("zTP_zBenutzer_zAccount", criteriaString).ToList();





            var tps = scsmClient.TypeProjection().GetTypeProjectionsByCriteria("Name like 'z%'");

            var tp = scsmClient.TypeProjection().GetTypeProjectionByClassName("zTP_zBenutzer_zAccount");

            //var mps = scsmClient.ManagementPack().GetManagementPacks().OrderBy(m => m.Name).ToList();
            ////var criteriaxml = scsmClient.Criteria().CreateCriteriaXmlFromFilterString("zFirstName like \"%Harald\"", tp);
            //var crit = scsmClient.Criteria().BuildManagementPackClassCriteria("Name='zBenutzer'");


            //var c = scsmClient.ClassId().GetClass(crit);

            //var criteria = scsmClient.Criteria().BuildObjectProjectionCriteria(criteriaString, tp);

            //var critOptions = new ObjectQueryOptions();
            //critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            //critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;
            //critOptions.MaxResultCount = 1;

            //var reader = scsmClient.TypeProjection().GetObjectProjectionReader(criteria, critOptions);

            //var result = reader.Take(critOptions.MaxResultCount).Select(obj => obj.Object.ToObjectDto()).ToList();

        }
    }
}

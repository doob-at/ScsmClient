using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions;
using Reflectensions.ExtensionMethods;
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



            //var relClasses = scsmClient.Relations().FindRelationShip("BMI.Person", "BMI.Organisationseinheit");


            //scsmClient.Relations().CreateRelation(Guid.Parse("f12726fc-6861-f856-9ee0-5ea69c2e0c56"),
            //    Guid.Parse("4df2806f-3cb0-41e8-9467-799242975b03"));

            //var personid = CreatePerson(scsmClient);
            //var accountId = CreateAccount(scsmClient);
            //var ben = CreateBenutzer(scsmClient);
            //CreateIncident(scsmClient);
            //TestCriteria(scsmClient);
            //var id = CreatePersonWithRelations(scsmClient);
            var person = CreateUserFromJson(scsmClient);

            //var personId = Guid.Parse("309a5c0e-6b53-999d-8198-b32e445b1054");
            //var accountId = Guid.Parse("b9daa419-8a0b-0431-ef0b-5c431f8937c3");
            //Guid relId = CreateRelation(scsmClient, accountId, ben);

            
            var ch = CreateChange(scsmClient);
            var rel = scsmClient.Relations().CreateRelation( person, ch);

            var str = File.OpenRead(@"Z:\Repos\BMI\BMI.Benutzerverwaltung.UI\BMI.Benutzerverwaltung.UI\bin\Debug\netcoreapp3.1\win-x64\Logs\BMI.Benutzerverwaltung.UI20201004.log");
            //var attachmentContent = "Das ist ein Test";
            //var attachmentContentStream = new MemoryStream();
            //var strwriter = new StreamWriter(attachmentContentStream);
            //strwriter.Write(attachmentContent);

            var fa = scsmClient.Attachment().AddAttachment(ch, "testfile.txt", str, "Nur zum testen");
            var fa1 = scsmClient.Attachment().AddAttachment(ch, "testfile1.txt", str, "Nur zum testen1");
            var fa2 = scsmClient.Attachment().AddAttachment(ch, "testfile2.txt", str, "Nur zum testen2");


            var change = scsmClient.ChangeRequest().GetByGenericId(ch);

            var scChange = scsmClient.ScsmObject()
                .GetObjectsByTypeId(WellKnown.ChangeRequest.ProjectionType, $"G:Id == '{ch}'").FirstOrDefault();


            var attachments = scsmClient.Attachment().GetAttachments(ch);
            var rels = scsmClient.Relations().GetAllRelatedObjects(ch);


            return;




            var cl = scsmClient.Types().GetClassByName("zOrganisationseinheit");
            var crit = scsmClient.Criteria().BuildObjectCriteria("zOKZ = '' and zOKZ like '%Test%'", cl);

            var obs = scsmClient.ScsmObject().GetObjectsByTypeName("zOrganisationseinheit", "zOKZ like '%Test%'").ToList();
            Main1(args);

        }

        private static Guid CreateChange(SCSMClient scsmclient)
        {
            var cr = new ScsmClient.SharedModels.Models.ChangeRequestDto();
            cr.Title = $"Neue Person erstellen - Mit Attachment";
            cr.Description =
                $"Automatisiertes erstellen von folgender Person inkl. etwaiger Abhängigkeiten";
            
            return scsmclient.ChangeRequest().Create(cr);
        }

        private static Guid CreateRelation(SCSMClient scsmClient, Guid personId, Guid accountId)
        {


            return scsmClient.Relations().CreateRelationByName("BMI.AccountToBenutzer.Relation", personId, accountId);


        }

        static Guid CreatePerson(SCSMClient scsmClient)
        {
            
            var json =
                "{\r\n  \"Geburtsdatum\": \"1981-06-06T00:00:00\",\r\n  \"Nachname\": \"Windisch\",\r\n  \"Fax\": null,\r\n  \"Vorname\": \"Bernhard\",\r\n  \"Geschlecht\": \"männlich\",\r\n  \"Telefon\": null,\r\n  \"BPK\": \"LLMPuM6U4GraXjbcD7ChSrVotaQ=\",\r\n  \"Bundesdienst\": false,\r\n  \"AkademischerGradVor\": \"\",\r\n  \"Adresse\": null,\r\n  \"Title\": \"\",\r\n  \"Mobile\": null,\r\n  \"Personalnummer\": null,\r\n  \"EMail\": null,\r\n  \"AkademischerGradNach\": \"\"\r\n}";
            var person = Json.Converter.ToDictionary<string, object>(json);



            return scsmClient.Object().CreateObjectByClassName("BMI.Person", person);

        }

        static Guid CreatePersonWithRelations(SCSMClient scsmClient)
        {
            var person = new Dictionary<string, object>();
            person["Geburtsdatum"] = new DateTime(1981,6,6);
            person["Nachname"] = "Windisch-r4";
            person["Vorname"] = "Bernhard-r4";
            person["Geschlecht"] = "Männlich";
            person["BPK"] = "LLMPuM6U4GraXjbcD7ChSrVotaQ=";
            person["Bundesdienst"] = false;


            var acc1 = new Dictionary<string, object>();
            acc1["Username"] = "windis01";
            acc1["InitialPassword"] = "ABC12abc";
            acc1["Beschreibung"] = "Nested Account";

            var ben1 = new Dictionary<string, object>();
            ben1["Type"] = "Stammportal";
            ben1["GiltAb"] = new DateTime(2020, 11, 1);
            ben1["GiltBis"] = new DateTime(2020,12,31);

            acc1["BMI.Benutzer.Stammportal!"] = ben1;

            var acc2 = new Dictionary<string, object>();
            acc2["Username"] = "windis02";
            acc2["InitialPassword"] = "ABC12abc";
            acc2["Beschreibung"] = "Nested Account";

            var ben2 = new Dictionary<string, object>();
            ben2["~type"] = "BMI.Benutzer.Stammportal";
            ben2["Type"] = "Stammportal2";
            ben2["GiltAb"] = new DateTime(2021, 1, 1);
            ben2["GiltBis"] = new DateTime(2021, 12, 31);
            acc2["BMI.Benutzer!"] = ben2;



            person["BMI.Account!"] = new List<object>
            {
                acc1,
                acc2
            };

            person["BMI.Organisationseinheit!OKZ"] = "BMI-PI_S_ZELL_SEE";

            //var relClasses = scsmClient.Relations().FindRelationShip("BMI.Person", "BMI.Account");

            var newP = scsmClient.Object().CreateObjectByClassName("BMI.Person", person);

            var nP = scsmClient.ScsmObject().GetObjectById(newP);

            return newP;
        }

        static Guid CreateAccount(ScsmClient.SCSMClient scsmClient)
        {

            var json =
                "{\r\n  \"Username\": \"windisc1\",\r\n  \"InitialPassword\": \"ABC12abc\",\r\n  \"Beschreibung\": \"Test Account\",\r\n  \"GVGID\": null\r\n}";
            var acc = Json.Converter.ToDictionary<string, object>(json);

            return scsmClient.Object().CreateObjectByClassName("BMI.Account", acc);


        }

        static Guid CreateBenutzer(SCSMClient scsmClient)
        {
            var dict = new Dictionary<string, object>();
            dict["Type"] = "Stammportal";
            dict["GiltAb"] = DateTime.Now;

          

            return scsmClient.Object().CreateObjectByClassName("BMI.StammportalBenutzer", dict);


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

        static void TestCriteria(SCSMClient scsmClient)
        {


            //var list = scsmClient.ManagementPack().GetManagementPacks().OrderBy(m => m.Name).ToList();
            //var cla = scsmClient.Types().GetClassByName("[zMP_zOrganisationseinheit]zOrganisationseinheit");

            ////var crit = new ManagementPackClassCriteria($"Name='[zMP_zOrganisationseinheit]zOrganisationseinheit' and ManagementPack='zMP_zOrganisationseinheit'");
            ////var foundClass = scsmClient.ManagementGroup.EntityTypes.GetClasses(crit).FirstOrDefault();

            //var cla2 = scsmClient.Types().GetClassByName("zBenutzer");

            //var enum1 = cla.PropertyCollection.Where(p => p.Type == ManagementPackEntityPropertyTypes.@enum).Select(p =>
            //    scsmClient.ManagementGroup.EntityTypes.GetChildEnumerations(p.EnumType.Id, TraversalDepth.Recursive)).ToList();

            //var enum1s = cla.PropertyCollection
            //    .Where(p => p.Type == ManagementPackEntityPropertyTypes.@enum)
            //    .Select(p => scsmClient.Enumeration().GetEnumerationChildByName(p.EnumType.GetElement(), "Fachabteilung")).ToList();

            //var enum2s = cla2.PropertyCollection.Where(p => p.Type == ManagementPackEntityPropertyTypes.@enum).Select(p =>
            //    scsmClient.ManagementGroup.EntityTypes.GetChildEnumerations(p.EnumType.Id, TraversalDepth.Recursive)).ToList();


            //var accs = scsmClient.TypeProjection()
            //    .GetObjectProjectionObjects("BMI.Account.Projection", "Username -like 'windis0%'", null).ToList();

            //var objs = scsmClient.TypeProjection()
            //    .GetObjectProjectionObjects("BMI.Account.Projection", "BMI.Benutzer!Type -like 'stamm%'", null).ToList();

            //var bens = scsmClient.TypeProjection()
            //    .GetObjectProjectionObjects("BMI.Benutzer.Projection", "Type -like 'stamm%'", null).ToList();

            var org = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Organisationseinheit", "Name -like '%Polizei%'", 1).ToList();

            var pers = scsmClient.TypeProjection()
                .GetTypeProjectionObjects("BMI.PErson.Projection", "vorname -like 'Bernhard%' -and Nachname -like 'Wind%'",1, 1).ToList();

            //var per = pers.Where(p => p.ContainsKey("!BMI.Account")).ToList();



            //var bmiAccount = per[0].GetValue<List<ScsmObject>>("!BMI.Account");


            //var json = Json.Converter.ToJson(per[1], true);
            //Console.WriteLine(json);
        }


        static Guid CreateUserFromJson(SCSMClient scsmClient)
        {

            var jsonString = @"
{
  ""Vorname"": ""Bernhard"",
  ""Nachname"": ""Windisch"",
  ""Geburtsdatum"": ""1981-06-06T00:00:00"",
  ""Geschlecht"": ""männlich"",
  ""Titel"": null,
  ""AkademischerGradVor"": null,
  ""AkademischerGradNach"": null,
  ""EMail"": null,
  ""Bundesdienst"": false,
  ""Personalnummer"": null,
  ""BPK"": ""LLMPuM6U4GraXjbcD7ChSrVotaQ="",
  ""BMI.Organisationseinheit!OKZ"": ""BMI-PI_ST_LEIBNITZ"",
  ""BMI.Account!"": [
    {
      ""Username"": ""windisc2"",
      ""Beschreibung"": ""Test-Account"",
      ""InitialPassword"": ""U9l3_j"",
      ""GVGID"": null,
      ""BMI.Benutzer!"": [
        {
          ""~type"": ""BMI.Benutzer.Stammportal"",
          ""Type"": ""Stammportal"",
          ""GiltAb"": ""2020-11-01T00:00:00"",
          ""GiltBis"": ""2021-01-31T00:00:00"",
          ""ReferenceId"": null
        }
      ]
    }
  ]
}
";

            var dict = Json.Converter.ToDictionary(jsonString);
            var result = scsmClient.Object().CreateObjectsByClassName("BMI.Person", new List<Dictionary<string, object>>(){dict});

            return result.First().Value;
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
                .GetTypeProjectionObjects("zTP_zBenutzer_zAccount", criteriaString).ToList();





            var tps = scsmClient.Types().GetTypeProjectionsByCriteria("Name like 'z%'");

            var tp = scsmClient.Types().GetTypeProjectionByName("zTP_zBenutzer_zAccount");

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

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using doob.Reflectensions;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient;
using ScsmClient.ExtensionMethods;
using ScsmClient.Helper;
using ScsmClient.Model;
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

            //scsmClient.Object().DeleteObjectsByClassId(WellKnown.ServiceRequest.ClassId, "");
            //scsmClient.Object().DeleteObjectsByClassId(WellKnown.ChangeRequest.ClassId, "");



            //var mpc = scsmClient.Types().GetClassByName("BMI.Benutzer");
            //var crit = scsmClient.Criteria().CreateCriteriaXmlFromFilterString("'14.5.2020 20:12' -lt '14.5.2020 20:13'", mpc);

            //var t = crit;
            //var or = scsmClient.Object().DeleteObjectsByClassName("BMI.Organisationseinheit", "");

            //scsmClient.Object().DeleteObjectsByClassName("BMI.Stammportal.Anwendung", "");
            //scsmClient.Object().DeleteObjectsByClassName("BMI.Stammportal.AnwendungsRecht", "");
            //scsmClient.Object().DeleteObjectsByClassName("BMI.Stammportal.AnwendungsRecht.Parameter", "");
            //var del4 = scsmClient.Object().DeleteObjectsByClassName("BMI.Stammportal.Berechtigung", null, 10000);


            //var anwendungsrecht = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Stammportal.AnwendungsRecht", null).ToList();
            //var upd = anwendungsrecht.ToDictionary(a => a.ObjectId, o => new Dictionary<string, object>
            //{
            //    ["BMI.Stammportal.Anwendung!-"] = null
            //});

            //scsmClient.Object().UpdateObjects(upd);



            //var z = 1;

            //var relClasses = scsmClient.Relations().FindRelationShip("BMI.Person", "BMI.Organisationseinheit");


            //scsmClient.Relations().CreateRelation(Guid.Parse("f12726fc-6861-f856-9ee0-5ea69c2e0c56"),
            //    Guid.Parse("4df2806f-3cb0-41e8-9467-799242975b03"));

            //var personid = CreatePerson(scsmClient);
            //var accountId = CreateAccount(scsmClient);
            //var ben = CreateBenutzer(scsmClient);
            //CreateIncident(scsmClient);
            //TestCriteria(scsmClient);
            //var id = CreatePersonWithRelations(scsmClient);
            //var person = CreateUserFromJson(scsmClient);

            //var personId = Guid.Parse("309a5c0e-6b53-999d-8198-b32e445b1054");
            //var accountId = Guid.Parse("b9daa419-8a0b-0431-ef0b-5c431f8937c3");
            //Guid relId = CreateRelation(scsmClient, accountId, ben);


            //var ch = CreateChange(scsmClient);
            //var rel = scsmClient.Relations().CreateRelation( person, ch);

            //var str = File.OpenRead(@"Z:\Repos\BMI\BMI.Benutzerverwaltung.UI\BMI.Benutzerverwaltung.UI\bin\Debug\netcoreapp3.1\win-x64\Logs\BMI.Benutzerverwaltung.UI20201004.log");
            //var attachmentContent = "Das ist ein Test";
            //var attachmentContentStream = new MemoryStream();
            //var strwriter = new StreamWriter(attachmentContentStream);
            //strwriter.Write(attachmentContent);

            //var fa = scsmClient.Attachment().AddAttachment(ch, "testfile.txt", str, "Nur zum testen");
            //var fa1 = scsmClient.Attachment().AddAttachment(ch, "testfile1.txt", str, "Nur zum testen1");
            //var fa2 = scsmClient.Attachment().AddAttachment(ch, "testfile2.txt", str, "Nur zum testen2");


            //var change = scsmClient.ChangeRequest().GetByGenericId(ch);

            //var scChange = scsmClient.ScsmObject()
            //    .GetObjectsByTypeId(WellKnown.ChangeRequest.ProjectionType, $"G:Id == '{ch}'").FirstOrDefault();


            //var attachments = scsmClient.Attachment().GetAttachments(ch);
            //var rels = scsmClient.Relations().GetAllRelatedObjects(ch);




            //person["Nachname"] = "Windisch";
            //person.LastModified = DateTime.Now.AddYears(1);

            //scsmClient.Object().UpdateObject(person);

            //var template = "BMI Create Person ChangeRequest";
            //var ch = new ChangeRequest();
            //ch.Title = $"TestChange from Template - '{template}'";

            //scsmClient.ChangeRequest().CreateFromTemplate(template, ch);


            //var sr = scsmClient.ServiceRequest().GetById("112");



            // var nsr = new ServiceRequest();
            // nsr.Title = "Test SR";
            // nsr.UserInput = new UserInput()
            // {
            //     ["Password"] = "ABC12abc",
            //     ["Age"] = 123,
            //     ["Timestamp"] = DateTime.UtcNow
            // };

            //     //"<UserInputs><UserInput Question=\"Password\" Answer=\"ABC12abc\" Type=\"string\" /></UserInputs>";
            // nsr["BMI.Account!Id"] = "A56";

            // var srId = scsmClient.ServiceRequest().CreateFromTemplate("BMI Reset Password", nsr);

            // nsr.Title = "öauksbfdöasöbdgiua";

            //scsmClient.Object().UpdateObject(srId, nsr.AsDictionary());


            // var _sr = scsmClient.ServiceRequest().GetByGenericId(srId);
            //var changeMp = scsmClient.Types().GetClassById(WellKnown.ChangeRequest.ClassId);
            //var axtMP = scsmClient.Types().GetClassByName("System.WorkItem.Activity");

            //var tps = scsmClient.Types().GetTypeProjections().OrderBy(tp => tp.Name).ToList();

            //var rel = scsmClient.Relations().FindRelationship(changeMp, axtMP);

            //var ch = scsmClient.ChangeRequest().GetByGenericId(Guid.Parse("5797ad96-27f0-6bbe-8d5b-3a0e6453a5fa"));

            //var workitems = ch.GetValuesOrDefault<WorkItem>("System.WorkItem.Activity.ManualActivity!").ToList();


            //var act = scsmClient.ScsmObject().GetObjectById(Guid.Parse("cd9eec23-ceaf-89e1-b9a6-d17c1f646da1"));

            //var act1 = scsmClient.ScsmObject().GetObjectsByTypeId(WellKnown.ChangeRequest.ProjectionType, "Id -eq '137'").ToList();

            //var per = CreatePersonWithRelations(scsmClient);

            //var person = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Person",$"Vorname -eq 'Bernhard' -and Nachname -eq 'Bernhard'").FirstOrDefault();
            //var upd = new Dictionary<string, object>();
            //upd["Bundesdienst"] = false;
            //upd["Personalnummer-"] = null;

            //scsmClient.Object().UpdateObject(person.ObjectId, upd);


            //var currentAccounts = person.GetValuesOrDefault<ScsmObject>("BMI.Account!");

            //var acc1 = new Dictionary<string, object>();
            //acc1["Username"] = "windis11";
            //acc1["InitialPassword"] = "ABC12abc";
            //acc1["Beschreibung"] = "Nested Account";

            //var upd = new Dictionary<string, object>();

            //var nAccounts = currentAccounts.Select(a => (object)a.ObjectId).ToList();

            //nAccounts.Add(acc1);


            //upd["BMI.Account!"] = nAccounts;

            //scsmClient.Object().UpdateObject(per, upd);

            //var persons = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Person.Projection", "Id -eq 'P253340'").ToList();

            //var person = persons.FirstOrDefault();
            //var upd = new Dictionary<string, object>();
            ////upd["BMI.Organisationseinheit!OKZ-"] = "BMI-PI_ST_RATTEN"; 
            ////upd["BMI.Organisationseinheit!PLZ-"] = "8673"; 
            //upd["BMI.Organisationseinheit!OKZ"] = new List<object>() {"BMI-PI_ST_RATTEN", "BMI-PI_V_FELDKIRCH" };
            //scsmClient.Object().UpdateObject(person.ObjectId, upd );

            //var after = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Person.Projection", "Id -eq 'P253340'").FirstOrDefault();

            //CreatePersonWithRelations(scsmClient);
            //SearchProperty(scsmClient);

            //var cl = scsmClient.Types().GetClassByName("BMI.Stammportal.Berechtigung");
            //var retOptions = new RetrievalOptions();
            //retOptions.PropertiesToLoad = new List<string> {"ReferenceId"};
            ////retOptions.MaxResultCount = 100000;

            //var sw = new Stopwatch();
            //sw.Start();
            //var res = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Stammportal.Berechtigung", null, retOptions).ToList();
            //sw.Stop();

            //Console.WriteLine(sw.Elapsed);
            //Console.ReadLine();

            var cl = scsmClient.Types().GetTypeProjectionByName("BMI.Rolle.Manager");
            var crit = scsmClient.Criteria()
                .CreateCriteriaXmlFromFilterString("@G:BMI.Person!Id -eq 'ba8abf73-de4d-9d09-b18c-4f5931f3d603'", cl);

            var stammportalRollen =
                scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Rolle.Manager", "@G:BMI.Person!Id -eq 'ba8abf73-de4d-9d09-b18c-4f5931f3d603'").ToList();

            //var benutzer = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Benutzer.Stammportal.Projection",
            //    $"@G:Id -eq '1f5fdf8f-f42b-5f26-4f76-c4fa78cd991f'").FirstOrDefault();


            return;




            //var cl = scsmClient.Types().GetClassByName("zOrganisationseinheit");
            //var crit = scsmClient.Criteria().BuildObjectCriteria("zOKZ = '' and zOKZ like '%Test%'", cl);

            //var obs = scsmClient.ScsmObject().GetObjectsByTypeName("zOrganisationseinheit", "zOKZ like '%Test%'").ToList();
            //Main1(args);

        }


        public static void SearchProperty(SCSMClient scsmClient)
        {

            //var criteria = "<Criteria xmlns=\"http://Microsoft.EnterpriseManagement.Core.Criteria/\">\r\n  <Reference Id=\"BMI.Benutzerverwaltung\" PublicKeyToken=\"2d046bf38cf96dc8\" Version=\"1.0.0.107\" Alias=\"Ac39803be44cb4cde892380257eed033f\" />\r\n      <Expression>\r\n        <SimpleExpression>\r\n          <ValueExpressionLeft>\r\n            <Property>$Context/Property[Type='Ac39803be44cb4cde892380257eed033f!BMI.Person']/Vorname$</Property>\r\n          </ValueExpressionLeft>\r\n          <Operator>Equal</Operator>\r\n          <ValueExpressionRight>\r\n            <Property>$Context/Property[Type='Ac39803be44cb4cde892380257eed033f!BMI.Person']/Nachname$</Property>\r\n          </ValueExpressionRight>\r\n        </SimpleExpression>\r\n      </Expression>\r\n</Criteria>";
            //var criteria = "(@LastSync -eq null) -or (@LastSync -ne null -and (@LastSync -lt @NextSync))";


            //var criteria = "@G:LastModified -gt '9.11.2020 17:00' -and @G:LastModified -lt '9.11.2020 17:10'";
            //var criteria = "'6.6.1981' -eq @Geburtsdatum";
            var criteria = "@ObjectStatus -eq 'Active' -and @GiltAb  '06.06.1981'";

             var mpc = scsmClient.Types().GetTypeProjectionByName("BMI.Benutzer.Projection");
            var crit = scsmClient.Criteria().CreateCriteriaXmlFromFilterString(criteria, mpc);

            var p = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Benutzer.Projection", criteria).ToList();

            var t = p.Count();


        }
        

        public static string XmlString(string text)
        {
            return new XElement("t", text).LastNode.ToString();
        }

        private static Guid CreateChange(SCSMClient scsmclient)
        {
            var cr = new ScsmClient.SharedModels.Models.ChangeRequest();
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
            person["Nachname"] = "Bernhard";
            person["Vorname"] = "Bernhard";
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
            acc2["Username"] = "Bernhard";
            acc2["InitialPassword"] = "ABC12abc";
            acc2["Beschreibung"] = "Nested Account";

            var ben2 = new Dictionary<string, object>();
            ben2["~type"] = "BMI.Benutzer.Stammportal";
            ben2["Type"] = "Stammportal2";
            ben2["GiltAb"] = new DateTime(1981, 6, 6);
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

            var inc = new Incident();
            inc.Title = "Test Incident 123";
            inc.Impact = WellKnown.Incident.Impact.High;
            inc["Description"] = "TestDescription";

            var dic = inc.AsDictionary();

            var json = Json.Converter.ToJson(inc, true);
            var jsondic = Json.Converter.ToJson(dic, true);

            var isEqual = json == jsondic;

            var inc2 = Json.Converter.ToObject<Incident>(json);
            var in3 = Json.Converter.ToObject<Incident>(jsondic);


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

            var org = scsmClient.ScsmObject().GetObjectsByTypeName("BMI.Organisationseinheit", "Name -like '%Polizei%'", new RetrievalOptions() {MaxResultCount = 1}).ToList();

            var pers = scsmClient.TypeProjection()
                .GetTypeProjectionObjects("BMI.PErson.Projection", "vorname -like 'Bernhard%' -and Nachname -like 'Wind%'", new RetrievalOptions() {MaxResultCount = 1, ReferenceLevels = 1}).ToList();

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

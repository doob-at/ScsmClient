using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using doob.Reflectensions;
using ScsmClient;
using ScsmClient.SharedModels.Models;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cl = new SCSMClient("192.168.75.121", new NetworkCredential("BWLAB\\admin", "ABC12abc"));


            var updJson =
                "{\"DisplayName\":null,\"zID\":\"P1004\",\"zAuftragsbezeichnung\":\"Test12\",\"zHerstellername\":null,\"zVersion\":null,\"zBeschreibung\":null,\"zLizenzinformation\":null,\"zSDTicketReferenz\":null,\"zFertigstellungPaketierung\":null,\"zBekannteSoftwareabhaengigkeit\":null,\"zBenoetigteHardware\":null,\"zAbhaengigkeitzuServern\":null,\"zKommentar\":null,\"zPackageCICode\":null,\"zPaketierungsdokumentation\":null,\"zUserInteraction\":null,\"zAbnahmeDatum\":null,\"zWebLinkZuWikiDoku\":null,\"zSoftwarekategorie\":null,\"zPrioritaet\":null,\"zLizenztyp\":null,\"zStatus\":null,\"zRelease\":null,\"zArchitektur\":null,\"zKomplexitaet\":null,\"zInstallationBehavior\":null,\"zLogonRequirement\":null,\"zFertigstellungUAT\":null,\"zInstallationProgramVisibility\":null,\"zInteractWithProgramInst\":false,\"zRebootNotwendig\":false,\"zManuelleInstallation\":false,\"zOSWin7x64\":false,\"zOSWin10x64\":false,\"zOSCitrixXenApp\":false,\"zBusinessApp\":false,\"zProzessabfrage\":false,\"zOSWin7x86\":false,\"zActiveSetup\":false,\"zPaketierungsMethode\":null,\"zSourceSharePath\":null,\"zDevSharePath\":null,\"zNotifyPaketierer\":null,\"zTimestampAuftragAbbrechen\":null,\"zTimestampAuftragWeiterleiten\":null,\"zTimestampAuftragWeiterleitenIQS\":null,\"zTimestampAuftragWeiterleitenUAT\":null,\"zTimestampSoftwareFreigeben\":null,\"zZugewieseneMandanten\":null,\"zLocalizedApplicationName\":null}";

            //var srJson =
            //    "{\"Status\":\"a52fbc7d-0ee3-c630-f820-37eae24d6e9b\",\"TemplateId\":null,\"Priority\":null,\"Urgency\":null,\"CompletedDate\":null,\"ClosedDate\":null,\"Source\":null,\"ImplementationResults\":null,\"Notes\":null,\"Area\":null,\"SupportGroup\":null,\"Id\":null,\"Title\":\"Passwort zurücksetzen für Stammportal-Benutzer 'kampusc'\",\"Description\":null,\"ContactMethod\":null,\"CreatedDate\":null,\"ScheduledStartDate\":null,\"ScheduledEndDate\":null,\"ActualStartDate\":null,\"ActualEndDate\":null,\"IsDowntime\":false,\"IsParent\":false,\"ScheduledDowntimeStartDate\":null,\"ScheduledDowntimeEndDate\":null,\"ActualDowntimeStartDate\":null,\"ActualDowntimeEndDate\":null,\"RequiredBy\":null,\"PlannedCost\":0.0,\"ActualCost\":0.0,\"PlannedWork\":0.0,\"ActualWork\":0.0,\"UserInput\":{\"Password\":{\"Value\":\"TC@]n7\",\"Type\":\"string\"},\"GVGID\":{\"Value\":\"AT:B:112:PID:18875\",\"Type\":\"string\"},\"BenutzerId\":{\"Value\":\"B287\",\"Type\":\"string\"}},\"FirstAssignedDate\":null,\"FirstResponseDate\":null,\"ObjectId\":\"00000000-0000-0000-0000-000000000000\",\"LastModified\":\"0001-01-01T00:00:00\",\"TimeAdded\":\"0001-01-01T00:00:00\",\"DisplayName\":null,\"BMI.Benutzer!\":\"1d63a3de-75ba-2d13-a32d-934b04f2f026\"}";

            var update = Json.Converter.ToObject<Dictionary<string, object>>(updJson);

            cl.Object().UpdateObject(Guid.Parse("bd22fa1a-163e-bec5-11b8-c7d9196d1101"), update );

            //var srres = cl.ServiceRequest().CreateFromTemplate("BMI Reset Password", sr);


        }
    }
}

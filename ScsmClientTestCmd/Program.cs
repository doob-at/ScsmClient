using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using ScsmClient;
using ScsmClient.ExtensionMethods;

namespace ScsmClientTestCmd
{
    class Program
    {

        static void Main(string[] args)
        {


            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.20", creds);


            var cl = scsmClient.Class().GetClassByName("zOrganisationseinheit");
            var crit = scsmClient.Criteria().BuildObjectCriteria("zOKZ = '' and zOKZ like '%Test%'", cl);
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
            var scsmClient = new SCSMClient("10.0.0.211", creds);


            var tps = scsmClient.TypeProjection().GetTypeProjections("Name like 'z%'");

            var tp = scsmClient.TypeProjection().GetTypeProjectionByClassName("zTP_zBenutzer_zAccount");

            var mps = scsmClient.ManagementPack().GetManagementPacks().OrderBy(m => m.Name).ToList();
            //var criteriaxml = scsmClient.Criteria().CreateCriteriaXmlFromFilterString("zFirstName like \"%Harald\"", tp);
            var crit = scsmClient.Criteria().BuildManagementPackClassCriteria("Name='zBenutzer'");


            var c = scsmClient.Class().GetClass(crit);

            var criteria = scsmClient.Criteria().BuildObjectProjectionCriteria(criteriaString, tp);

            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;
            critOptions.MaxResultCount = 1;

            var reader = scsmClient.TypeProjection().GetObjectProjectionReader(criteria, critOptions);

            var result = reader.Take(critOptions.MaxResultCount).Select(obj => obj.Object.ToDto()).ToList();

        }
    }
}

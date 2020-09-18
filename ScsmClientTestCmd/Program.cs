using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaseIT.SCSM.Client;
using BaseIT.SCSM.Client.ExtensionMethods;
using Microsoft.EnterpriseManagement.Common;

namespace ScsmClientTestCmd
{
    class Program
    {
        static void Main(string[] args)
        {

            var criteriaString = @"
<Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">
    <Reference Id=""zMP_zBenutzer"" PublicKeyToken=""3e4cbba3563626ae"" Version=""1.0.0.0"" Alias=""zMP_zBenutzer"" />
    <Reference Id=""System.Library"" PublicKeyToken=""31bf3856ad364e35"" Version=""7.5.0.0"" Alias=""SystemLibrary"" />
    <Expression>
        <And>
            <Expression>
                <SimpleExpression>
                    <ValueExpressionLeft>
                        <Property>$Context/Property[Type='SystemLibrary!System.ConfigItem']/ObjectStatus$</Property>
                    </ValueExpressionLeft>
                    <Operator>NotEqual</Operator>
                    <ValueExpressionRight>
                        <Value>47101e64-237f-12c8-e3f5-ec5a665412fb</Value>
                    </ValueExpressionRight>
                </SimpleExpression>
            </Expression>
            <Expression>
                <SimpleExpression>
                    <ValueExpressionLeft>
                        <Property>$Context/Property[Type='zMP_zBenutzer!zBenutzer']/zFirstName$</Property>
                    </ValueExpressionLeft>
                    <Operator>Like</Operator>
                    <ValueExpressionRight>
                        <Value>%Harald%</Value>
                    </ValueExpressionRight>
                </SimpleExpression>
            </Expression>

        </And>
    </Expression>
</Criteria>
";
            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("192.168.75.20", creds);

            var tp = scsmClient.TypeProjection().GetTypeProjectionByClassName("zTP_zBenutzer_zAccount");
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

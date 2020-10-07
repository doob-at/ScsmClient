using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScsmClient.Attributes;

namespace ScsmClient
{
    public static class WellKnown
    {

        public static class Incident
        {
            public static Guid ClassId => new Guid("A604B942-4C7B-2FB2-28DC-61DC6F465C68");

            public static class EnumList
            {
                public static string Urgency => "04b28bfb-8898-9af3-009b-979e58837852";
                public static string Impact => "11756265-f18e-e090-eed2-3aa923a4c872";
            }

            public static class Urgency
            {
                public static string High => "2f8f0747-b6cb-7996-fd4a-84d09743f218";
                public static string Medium => "02625c30-08c6-4181-b2ed-222fa473280e";
                public static string Low => "725a4cad-088c-4f55-a845-000db8872e01";
            }

            public static class Impact
            {
                public static string Medium => "80cc222b-2653-2f68-8cee-3a7dd3b723c1";

                public static string High => "d2b5e816-2d24-8e7d-a61f-2cceaeac2664";

                public static string Low => "8f1a713e-53aa-9d8a-31b9-a9540074f305";

            }

        }

        public static class ServiceRequest
        {
            public static Guid ClassId => new Guid("04b69835-6343-4de2-4b19-6be08c612989");

            public static class EnumList
            {
                public static string Urgency => "eb35f771-8b0a-41aa-18fb-0432dfd957c4";
                public static string Priority => "d55e65ea-fae9-f7db-0937-843bfb1367c0";
                public static string Status => "4e0ab24a-0b46-efe6-c7d2-5704d95824c7";
                public static string Source => "848211a2-393a-6ec5-9c97-8e1e0cfebba2";
                public static string ImplementationResults => "4ea37c27-9b24-615a-94da-510539371f4c";
            }

            public static class Urgency
            {
                public static string Immediate => "cf01467e-5f6d-a521-867b-7ab453261171";
                public static string High => "530ee945-9d39-8801-52aa-b910694e0254";
                public static string Medium => "c3945d25-5f43-36c4-c1c9-2d6da1912d07";
                public static string Low => "b02d9277-a9fe-86f1-e95e-0ba8cd4fd075";
            }

            public static class Priority
            {
                public static string Immediate => "d0a0fadd-7f17-c0a2-cb2f-00e15c51282c";
                public static string High => "536beaf3-62a8-5dd0-248a-39c2bf86d3bc";
                public static string Medium => "dd43a3a8-c640-2146-85a4-77978e3bb375";
                public static string Low => "1e070214-693f-4a19-82bb-b88ee6362d98";
            }

            public static class Status
            {
                public static string New => "a52fbc7d-0ee3-c630-f820-37eae24d6e9b";
                public static string Closed => "c7b65747-f99e-c108-1e17-3c1062138fc4";
                public static string Completed => "b026fdfd-89bd-490b-e1fd-a599c78d440f";
                public static string Failed => "21dbfcb4-05f3-fcc0-a58e-a9c48cde3b0e";
                public static string Cancelled => "674e87e4-a58e-eab0-9a05-b48881de784c";
                public static string OnHold => "05306bf5-a6b9-b5ad-326b-ba4e9724bf37";
                public static string InProgress => "59393f48-d85f-fa6d-2ebe-dcff395d7ed1";
                public static string Submitted => "72b55e17-1c7d-b34c-53ae-f61f8732e425";
            }

            public static class Source
            {
                public static string Other => "57e11419-ac27-a6fa-97f1-0ec2a9722807";
                public static string Email => "b08ab652-8aff-3244-fe57-19bcd9dd3e23";
                public static string Portal => "e98657b3-2e82-9eda-ab3d-7cbb8dec8f01";
            }

            public static class ImplementationResults
            {
                public static string PartiallyImplemented => "076d31df-12e6-7213-d9e0-791b8184b79c";
                public static string Rejected => "b6fc74cc-a408-99a5-f53b-806b7eb1d161";
                public static string SuccessfullyImplemented => "d84df223-cc9f-b503-d1d4-824d225d2722";
                public static string Unauthorized => "c12070fa-a1b9-9820-e40a-ba5d7f584f8f";
                public static string ImplementedWithIssues => "6af5f8c2-ed77-cb69-26af-d66c1e62e720";
                public static string Cancelled => "e8863e3a-1ac3-159e-1cf6-d9d9638ffb9a";
                public static string Failed => "3dd522f4-92ec-689a-ae0e-db78fe95e3ad";
            }

        }
    }



}

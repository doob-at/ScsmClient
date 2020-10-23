using System;

namespace ScsmClient.SharedModels
{
    public static class WellKnown
    {

        public static class Incident
        {
            public static Guid ClassId => new Guid("A604B942-4C7B-2FB2-28DC-61DC6F465C68");

            public static Guid ProjectionType => new Guid("9cc39579-e071-cca8-ef6e-07263620b87f");

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
            public static Guid ProjectionType => new Guid("e44b7c06-590d-64d6-56d2-2219c5e763e0");
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

            public static class Area
            {

                public static class Content
                {
                    public static string Root => "defb6baf-8291-7d62-c2e6-68ce80485d80";
                    public static string Extranet => "fb3d8743-a1c4-c4a2-05a6-2000ec1b8acb";
                    public static string Intranet => "3a049a88-9867-1f8b-749a-9e3b07826de0";
                    public static string Knowledge => "37f56097-c2d7-b006-dd66-e65e11d6dcd2";
                    public static string Other => "c03109d8-5c0c-8052-cf58-c9315ae9982a";
                }

                public static class Directory
                {
                    public static string Root => "b69a3387-5ffd-e6f8-f343-ac8da2343687";
                    public static string AccountManagement => "5e2acc80-e351-1290-9f6a-0305effae839";
                    public static string Other => "275de5f0-8318-c382-690f-753d8cc230d9";
                    public static string OUManagement => "486afd4c-394f-6083-9cd2-2dfb4e3a9338";
                }

                public static class Facilities
                {
                    public static string Root => "e332182f-c88e-2fd1-aa4d-64d6efba5c6d";
                    public static string Other => "93a6352d-757d-ee0a-7e19-1b7be577725b";
                    public static string Power => "fdbd371d-6966-05d5-c80d-14cd0bdaff85";
                }

                public static class File
                {
                    public static string Root => "0ad08325-0c3d-f201-74da-3263594d38c4";
                    public static string DiskVolumesAndDFS => "c8c36f40-8560-a38f-4dd5-3e18acd2e929";
                    public static string Other => "21cc3262-54f2-0f09-dae1-8283447ed8ba";
                    public static string RestoreFile => "acc28d76-46a7-65a0-d398-c13dfd38d9ab";
                    public static string Shares => "09153b6c-ce8b-8bbe-613a-7a20c4813538";
                }

                public static class Hardware
                {
                    public static string Root => "a2ed2169-e33f-da52-9295-bb737a5d2348";
                    public static string Client => "c31de0fa-04c2-0f60-5fe2-e287ccbd047b";
                    public static string Components => "23e9b7f0-fb2b-821b-beac-945569707fe2";
                    public static string Network => "85cfbae1-4628-c9ac-28f7-b25f55630132";
                    public static string Other => "4c85dee1-f81a-336a-fa93-2db9ddfae13e";
                    public static string Server => "c0167d93-69e3-669b-264c-857e1ad82e51";
                    public static string Storage => "1ac9e453-c457-fc50-cf98-c6081030cabb";
                }

                public static class Infrastructure
                {
                    public static string Root => "3c5e6037-b057-5745-4d89-e95f67d3236b";
                    public static string Backups => "25fdd858-e5d5-b342-bb80-66280cf76414";
                    public static string Monitoring => "7a7a7f00-97b0-7f75-c43a-8bb7e475c581";
                    public static string NameResolution => "86f5aa89-5f42-fb80-3838-cdea69850a79";
                    public static string NetworkConnectivity => "4560320b-98f0-9ce3-a057-4b8ed5624ba8";
                    public static string Other => "d2a67314-6d99-822e-edf2-dd4b37fbcaaf";
                    public static string ProxyOrFirewall => "db1943a1-c257-a4c0-7ff5-8aa1c8b6b609";
                    public static string RemoteAccess => "d1440b3a-65d5-b61b-80ee-a26b608e11ba";
                    public static string ServerServices => "d1bc0c04-cfa4-b3e6-b75f-e613bb512055";
                    public static string Telephony => "3c10384d-da8d-685b-17a8-adb16cc6aef9";
                }

                public static class Messaging
                {
                    public static string Root => "78be9804-febf-e164-b62e-4b5bd87055f6";
                    public static string Client => "653eee38-b278-8546-0731-1ed56fbb08a8";
                    public static string Other => "b668438f-8903-28a4-84dd-118baed60ff5";
                    public static string Server => "308ba500-b09c-3d1c-8448-de8c5896d37b";
                }

                public static class Operations
                {
                    public static string Root => "58113904-cd9b-c3b0-6af4-5f371b269482";
                    public static string Documentation => "0fffb8a2-cce2-407e-10c9-a988aa4bcad1";
                    public static string Management => "9706cf37-8eff-d949-a94a-92ffed899810";
                    public static string Other => "d223e3c0-bcc5-395c-94aa-73838db8399e";
                    public static string Process => "8b2b2c95-002b-8cf5-7596-8815f6c9b53a";
                }

                public static class Security
                {
                    public static string Root => "8d8fb939-7174-7bb9-b7c4-58bb0e228f5a";
                    public static string AccessControl => "d8655b75-8e8e-d3f4-f974-884b31af53e5";
                    public static string AccountManagement => "64841df1-ac2b-16dd-cc8c-9a229d11fbe7";
                    public static string Information => "05b288a9-0899-5d36-17a4-a6a9d96fcab4";
                    public static string Other => "00f04276-23b7-e12c-5e82-1d2d5e292056";
                    public static string Physical => "5b8feea3-f814-4d3d-9caa-e94401d870b6";
                }

                public static class Software
                {
                    public static string Root => "bc424d1d-8f2b-8c66-ba7c-7736588f059f";
                    public static string Application => "97a972f8-c5c6-79c9-c507-83582fa4df1c";
                    public static string Configuration => "3400a8b7-b2fe-a0ac-51fa-29246ed00ff3";
                    public static string Driver => "aba6b3fc-45cb-e408-f7de-b5b6638b5e34";
                    public static string Firmware => "8fe17a43-01a8-00df-b582-e73c73a12d12";
                    public static string Installation => "8bc0c1ae-00fe-f764-a124-4c1ccb6a22ef";
                    public static string Licenses => "b58aa6f0-7053-f4bb-f029-ab9084387a43";
                    public static string OperatingSystem => "34ff7bbd-dce8-29dd-1127-169df3784437";
                    public static string Other => "f2256d85-2640-29a4-e64f-116588cacf8f";
                    public static string Patch => "63b2fd33-e6be-c47c-f409-246f00a68556";
                }

                public static string Other => "d599b332-c9ed-d04c-710c-6354345f6246";
            }

        }

        public static class ChangeRequest
        {
            public static Guid ClassId => new Guid("e6c9cf6e-d7fe-1b5d-216c-c3f5d2c7670c");

            public static Guid ProjectionType => new Guid("674194d8-0246-7b90-d871-e1ea015b2ea7");

            public static class Status
            {
                public static string New => "a87c003e-8c19-a25f-f8b2-151b56670e5c";
                public static string Closed => "f228d50b-2b5a-010f-b1a4-5c7d95703a9b";
                public static string Completed => "68277330-a0d3-cfdd-298d-d5c31d1d126f";
                public static string Failed => "85f00ead-2603-6c68-dfec-531c83bf900f";
                public static string Cancelled => "877defb6-0d21-7d19-89d5-a1107d621270";
                public static string OnHold => "dd6b0870-bcea-1520-993d-9f1337e39d4d";
                public static string InProgress => "6d6c64dd-07ac-aaf5-f812-6a7cceb5154d";
                public static string Submitted => "504f294c-ae38-2a65-f395-bff4f085698b";
            }

            public static class Category
            {
                public static string Emergency => "357662a7-451c-df62-ed68-3147bdff324e";
                public static string Standard => "02d2f92f-d925-5ad6-eb1f-67020701697a";
                public static string Major => "e9ee7044-3a42-a34b-1237-ec0d32f2377a";
                public static string Minor => "c0126730-ab4a-4c62-26e0-7706bc176413";
            }

            public static class Priority
            {
                public static string High => "32ee0157-fdc5-67c4-7971-c6b1fe8dfc66";
                public static string Medium => "3a96f7dc-4469-ed8b-48a1-517f7ccf6189";
                public static string Immediate => "ff5fd035-4d4e-42d4-f076-7412257b88ac";
                public static string Low => "ab51007f-65a3-845b-a8f4-2896c9da9f5f";
            }

            public static class Impact
            {
                public static string Significant => "70ca6737-6f4d-ea78-c392-2cfc61eaadfc";
                public static string Standard => "312b4612-6b0e-63e9-e9c4-0a4d04d7363a";
                public static string Major => "7b30147f-3820-733e-55ee-0885aceb60f4";
                public static string Minor => "e90e735e-959b-eb5b-41be-1fd07c30a740";
            }

            public static class Risk
            {
                public static string Low => "13b87263-844c-833f-2fae-30939de58244";
                public static string Medium => "d92ca060-fd52-27dc-8268-9b0f5fd4ffda";
                public static string NotAssessed => "1fbd7d66-be43-d48d-8ea8-2367f6039697";
                public static string High => "978b7e07-10c5-25c1-e6f4-e1df0579fc82";
            }

            public static class ImplementationResults
            {
                public static string PartiallyImplemented => "7dc7e36a-594c-9102-fce3-2e7058a27527";
                public static string Rejected => "5973a0f1-737f-010c-2218-f10376fb3032";
                public static string SuccessfullyImplemented => "d16a0c70-821f-5dbb-11bc-dc7d897b8c5b";
                public static string Unauthorized => "6ea74967-25af-41e4-f350-12fa8b803241";
                public static string ImplementedWithIssues => "8cb8f9df-b7e6-d9c1-e9d0-71fe3f0afe5f";
                public static string Cancelled => "dcbf4f56-7195-128d-d7be-62cfb6a776bc";
                public static string Failed => "3f02a1ec-594f-1f59-8112-3658490f232d";
            }

            public static class Area
            {

                public static class Content
                {

                    public static string Root => "48bf1fcf-7552-2e5c-187d-b3cd2dcfa993";
                    public static string Extranet => "1cfc30b8-26f2-591f-bd7b-dd28e172b1b5";
                    public static string Intranet => "0d5ff99e-a276-1250-f8df-8694ccd93623";
                    public static string Knowledge => "aeeb06f2-dfe6-972a-93c8-488fb4016e51";
                    public static string Other => "ff77456b-9c82-cf2a-cc76-44df6238a2cf";
                }

                public static class Directory
                {
                    public static string Root => "8f1acc16-ef2c-4374-3b4c-e16a8a04ad0b";
                    public static string AccountManagement => "ada3b039-a4d3-3005-3e5c-c6a7a85c5655";
                    public static string Other => "ebe8547f-529c-964d-a232-c80fefd87c82";
                    public static string OUManagement => "5974b1bf-a5b9-cf94-1639-6c38f5a7c732";
                }

                public static class Facilities
                {
                    public static string Root => "54bc5e38-559f-b569-1859-ec5db5f3012b";
                    public static string Other => "815108bf-d241-dde8-279b-bcde7dceec1e";
                    public static string Power => "a483aa7d-e0e8-a454-28d2-5802d9ef331c";
                }

                public static class File
                {
                    public static string Root => "7ee1901d-b877-846d-3b77-c347f21d7b87";
                    public static string DiskVolumesAndDFS => "a4e69877-6ffa-ec18-558e-8830e34dae06";
                    public static string Other => "5b3cd9e9-a4d7-c866-63ed-f6d9de38e142";
                    public static string RestoreFile => "9085ca71-0806-02f4-f3b9-9e60ddbfa6e1";
                    public static string Shares => "68fc3611-af82-dfc6-c5a5-8b84e4eec9b6";
                }

                public static class Hardware
                {
                    public static string Root => "cb454993-e2e0-6bee-163c-69985be58d40";
                    public static string Client => "76c3d837-0279-4b70-db9b-9d31502b0296";
                    public static string Components => "6ffab887-b306-3d1a-e4cc-93a5b25b12c7";
                    public static string Network => "5da666cc-0c8b-2e01-e4eb-dd6d791b2699";
                    public static string Other => "fdf0b03e-1df9-4dcb-baad-cf900a89edf4";
                    public static string Server => "b60f4a7b-35bc-3ea6-480c-976da3946d63";
                    public static string Storage => "f16719f8-26e5-d162-7d9a-3beb0a70c9a9";
                }

                public static class Infrastructure
                {
                    public static string Root => "4c4417b1-58f4-48f7-9e5f-cd137f3260ac";
                    public static string Backups => "06918a78-4766-8ede-2a45-8b053c9f4842";
                    public static string Monitoring => "2fb70440-1728-5c51-dd8c-28c846eaf886";
                    public static string NameResolution => "c129e285-1e8b-faaa-a003-72fd839d54b0";
                    public static string NetworkConnectivity => "89617d89-7a8a-f8bd-2c7c-540eddf4129a";
                    public static string Other => "5dedcac5-da2a-cead-109a-5882ecd5efcf";
                    public static string ProxyOrFirewall => "cd44283b-61a7-fb92-0624-cba26a43a77b";
                    public static string RemoteAccess => "089b4783-1e4d-7e03-2745-527d19cdeaa0";
                    public static string ServerServices => "0a2e4aad-1261-b570-0a12-82ea0cd74f69";
                    public static string Telephony => "7cf621d2-26f2-a8b3-c616-6c753efb9489";

                }

                public static class Messaging
                {
                    public static string Root => "0ea668a8-2f7e-7f8c-67ce-088b1f16c2a5";
                    public static string Client => "69beed6c-94f5-3c41-4a86-d693486f696a";
                    public static string Other => "4beff599-1f64-266a-d64f-1a1682caa2e3";
                    public static string Server => "4c2a3da7-d4df-b090-aeed-0ce4af4ef420";
                }

                public static class Operations
                {
                    public static string Root => "e565fd01-3a45-e02a-6dd2-12753826dcee";
                    public static string Documentation => "202f96bd-6df0-fa18-8201-4472f5dd5ab4";
                    public static string Management => "273efbce-47db-c3eb-72e5-558caa125bfb";
                    public static string Other => "fe6b0386-0826-9a18-257b-c331c64a1067";
                    public static string Process => "c1109978-2530-fb99-e37d-a02822f9f040";
                }

                public static class Security
                {
                    public static string Root => "066507f5-59e5-627b-8739-098bacd244c7";
                    public static string AccessControl => "e5b918a3-7421-d647-58ce-490bbacd29ef";
                    public static string AccountManagement => "987e521b-9717-714a-77ef-2d4f03372f05";
                    public static string Information => "96aa1529-0b7e-292f-e669-d1eae66502f2";
                    public static string Other => "53d1c3ec-0b24-b894-04ec-d635f044c068";
                    public static string Physical => "fe6c5486-5eb1-3d22-5d59-e80cfc58b86f";
                }

                public static class Software
                {
                    public static string Root => "55abaa96-5b63-600f-9cf6-f7d5ac71544e";
                    public static string Application => "29308eb3-cfc3-9cb6-23f7-53986cf31ecf";
                    public static string Configuration => "bbf09306-8b96-dcab-554f-bcf020f0a599";
                    public static string Driver => "ee5a5c4c-6c83-9aaf-abfd-c6ce8cef00b3";
                    public static string Firmware => "c6350785-896e-2096-2fca-58dc0bb4e774";
                    public static string Installation => "4374204c-ced1-9a88-52ae-ea457001a0bc";
                    public static string Licenses => "241bb564-adeb-1fdd-754a-e66fb0734bd6";
                    public static string OperatingSystem => "45dc9909-5ea5-e70e-fddc-c621c4acd9aa";
                    public static string Other => "d79c30c0-6a2c-4b31-ed75-902d2e72a44e";
                    public static string Patch => "c1223efb-32cd-29d7-0f0e-d29cff8f21c8";
                }


                public static string Other => "34fd9437-eaf1-0c15-9510-3a38fbc3f798";



            }

        }
    }



}

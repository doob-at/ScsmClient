using System;
using System.Net;
using ScsmClient;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var creds = new NetworkCredential("LANFL\\administrator", "ABC12abc");
            var scsmClient = new SCSMClient("10.0.0.211", creds);


            var tps = scsmClient.TypeProjection().GetTypeProjections("Name like 'z%'");
        }
    }
}

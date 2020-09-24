using System;
using System.Linq;
using System.Net;
using System.Security;

namespace ScsmClient.Helper
{
    public static class CredentialsHelper
    {
        public static NetworkCredential BuildSecureCredential(NetworkCredential credentials)
        {

            var nCred = new NetworkCredential();


            if (credentials.UserName?.Contains("\\") == true)
            {
                var splitted = credentials.UserName.Split('\\').Where(p => !String.IsNullOrWhiteSpace(p)).ToList();
                var parts = splitted.Count;
                if (parts == 0)
                {
                    throw new ArgumentNullException(nameof(credentials.UserName));
                }

                if (parts == 1)
                {
                    nCred.UserName = splitted[0];
                }
                else
                {
                    nCred.Domain = splitted[0];
                    nCred.UserName = splitted[1];
                }
            }
            else
            {
                nCred.UserName = credentials.UserName;
            }


            if (credentials.SecurePassword != null)
            {
                nCred.SecurePassword = credentials.SecurePassword;
            }
            else
            {
                var secureString = new SecureString();
                foreach (char c in credentials.Password)
                    secureString.AppendChar(c);

                secureString.MakeReadOnly();
                nCred.SecurePassword = secureString;
            }

            return nCred;
        }
    }
}

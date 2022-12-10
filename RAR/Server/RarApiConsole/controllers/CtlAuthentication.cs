using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;
using System.Security.Cryptography;

namespace RarApiConsole.controllers
{
    internal class CtlAuthentication
    {
        private DoUser temp = new();
        private DatabaseContext db = new();

        public CtlAuthentication()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Authentication", HandleRequest);
        }

        public bool HandleRequest(HttpListenerContext aContext)
        {
            bool retVal = false;
            var aReq = aContext.Request;
            var aRes = aContext.Response;

            switch (aReq.HttpMethod)
            {
                case "POST":
                    retVal = Login(aContext);
                    break;
                default:
                    retVal = NotSupported(aContext);
                    break;
            }

            return retVal;
        }

        bool Login(HttpListenerContext aContext)
        {
            bool retVal = false;

            var aRequest = aContext.Request;
            var aResponse = aContext.Response;

            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if (aRequest.HasEntityBody == true)
            {
                var obj = new DoUser();
                bool usernameIsSet = false;
                bool passwordIsSet = false;

                foreach (var pair in keyPair)
                {
                    if (pair.Key.Equals("username"))
                    {
                        obj.username = pair.Value;
                        usernameIsSet = true;
                    }
                    if (pair.Key.Equals("password"))
                    {
                        obj.password = GetHashString(pair.Value);
                        passwordIsSet = true;
                    }
                }

                if ((usernameIsSet == true) && (passwordIsSet == true))
                {
                    bool userFound = false;
                    foreach (var user in db.users)
                    {
                        if (userFound == false)
                        {
                            if ((user.username == obj.username) && (user.password == obj.password))
                            {
                                userFound = true;
                            }
                        }
                    }

                    if (userFound == true)
                    {
                        aResponse.StatusCode = (int)HttpStatusCode.OK;
                        retVal = true;
                    }
                    else
                    {
                        aResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    aResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                aResponse.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(arr);
            aResponse.OutputStream.Write(bytes, 0, bytes.Length);
            aResponse.OutputStream.Close();

            return retVal;
        }

        static bool NotSupported(HttpListenerContext aContext)
        {
            var response = aContext.Response;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ContentType = "text/plain";
            response.OutputStream.Close();
            return false;
        }
        public static byte[] GetHash(string inputString)
        {
            byte[] arr = null;
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                arr = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }

            return arr;
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
            {
                sb.Append(b.ToString("X2")); // X2 is for Hexadecimal 
            }

            return sb.ToString();
        }
    }
}
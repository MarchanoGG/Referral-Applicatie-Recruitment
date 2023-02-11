using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace RarApiConsole.controllers
{
    internal class CtlUsers
    {
        private DoUser temp = new();
        private DatabaseContext db = new();
        private static CtlUsers? instance;

        public CtlUsers()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Users", HandleRequest);
        }

        public static CtlUsers Instance()
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }

        public bool HandleRequest(HttpListenerContext aContext)
        {
            bool retVal = false;
            var aReq = aContext.Request;
            var aRes = aContext.Response;

            switch (aReq.HttpMethod)
            {
                case "GET":
                    retVal = Get(aContext);
                    break;
                case "POST":
                    retVal = Post(aContext);
                    break;
                case "PUT":
                    retVal = Put(aContext);
                    break;
                case "DELETE":
                    retVal = Delete(aContext);
                    break;
                default:
                    retVal = NotSupported(aContext);
                    break;
            }

            return retVal;
        }

        public bool Get(HttpListenerContext aContext)
        {
            bool resVal = false;

            var aRequest = aContext.Request;
            var response = aContext.Response;

            string arr = "";
            var ok = aRequest.QueryString.Get("object_key");
            var token = aRequest.QueryString.Get("token");
            var scoreboard_key = aRequest.QueryString.Get("fk_scoreboard");

            if (aRequest.QueryString.HasKeys() == true && scoreboard_key != null)
            {
                arr = temp.ReadByScoreboard(db, int.Parse(scoreboard_key));
            }
            else if (aRequest.QueryString.HasKeys() == true && ok != null)
            {
                arr = temp.ReadSpecific(db, int.Parse(ok));
            }
            else if (aRequest.QueryString.HasKeys() == true && token != null)
            {
                arr = temp.ReadSpecific(db, int.Parse(token));
            }
            else
            {
                arr = temp.ReadAll(db);
            }

            if (arr.Length > 0)
            {
                response.ContentType = "application/json";
                resVal = true;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NoContent;
                resVal = true;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(arr);
            response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            response.OutputStream.Close();

            return resVal;
        }

        public bool GetAction()
        {
            bool resVal = false;

            if(temp.ReadAll(db).Length > 0) 
            {
                resVal = true;
            }

            return resVal;
        }

        bool Post(HttpListenerContext aContext)
        {
            bool retVal = false;
            var aResponse = aContext.Response;
            var aRequest = aContext.Request;

            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if ((aRequest.HasEntityBody == true) && (temp.ValidateInput(keyPair)))
            {
                if (CreateAction(keyPair) > 0)
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

            byte[] bytes = Encoding.UTF8.GetBytes(arr);
            aResponse.OutputStream.Write(bytes, 0, bytes.Length);
            aResponse.OutputStream.Close();

            return retVal;
        }

        public int CreateAction(Dictionary<string, string> aPair)
        {
            int retVal = 0;

            var obj = new DoUser();

            var ctlProfiles = CtlProfiles.Instance();

            foreach (var pair in aPair)
            {
                if (pair.Value != null)
                {
                    if (pair.Key.Equals("username"))
                    {
                        obj.username = pair.Value;
                    }
                    if (pair.Key.Equals("password"))
                    {
                        obj.password = GetHashString(pair.Value);
                    }
                    if (pair.Key.Equals("recruiter"))
                    {
                        obj.recruiter = int.Parse(pair.Value);
                    }

                    if (pair.Key.Equals("profile"))
                    {
                        var profilePair = JsonConvert.DeserializeObject<Dictionary<string, string>>(pair.Value);

                        if (profilePair != null)
                        {
                            int profileKey = ctlProfiles.CreateAction(profilePair);
                            if (profileKey > 0)
                            {
                                obj.fk_profile = profileKey;
                            }
                        }
                    }
                    else if (pair.Key.Equals("fk_profile"))
                    {
                        obj.fk_profile = int.Parse(pair.Value);
                    }
                }
            }

            bool usernameFound = false;

            foreach (var user in db.users.ToList())
            {
                if(user.username == obj.username) 
                {
                    usernameFound = true;
                }
            }

            if(!usernameFound)
            {
                if (temp.Create(db, obj) == true)
                {
                    retVal = obj.object_key;
                }
            }

            return retVal;
        }

        bool Put(HttpListenerContext aContext)
        {
            bool retVal = false;

            var aRequest = aContext.Request;
            var aResponse = aContext.Response;

            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if ((aRequest.HasEntityBody == true) && (temp.ValidateInput(keyPair)))
            {
                int objectKey = 0;
                foreach (var pair in keyPair)
                {
                    if (pair.Key.Equals("object_key"))
                    {
                        objectKey = int.Parse(pair.Value);
                    }
                }

                if (UpdateAction(keyPair, objectKey) > 0)
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

            byte[] bytes = Encoding.UTF8.GetBytes(arr);
            aResponse.OutputStream.Write(bytes, 0, bytes.Length);
            aResponse.OutputStream.Close();

            return retVal;
        }

        public int UpdateAction(Dictionary<string, string> aPair, int aObjectKey)
        {
            int retVal = 0;

            var temp = new DoUser();

            var obj = temp.ReadSpecificObject(db, aObjectKey);

            var ctlProfiles = CtlProfiles.Instance();

            foreach (var pair in aPair)
            {
                if (pair.Value != null)
                {
                    if (pair.Key.Equals("username"))
                    {
                        obj.username = pair.Value;
                    }
                    if (pair.Key.Equals("recruiter"))
                    {
                        obj.recruiter = int.Parse(pair.Value);
                    }
                    if (pair.Key.Equals("fk_profile"))
                    {
                        obj.fk_profile = int.Parse(pair.Value);
                    }

                    if (pair.Key.Equals("profile"))
                    {
                        var profilePair = JsonConvert.DeserializeObject<Dictionary<string, string>>(pair.Value);

                        if (profilePair != null && obj.fk_profile != null)
                        {
                            obj.fk_profile = ctlProfiles.UpdateAction(profilePair, obj.fk_profile.Value);
                        }
                        else if (profilePair != null)
                        {
                            obj.fk_profile = ctlProfiles.CreateAction(profilePair);
                        }
                    }
                }
            }

            if (temp.Update(db, obj) == true)
            {
                retVal = obj.object_key;
            }

            return retVal;
        }

        bool Delete(HttpListenerContext aContext)
        {
            bool retVal = false;

            var aResponse = aContext.Response;
            var aRequest = aContext.Request;

            string arr = "";

            var ctlProfiles = CtlProfiles.Instance();
            var ok = aRequest.QueryString.Get("object_key");

            if (aRequest.QueryString.HasKeys() == true && ok != null)
            {
                var tempObj = db.users.Find(int.Parse(ok));
                if (tempObj != null && tempObj.fk_profile != null)
                {
                    ctlProfiles.DeleteAction(tempObj.fk_profile.Value);
                }

                if (DeleteAction(int.Parse(ok)))
                {
                    aResponse.StatusCode = (int)HttpStatusCode.OK;
                    aResponse.ContentType = "application/json";
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

            byte[] bytes = Encoding.UTF8.GetBytes(arr);
            aResponse.OutputStream.Write(bytes, 0, bytes.Length);
            aResponse.OutputStream.Close();

            return retVal;
        }

        public bool DeleteAction(int ObjectKey)
        {
            bool res = false;

            res = temp.Delete(db, ObjectKey);

            return res;
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
            byte[] arr;
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

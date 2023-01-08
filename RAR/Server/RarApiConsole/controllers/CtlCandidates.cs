using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;
using Newtonsoft.Json;

namespace RarApiConsole.controllers
{
    internal class CtlCandidates
    {
        private DoCandidate temp = new();
        private DatabaseContext db = new();
        private static CtlCandidates? instance;

        public CtlCandidates()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Candidates", HandleRequest);
        }

        public static CtlCandidates Instance()
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

            if (aRequest.QueryString.HasKeys() == true && ok != null)
            {
                arr = temp.ReadSpecific(db, int.Parse(ok));
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

        bool Post(HttpListenerContext aContext)
        {
            bool retVal = false;
            var aResponse = aContext.Response;
            var aRequest = aContext.Request;

            var ctlProfiles = CtlProfiles.Instance();

            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if ((aRequest.HasEntityBody == true) && (temp.ValidateInput(keyPair)))
            {
                var obj = new DoCandidate();

                foreach (var pair in keyPair)
                {
                    if (pair.Key.Equals("fk_profile"))
                    {
                        obj.fk_profile = int.Parse(pair.Value);
                    }
                    if (pair.Key.Equals("referred_at"))
                    {
                        obj.referred_at = DateTime.Parse(pair.Value);
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
                }

                if (temp.Create(db, obj) == true)
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

        bool Put(HttpListenerContext aContext)
        {
            bool retVal = false;

            var aRequest = aContext.Request;
            var aResponse = aContext.Response;

            var ctlProfiles = CtlProfiles.Instance();
            
            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if ((aRequest.HasEntityBody == true) && (temp.ValidateInput(keyPair)))
            {
                var obj = db.candidates.Find(int.Parse(keyPair["object_key"]));

                if (obj != null)
                {
                    foreach (var pair in keyPair)
                    {
                        if (pair.Key.Equals("fk_profile"))
                        {
                            obj.fk_profile = int.Parse(pair.Value);
                        }
                        if (pair.Key.Equals("referred_at"))
                        {
                            obj.referred_at = DateTime.Parse(pair.Value);
                        }
                        if (pair.Key.Equals("object_key"))
                        {
                            obj.object_key = int.Parse(pair.Value);
                        }
                        if (pair.Key.Equals("profile"))
                        {
                            var profilePair = JsonConvert.DeserializeObject<Dictionary<string, string>>(pair.Value);

                            if (profilePair != null && obj.fk_profile != 0)
                            {
                                obj.fk_profile = ctlProfiles.UpdateAction(profilePair, obj.fk_profile);
                            }
                        }
                    }

                    if ((temp.Update(db, obj) == true))
                    {
                        aResponse.StatusCode = (int)HttpStatusCode.OK;
                        retVal = true;
                    }
                    else
                    {
                        aResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
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

        bool Delete(HttpListenerContext aContext)
        {
            bool retVal = false;

            var aResponse = aContext.Response;
            var aRequest = aContext.Request;

            string arr = "";
            var ok = aRequest.QueryString.Get("object_key");

            if (aRequest.QueryString.HasKeys() == true && ok != null)
            {
                if (temp.Delete(db, int.Parse(ok)))
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

        static bool NotSupported(HttpListenerContext aContext)
        {
            var response = aContext.Response;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ContentType = "text/plain";
            response.OutputStream.Close();
            return false;
        }
    }
}

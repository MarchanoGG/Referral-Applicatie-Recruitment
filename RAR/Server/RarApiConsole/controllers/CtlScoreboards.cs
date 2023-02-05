using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RarApiConsole.controllers
{
    internal class CtlScoreboards
    {
        private DoScoreboard temp = new();
        private DatabaseContext db = new();
        private static CtlScoreboards? instance;

        public CtlScoreboards()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Scoreboards", HandleRequest);
        }

        public static CtlScoreboards Instance()
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
            var okuser = aRequest.QueryString.Get("fk_user");

            if (aRequest.QueryString.HasKeys() == true && ok != null)
            {
                arr = temp.ReadSpecific(db, int.Parse(ok));
            }
            else if (aRequest.QueryString.HasKeys() && okuser != null)
            {
                arr = temp.ReadByUserId(db, int.Parse(okuser));
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

            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if ((aRequest.HasEntityBody == true) && (temp.ValidateInput(keyPair)))
            {
                int objectKey = CreateAction(keyPair);
                if (objectKey > 0)
                {
                    aResponse.StatusCode = (int)HttpStatusCode.OK;
                    arr = temp.ReadSpecific(db, objectKey);
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

            var obj = new DoScoreboard();

            var ctlUsers = CtlUsers.Instance();

            foreach (var pair in aPair)
            {
                if (pair.Value != null)
                {
                    if (pair.Key.Equals("name"))
                    {
                        obj.name = pair.Value;
                    }
                    if (pair.Key.Equals("start_dt"))
                    {
                        obj.start_dt = DateTime.Parse(pair.Value);
                    }
                    if (pair.Key.Equals("end_dt"))
                    {
                        obj.end_dt = DateTime.Parse(pair.Value);
                    }
                    if (pair.Key.Equals("fk_user"))
                    {
                        obj.fk_user = int.Parse(pair.Value);
                    }

                    if (pair.Key.Equals("user"))
                    {
                        var userPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(pair.Value);

                        if (userPair != null)
                        {
                            int userKey = ctlUsers.CreateAction(userPair);
                            if (userKey > 0)
                            {
                                obj.fk_user = userKey;
                            }
                        }
                    }
                }
            }

            if (temp.Create(db, obj) == true)
            {
                retVal = obj.object_key;
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
                    arr = temp.ReadSpecific(db, objectKey);
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

            var obj = new DoScoreboard();

            obj.object_key = aObjectKey;

            var ctlUsers = CtlUsers.Instance();

            foreach (var pair in aPair)
            {
                if (pair.Value != null)
                {
                    if (pair.Key.Equals("name"))
                    {
                        obj.name = pair.Value;
                    }
                    if (pair.Key.Equals("start_dt"))
                    {
                        obj.start_dt = DateTime.Parse(pair.Value);
                    }
                    if (pair.Key.Equals("end_dt"))
                    {
                        obj.end_dt = DateTime.Parse(pair.Value);
                    }
                    if (pair.Key.Equals("fk_user"))
                    {
                        obj.fk_user = int.Parse(pair.Value);
                    }

                    if (pair.Key.Equals("user"))
                    {
                        var userPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(pair.Value);

                        if (userPair != null && obj.fk_user != 0)
                        {
                            obj.fk_user = ctlUsers.UpdateAction(userPair, obj.fk_user);
                        }
                        else if (userPair != null)
                        {
                            obj.fk_user = ctlUsers.CreateAction(userPair);
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

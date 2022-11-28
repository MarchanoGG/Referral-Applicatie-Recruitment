using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;

namespace RarApiConsole.controllers
{
    internal class CtlProfiles
    {
        private DoProfile temp = new();
        private DatabaseContext db = new();

        public CtlProfiles()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Profiles", HandleRequest);
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

            if (aRequest.QueryString.HasKeys() == true)
            {
                arr = temp.ReadSpecific(db, int.Parse(aRequest.QueryString.Get("object_key")));
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
                var obj = new DoProfile();

                foreach (var pair in keyPair)
                {
                    if (pair.Key.Equals("initials"))
                    {
                        obj.initials = pair.Value;
                    }
                    if (pair.Key.Equals("name"))
                    {
                        obj.name = pair.Value;
                    }
                    if (pair.Key.Equals("surname"))
                    {
                        obj.surname = pair.Value;
                    }
                    if (pair.Key.Equals("email"))
                    {
                        obj.email = pair.Value;
                    }
                    if (pair.Key.Equals("phone_number"))
                    {
                        obj.phone_number = pair.Value;
                    }
                    if (pair.Key.Equals("address"))
                    {
                        obj.address = pair.Value;
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

            string arr = "";

            var keyPair = formData.FormData.GetFormData(aRequest);

            if ((aRequest.HasEntityBody == true) && (temp.ValidateInput(keyPair)))
            {
                var obj = new DoProfile();
                bool keyIsSet = false;

                foreach (var pair in keyPair)
                {
                    if (pair.Key.Equals("initials"))
                    {
                        obj.initials = pair.Value;
                    }
                    if (pair.Key.Equals("name"))
                    {
                        obj.name = pair.Value;
                    }
                    if (pair.Key.Equals("surname"))
                    {
                        obj.surname = pair.Value;
                    }
                    if (pair.Key.Equals("email"))
                    {
                        obj.email = pair.Value;
                    }
                    if (pair.Key.Equals("phone_number"))
                    {
                        obj.phone_number = pair.Value;
                    }
                    if (pair.Key.Equals("address"))
                    {
                        obj.address = pair.Value;
                    }
                    if (pair.Key.Equals("object_key"))
                    {
                        obj.object_key = int.Parse(pair.Value);
                        keyIsSet = true;
                    }
                }

                if ((keyIsSet == true) && (temp.Update(db, obj) == true))
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

        bool Delete(HttpListenerContext aContext)
        {
            bool retVal = false;

            var aResponse = aContext.Response;
            var aRequest = aContext.Request;

            string arr = "";

            if (aRequest.QueryString.HasKeys() == true)
            {
                if (temp.Delete(db, int.Parse(aRequest.QueryString.Get("object_key"))))
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

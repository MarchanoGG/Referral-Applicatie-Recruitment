using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;

namespace RarApiConsole.controllers
{
    internal class CtlUsers
    {
        private DoUser ?temp;
        private DatabaseContext db;
        public void Get(HttpListenerContext aContext, HttpListenerRequest aRequest)
        {
            var response = aContext.Response;

            string arr = "";

            if (aRequest.QueryString.HasKeys() == true)
            {
                arr = DoUser.ReadSpecific(db, int.Parse(aRequest.QueryString.Get("object_key")));
            }
            else
            {
                arr = DoUser.ReadAll(db);
            }

            if (arr.Length > 0)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/json";
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NoContent;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(arr);
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.OutputStream.Close();
        }

        static void NotSupported(HttpListenerContext aContext)
        {
            var response = aContext.Response;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ContentType = "text/plain";
            response.OutputStream.Close();
        }
    }
}

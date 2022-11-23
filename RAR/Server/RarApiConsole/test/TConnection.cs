using System.Net;

namespace RAR
{
    class TConnection
    {
        public TConnection()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Systems", System);
        }

        private bool System(HttpListenerContext aContext)
        {
            bool retVal = false;
            var aReq = aContext.Request;
            var aRes = aContext.Response;

            switch (aReq.HttpMethod)
            {
                case "GET":
                    retVal = true;
                    break;
                default:
                    aRes.StatusCode = 403;
                    break;
            }

            return retVal;
        }
    }
}

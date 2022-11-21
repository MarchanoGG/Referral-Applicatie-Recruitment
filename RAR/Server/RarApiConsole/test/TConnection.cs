using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace RAR
{
    class TConnection
    {
        public TConnection()
        {
            TServer.RegisterCallback("/System", System);

        }

        private bool System(HttpListenerRequest aReq, HttpListenerResponse aRes)
        {
            if (aReq.HttpMethod != "GET")
            {
                aRes.StatusCode = 403;
                return false;
            }
            return true;
        }

    }
}

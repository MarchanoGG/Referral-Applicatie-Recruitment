using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RAR
{
    class TServer
    {
        HttpClient d_client = new HttpClient();
        public static bool d_IsServerRunning = false;
        public static HttpListener listener;
        public static HttpListenerRequest aReq;
        public static HttpListenerResponse aRes;

        private static TServer d_instance = null;
        private Form1 form1 = null;
        public String port { get; set; }
        public String server { get; set; }
        public String protocol { get; set; }
        public static String url { get; set; }

        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>API TEST</title>" +
            "  </head>" +
            "  <body>" +
            "    <p>Page Views: {0}</p>" +
            "  </body>" +
            "</html>";

        private static volatile bool Terminated = false;
        private static String s_url;

        private static void Execute()
        {
            TServer srv = TServer.Instance();
            srv.StartServer(s_url);

            //this.d_callbackMethod("SetupServer", url);
            while (Volatile.Equals(Terminated, false))
            {

                // handle some stufff
                Thread.Sleep(1);
            }

            DestroyInstance();
            //srv.StopServer();
            // destroy the server

        }

        public static void ThStartServer(String aURL)
        {
            s_url = aURL;
            Thread t = new Thread(new ThreadStart(Execute));
            t.Start();

        }

        public static void ThStopServer()
        {
            Terminated = true;
        }

        private static Dictionary<String, Func<HttpListenerRequest, HttpListenerResponse, bool>> s_callbackMethod = new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, bool>>();


        public static void RegisterCallback(String aEndpoint,Func<HttpListenerRequest, HttpListenerResponse, bool> aCallback)
        {
            s_callbackMethod.Add(aEndpoint, aCallback);
        }



        private static TServer Instance()
        {
            if (d_instance == null)
            {
                d_instance = new TServer();
            }
            return d_instance;
        }

        private static void DestroyInstance()
        {
            if (d_instance != null)
            {
//                runServer = false;
            }
            d_instance = null;
        }

        private TServer()
        {
        }

        private static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (!Terminated)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                String url = req.RawUrl;

                bool success = true;
                if (s_callbackMethod.ContainsKey(url))
                {
                    success = s_callbackMethod[url](req, resp);
                }

                if (req.HttpMethod == "GET")
                {
                    Console.WriteLine("Someone just connected...");
                }

                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                // Write the response info
                if (success)
                {
                    resp.ContentType = "application/json";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                else // not success
                {
                    resp.ContentType = "application/json";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                

                // Write out to the response stream (asynchronously), then close it
                resp.Close();
            }
        }
        private void StartServer(String aUrl)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(aUrl);
            listener.Start();

            Console.WriteLine("Listening for connections on {0}", aUrl);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }

        //private void StartServer(object sender, EventArgs e)
        //{
        //    Thread th = new Thread(SetupServer);
        //    d_IsServerRunning = true;
        //    //richTextBox1.Text = "Server created";

        //    th.Start();
        //}
    }
}

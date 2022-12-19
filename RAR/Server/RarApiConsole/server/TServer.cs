using System.Net;
using System.Text;

namespace RAR
{
    public class TServer
    {
        HttpClient d_client = new HttpClient();
        public static bool d_IsServerRunning = false;
        public static HttpListener ?listener;
        public static HttpListenerRequest ?aReq;
        public static HttpListenerResponse ?aRes;

        private static TServer ?s_instance = null;
        public String ?port { get; set; }
        public String ?server { get; set; }
        public String ?protocol { get; set; }
        public static String ?url { get; set; }
        public static int s_logging { get; set; }


        private static Dictionary<string, Func<HttpListenerContext, bool>> s_callbackMethod = new Dictionary<string, Func<HttpListenerContext, bool>>();

        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData =
            "<!DOCTYPE>\r\n" +
            "<html>\r\n" +
            "  <head>\r\n" +
            "    <title>Referral Application API</title>\r\n" +
            "  </head>\r\n" +
            "  <body>\r\n" +
            "    <p>Page Views: {0}</p>\r\n" +
            "  </body>\r\n" +
            "</html>";

        private static volatile bool Terminated = false;
        private static String ?s_url;

        public static void Execute()
        {
            TServer srv = TServer.Instance();

            if (s_url != null)
            { 
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

        }

        public static void ThStartServer(String aURL)
        {
            s_url = aURL;
            Thread t = new Thread(new ThreadStart(Execute));
            t.Start();

        }

        public static void thStopServer()
        {
            Terminated = true;
        }

        public void RegisterCallback(string aEndpoint,Func<HttpListenerContext, bool> aCallback)
        {
            s_callbackMethod.Add(aEndpoint, aCallback);
        }

        public static TServer Instance()
        {
            if (s_instance == null)
            {
                s_instance = new TServer();
            }
            return s_instance;
        }

        public static void DestroyInstance()
        {
            if (s_instance != null)
            {
                s_instance = null;
            }
        }

        private TServer()
        {
        }

        private static void HandleIncomingConnections(IAsyncResult result)
        {
            bool runServer = true;

            if ((listener != null) && (listener.IsListening == true))
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = listener.EndGetContext(result);

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;


                if((req.Url != null) && (req.RawUrl != null))
                {
                    if (s_logging == 1)
                    {
                        // Print out some info about the request
                        Console.WriteLine("Request #: {0}", ++requestCount);
                        Console.WriteLine(req.Url.ToString());
                        Console.WriteLine(req.HttpMethod);
                        Console.WriteLine(req.UserHostName);
                        Console.WriteLine(req.UserAgent);
                        Console.WriteLine();
                    }
                    else if (s_logging == 2)
                    {
                        string[] log =
                            {
                            "Request "+ ++requestCount, req.Url.ToString(), req.HttpMethod, req.UserHostName, req.UserAgent
                        };

                        if (!Directory.Exists("logs"))
                        {
                            Directory.CreateDirectory("Logs");
                        }
                        File.WriteAllLinesAsync("logs/Log-" + requestCount + "-" + req.HttpMethod + ".txt", log);
                    }

                    // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                    string url = req.RawUrl;

                    if (url.Contains('?') == true)
                    {
                        url = url.Substring(0, url.IndexOf('?'));
                    }

                    if (ctx.Request.HttpMethod == "OPTIONS")
                    {
                        resp.StatusCode = (int)HttpStatusCode.OK;
                        resp.Headers.Add("Access-Control-Allow-Origin", "*");
                        resp.Headers.Add("Access-Control-Allow-Headers", "content-type");
                        resp.OutputStream.Close();
                    }
                    else if (s_callbackMethod.ContainsKey(url))
                    {
                        s_callbackMethod[url](ctx);
                    }
                    else
                    {
                        // No endpint found/registered, displaying default page
                        string disableSubmit = !runServer ? "disabled" : "";
                        byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                        resp.ContentType = "text/html";
                        resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.OutputStream.Close();
                    }

                    // Make sure we don't increment the page views counter if `favicon.ico` is requested
                    if (req.Url.AbsolutePath != "/favicon.ico")
                    {
                        pageViews += 1;
                    }

                    // Close this callback and open the following
                    listener.BeginGetContext(new AsyncCallback(HandleIncomingConnections), listener);
                }
            }
        }

        public void StartServer(String aUrl)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(aUrl);
            listener.Start();

            Console.WriteLine("Listening for connections on {0}", aUrl);

            // Handle requests
            listener.BeginGetContext(new AsyncCallback(HandleIncomingConnections), listener);
        }
    }
}

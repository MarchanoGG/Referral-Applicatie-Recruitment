using RAR;
using RarApiConsole.providers;

namespace RarApiConsole
{
    class RarApiConsole
    {
        static void Main(string[] args)
        {

            // Initialize configs
            TSettings.Instance();


            if (args.Length > 0)
            {
                if (args.Length > 1)
                {
                    SetLogging(args[1]);
                }

                Options(args);
            }
            else
            {
                // Start server because no arguments are provided
                StartServer();
            }

            // Register all modules
            ControllerProvider.RegisterControllers();
        }

        public static void Options(string[] aVal)
        {
            switch (aVal[0])
            {
                case "start":
                    StartServer();
                    break;
                case "stop":
                    StopServer();
                    break;
                case "kill":
                    KillServer();
                    break;
                case "restart":
                    RestartServer();
                    break;
            }
        }

        static void SetLogging(string aCase)
        {
            TServer.s_logging = aCase switch
            {
                "1" => 1,// Use logging in console
                "2" => 2,// Create log files
                _ => 0,// No logging
            };
        }

        static void StartServer()
        {
            var s = TSettings.Instance();
            var conf = s.GetApplication();

            if (conf != null)
            {
                string URL = conf.Protocol + "://" + conf.Server + ":" + conf.Port + "/";
                TServer.ThStartServer(URL);
            }
        }

        static void StopServer()
        {
            //   RAR.TServer.thStopServer();
        }

        static void KillServer()
        {
            // RAR.TServer.KillServer();
            Environment.Exit(1);
        }

        static void RestartServer()
        {
            //   RAR.TServer.thStopServer();
            //   RAR.TServer.Execute();
        }
    }
}
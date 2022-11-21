using RAR;

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
        new TConnection();

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
        switch (aCase)
        {
            case "1":
                // Use logging in console
                RAR.TServer.s_logging = 1;
                break;

            case "2":
                // Create log files
                RAR.TServer.s_logging = 2;
                break;
            case "0":
            default:
                // No logging
                RAR.TServer.s_logging = 0;
                break;

        }
    }

    static void StartServer()
    {
        var s = TSettings.Instance();
        var conf = s.GetApplication();

        if (conf != null)
        {
            string URL = conf.Protocol + "://" + conf.Server + ":" + conf.Port + "/";
            RAR.TServer.ThStartServer(URL);
        }
    }

    static void StopServer()
    {
        //   RAR.TServer.thStopServer();
    }

    static void KillServer()
    {
        // RAR.TServer.KillServer();
        System.Environment.Exit(1);
    }

    static void RestartServer()
    {
        //   RAR.TServer.thStopServer();
        //   RAR.TServer.Execute();
    }
}
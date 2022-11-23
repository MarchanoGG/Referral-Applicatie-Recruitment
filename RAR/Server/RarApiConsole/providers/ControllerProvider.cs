using RarApiConsole.controllers;
using RAR;

namespace RarApiConsole.providers
{
    internal class ControllerProvider
    {
        public static void RegisterControllers()
        {
            TConnection connection = new();
            CtlUsers users = new();
        }
    }
}

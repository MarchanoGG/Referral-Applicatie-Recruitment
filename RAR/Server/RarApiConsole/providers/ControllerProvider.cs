using RarApiConsole.controllers;
using RAR;

namespace RarApiConsole.providers
{
    internal class ControllerProvider
    {
        public static void RegisterControllers()
        {
            TConnection connection = new();
            CtlAuthentication authentication = new();
            CtlCandidates candidates = new();
            CtlProfiles profiles = new();
            CtlReferrals referrals = new();
            CtlRewards rewards = new();
            CtlScoreboards scoreboards = new();
            CtlTasks tasks = new();
            CtlUsers users = new();
            CtlSystems systems = new();
        }
    }
}

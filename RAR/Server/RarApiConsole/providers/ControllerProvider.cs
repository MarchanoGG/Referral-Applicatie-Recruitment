using RarApiConsole.controllers;
using RAR;

namespace RarApiConsole.providers
{
    internal class ControllerProvider
    {
        public static void RegisterControllers()
        {
            TConnection connection = new();
            CtlAuthentication authentication = CtlAuthentication.Instance();
            CtlCandidates candidates = CtlCandidates.Instance();
            CtlProfiles profiles = CtlProfiles.Instance();
            CtlReferrals referrals = CtlReferrals.Instance();
            CtlRewards rewards = CtlRewards.Instance();
            CtlScoreboards scoreboards = CtlScoreboards.Instance();
            CtlTasks tasks = CtlTasks.Instance();
            CtlUsers users = CtlUsers.Instance();
            CtlSystems systems = CtlSystems.Instance();
        }
    }
}

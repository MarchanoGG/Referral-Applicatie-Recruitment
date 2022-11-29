using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;

namespace RarApiConsole.controllers
{
    internal class CtlSystems
    {
        private DatabaseContext db = new();
        public CtlSystems()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Systems/SeedDatabase", HandleSeeding);
        }

        public bool HandleSeeding(HttpListenerContext aContext)
        {
            if (db is null)
            {
                Console.WriteLine("Database context is null"); 
            }
            else
            {
                if (db.users != null)
                {
                    if (db.users.Count() != 0)
                    {
                        Console.WriteLine("Seeding works with empty database. Database already contains data");
                    }
                    else
                    {
                        db.users.AddRange(GetUsers());
                        Console.WriteLine($"{db.SaveChanges()} rows effected");
                    }
                }
            }

            var response = aContext.Response;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.OutputStream.Close();
            return true;
        }

        private static List<DoUser> GetUsers()
        {
            return new List<DoUser>() {
                new DoUser("admin", "admin", 1)
            };
        }
    }
}

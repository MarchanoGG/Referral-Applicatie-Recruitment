using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;
using System.Security.Cryptography;

namespace RarApiConsole.controllers
{
    internal class CtlSystems
    {
        private DatabaseContext db = new();
        private static CtlSystems? instance;
        public CtlSystems()
        {
            TServer server = TServer.Instance();
            server.RegisterCallback("/Systems/SeedDatabase", HandleSeeding);
        }

        public static CtlSystems Instance()
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
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
                new DoUser("admin", GetHashString("admin"), 1)
            };
        }
        public static byte[] GetHash(string inputString)
        {
            byte[] arr;
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                arr = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }

            return arr;
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
            {
                sb.Append(b.ToString("X2")); // X2 is for Hexadecimal 
            }

            return sb.ToString();
        }
    }
}

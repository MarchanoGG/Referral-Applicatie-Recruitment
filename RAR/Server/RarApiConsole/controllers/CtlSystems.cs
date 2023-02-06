using System.Net;
using System.Text;
using RarApiConsole.providers;
using RarApiConsole.dataObjects;
using RAR;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

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
            server.RegisterCallback("/Systems/UnitTest", UnitTesting);
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

        public bool UnitTesting(HttpListenerContext aContext)
        {
            string arr = "[";

            var response = aContext.Response;


            // Unit test user module
            arr += UserTest(aContext);

            if (arr.Length > 1)
            {
                arr += ",";
            }

            if (arr.Length > 0)
            {
                arr += "]";

                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/json";

                byte[] bytes = Encoding.UTF8.GetBytes(arr);
                response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NoContent;
            }

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

        private string UserTest(HttpListenerContext aContext)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            CtlUsers controller = CtlUsers.Instance();

            Dictionary<string, string> createPair = new Dictionary<string, string>();
            Dictionary<string, string> updatePair = new Dictionary<string, string>();
            Dictionary<string, string> deletePair = new Dictionary<string, string>();

            createPair.Add("Email", "test@test.nl");
            createPair.Add("Password", "test");
            createPair.Add("Recruiter", "0");

            updatePair.Add("Email", "test2@test.nl");

            // Create user
            int createRes = controller.CreateAction(createPair);
            retVal.Add("Created", createRes.ToString());

            // Get user
            bool getRes = controller.GetAction();

            if (getRes == true)
            {
                retVal.Add("Get", "Successfull");
            }
            else
            {
                retVal.Add("Get", "Failed");
            }

            // Update user
            int updateRes = controller.UpdateAction(updatePair, createRes);
            retVal.Add("Updated", updateRes.ToString());

            // Delete user
            bool deleteRes = controller.DeleteAction(updateRes);
            if (deleteRes == true)
            {
                retVal.Add("Delete", "Successfull");
            }
            else
            {
                retVal.Add("Delete", "Failed");
            }

            return JsonConvert.SerializeObject(retVal);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleApp1
{  
    class MyContext : DbContext {
        public DbSet<User> users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("User ID = postgres; Password = Fewnbd6g; Host = localhost; port = 5432; Database = Project C; Pooling = true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("public");
        }
    }
    class User
    {
        [Key]
        public int object_key { get; set; }

        [Column(TypeName = "int")]
        public int? fk_profile { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string username { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string password { get; set; }

        [Column(TypeName = "int")]
        public int recruiter { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime creation_dt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime modification_dt { get; set; }

    }

    internal class Program
    {
        public static int Port = 8080;
        public static HttpListener listener = new HttpListener();
        public static bool killed = false;
        public static MyContext db;
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching database..");
            db = new MyContext();
            Console.Clear();

            listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");

            listener.Start();

            Receive();

            while (killed == false)
            { 
            }
        }

        public static void Stop()
        {
            listener.Stop();
        }

        private static void Receive()
        {
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        }

        private static void ListenerCallback(IAsyncResult result)
        {
            if (listener.IsListening == true)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;

                string className = request.RawUrl.Remove(0, 1);

                if (className.IndexOf('?') >= 0)
                {
                    className = className.Substring(0, className.IndexOf('?'));
                }

                switch (request.HttpMethod)
                {
                    case "GET":
                        Get(className, context, request);
                        break;
                    case "POST":
                        Post(className, context, request);  
                        break;
                    case "PUT":
                        break;
                    case "DELETE":
                        break;
                    default:
                        NotSupported(context);
                        break;
                }

                Receive();
            }
        }

        static void Get(string aClassName, HttpListenerContext aContext, HttpListenerRequest aRequest)
        {
            if (aClassName.CompareTo("Users") == 0)
            {
                var response = aContext.Response;

                string arr = "";

                if (aRequest.QueryString.HasKeys() == true)
                {
                    arr = ReadSpecific(db, int.Parse(aRequest.QueryString.Get("ObjectKey")));
                }
                else
                {
                    arr = ReadAll(db);
                }

                if (arr.Length > 0)
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.ContentType = "application/json";
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.NoContent;
                }

                byte[] bytes = Encoding.UTF8.GetBytes(arr);
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
            else
            {
                NotSupported(aContext);
            }
        }

        static void Post(string aClassName, HttpListenerContext aContext, HttpListenerRequest aRequest)
        {
            if (aClassName.CompareTo("Users") == 0)
            {
                var response = aContext.Response;

                string arr = "";

                if ((aRequest.HasEntityBody == true) && (ValidateUserInput(aRequest)))
                {
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                byte[] bytes = Encoding.UTF8.GetBytes(arr);
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
            else
            {
                NotSupported(aContext);
            }
        }

        static void NotSupported(HttpListenerContext aContext)
        {
            var response = aContext.Response;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ContentType = "text/plain";
            response.OutputStream.Close();
        }

        static string ReadAll(MyContext myDB)
        {
            String arr = "[";
            bool second = false;
            bool aUserFound = false;

            foreach (var user in myDB.users.ToList())
            {
                aUserFound = true;
                if (second == true)
                {
                    arr += ",";
                }
                string json = JsonConvert.SerializeObject(user);

                if (json.Length > 0)
                {
                    arr += json;
                }

                second = true;
            }

            if (aUserFound == true)
            {
                arr += "]";
            }
            else
            {
                arr = "";
            }

            return arr;
        }

        static string ReadSpecific(MyContext myDB, int aObjectKey)
        {
            String arr = "[";
            bool userFound = false;
            foreach (var user in myDB.users.ToList())
            {
                if(user.object_key == aObjectKey)
                {
                    userFound = true;
                    arr += JsonConvert.SerializeObject(user);
                }
            }

            if (userFound == false)
            {
                arr = "";
            }
            else
            {
                arr += "]";
            }
            return arr;
        }

        static bool ValidateUserInput(HttpListenerRequest aRequest)
        {
            bool retVal = false;

            var KeyVal = new Dictionary<string, string>();

            var stream = aRequest.InputStream;
            var type = aRequest.ContentType;

            string boundary = "";
            if (type.IndexOf('=') >= 0)
            {
                boundary = type.Substring(type.IndexOf('=') + 1);
            }

            var encoding = aRequest.ContentEncoding.BodyName;
            Encoding decoder = Encoding.GetEncoding(encoding);

            if ((stream != null) && (decoder != null))
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);

                var aVal = decoder.GetString(ms.ToArray());
                aVal = aVal.Replace(boundary, "");
                aVal = aVal.Replace("\r\n", "");

                while (aVal.Length > 0)
                {
                    if (aVal.IndexOf("=") >= 0)
                    {
                        aVal = aVal.Substring(aVal.IndexOf("=") + 1);
                        if (aVal.IndexOf("--Content-Disposition: form-data; name") >= 0)
                        {
                            var sub = aVal.Substring(0, aVal.IndexOf("--Content-Disposition: form-data; name"));
                            string aKey = sub.Substring(aVal.IndexOf("\"") + 1);
                            string aValue = aKey.Substring(aKey.IndexOf("\"") + 1);
                            aKey = aKey.Substring(0, aKey.IndexOf("\""));

                            KeyVal[aKey] = aValue;
                        }
                        else if (aVal.IndexOf("----") >= 0)
                        {
                            aVal = aVal.Substring(aVal.IndexOf("=") + 1);
                            var sub = aVal.Substring(0, aVal.IndexOf("----"));
                            string aKey = sub.Substring(aVal.IndexOf("\"") + 1);
                            string aValue = aKey.Substring(aKey.IndexOf("\"") + 1);
                            aKey = aKey.Substring(0, aKey.IndexOf("\""));

                            KeyVal[aKey] = aValue;
                        }
                    }
                    else
                    {
                        aVal = "";
                    }
                }
            }
            Console.WriteLine("Received form data:\r\n");
            foreach (var pair in KeyVal)
            {   
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }

            return retVal;
        }

        static void Create(MyContext myDB)
        {
            var user = new User();

            Console.WriteLine("\n\nInsert username:");
            user.username = Console.ReadLine();

            Console.WriteLine("\nInsert password:");
            user.password = Console.ReadLine();

            int highestKey = 0;


            foreach (var temp in myDB.users.ToList())
            {
                if (temp.object_key > highestKey)
                { 
                    highestKey = user.object_key;
                }
            }

            user.object_key = highestKey + 1;
            user.fk_profile = null;
            user.recruiter = 1;
            user.creation_dt = DateTime.Now;
            user.modification_dt = DateTime.Now;

            try
            { 
                var res = myDB.Add<User>(user);

                myDB.SaveChanges();

                Console.WriteLine("\nSuccessfully added new user.");
            }
            catch (Exception ex) 
            {
                Console.WriteLine("\n\nError: " + ex.InnerException.Message);
            }

            Console.WriteLine("\nPress any key to continue..");

            Console.ReadKey();
        }

        static void Update(MyContext myDB)
        {
            Console.WriteLine("\n\nProvide object key: \n");
            var input = Console.ReadLine();

            int key;
            bool isNumeric = int.TryParse(input, out key);

            while (isNumeric == false)
            {
                Console.WriteLine("Please provide a valid key!\n");
                Console.WriteLine("Provide object key: \n");
                input = Console.ReadLine();
                isNumeric = int.TryParse(input, out key);
            }
            if (isNumeric)
            {
                bool userFound = false;
                foreach (var user in myDB.users.ToList())
                {
                    if (user.object_key == key)
                    {
                        Console.WriteLine("\n\nInsert new username:");
                        user.username = Console.ReadLine();

                        user.modification_dt = DateTime.Now;
                        try
                        {
                            userFound = true;

                            myDB.users.Update(user);

                            myDB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                        }
                    }
                }

                if (userFound == false)
                {
                    Console.WriteLine("Could not retrieve user with this object key!");
                }
            }

            Console.WriteLine("\nPress any key to continue..");

            Console.ReadKey();
        }

        static void Delete(MyContext myDB)
        {
            Console.WriteLine("\n\nProvide object key: \n");
            var input = Console.ReadLine();

            int key;
            bool isNumeric = int.TryParse(input, out key);

            while (isNumeric == false)
            {
                Console.WriteLine("Please provide a valid key!\n");
                Console.WriteLine("Provide object key: \n");
                input = Console.ReadLine();
                isNumeric = int.TryParse(input, out key);
            }
            if (isNumeric)
            {
                bool userFound = false;
                foreach (var user in myDB.users.ToList())
                {
                    if (user.object_key == key)
                    {
                        try
                        {
                            userFound = true;

                            myDB.users.Remove(user);

                            myDB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                        }
                    }
                }

                if (userFound == false)
                {
                    Console.WriteLine("Could not retrieve user with this object key!");
                }
            }

            Console.WriteLine("\nPress any key to continue..");

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;

                // do something with the request
                string className = request.RawUrl.Remove(0, 1);

                switch (request.HttpMethod)
                {
                    case "GET":
                        Get(className, context);
                        break;
                    default:
                        NotSupported(context);
                        break;
                }

                Receive();
            }
        }

        static void Get(string aClassName, HttpListenerContext aContext)
        {
            if (aClassName.CompareTo("Users") == 0)
            {
                var response = aContext.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/json";
                string arr = ReadAll(db);

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

            foreach (var user in myDB.users.ToList())
            {
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

            arr += "]";

            return arr;
        }

        static void ReadSpecific(MyContext myDB)
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
                    if(user.object_key == key)
                    {
                        userFound = true;
                        Console.WriteLine("Username:" + user.username + "\n");
                        Console.WriteLine("Creation date:" + user.creation_dt + "\n");
                        Console.WriteLine("Last modification date:" + user.modification_dt + "\n");
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

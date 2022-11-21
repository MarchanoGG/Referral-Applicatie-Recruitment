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
                        Put(className, context, request);
                        break;
                    case "DELETE":
                        Delete(className, context, request);
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
                    arr = ReadSpecific(db, int.Parse(aRequest.QueryString.Get("object_key")));
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

                var keyPair = new Dictionary<string, string>();

                if ((aRequest.HasEntityBody == true) && (ValidateUserInput(aRequest, keyPair)))
                {
                    var obj = new User();

                    foreach (var pair in keyPair)
                    {
                        if (pair.Key.Equals("username"))
                        {
                            obj.username = pair.Value;
                        }
                        if (pair.Key.Equals("password"))
                        {
                            obj.password = pair.Value;
                        }
                        if (pair.Key.Equals("recruiter"))
                        {
                            obj.recruiter = int.Parse(pair.Value);
                        }
                    }

                    if (Create(db, obj) == true)
                    {
                        response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
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

        static void Put(string aClassName, HttpListenerContext aContext, HttpListenerRequest aRequest)
        {
            if (aClassName.CompareTo("Users") == 0)
            {
                var response = aContext.Response;

                string arr = "";

                var keyPair = new Dictionary<string, string>();

                if ((aRequest.HasEntityBody == true) && (ValidateUserInput(aRequest, keyPair)))
                {
                    var obj = new User();
                    bool keyIsSet = false;

                    foreach (var pair in keyPair)
                    {
                        if (pair.Key.Equals("username"))
                        {
                            obj.username = pair.Value;
                        }
                        if (pair.Key.Equals("password"))
                        {
                            obj.password = pair.Value;
                        }
                        if (pair.Key.Equals("recruiter"))
                        {
                            obj.recruiter = int.Parse(pair.Value);
                        }
                        if (pair.Key.Equals("object_key"))
                        {
                            obj.object_key = int.Parse(pair.Value);
                            keyIsSet = true;
                        }
                    }

                    if ((keyIsSet == true) && (Update(db, obj) == true))
                    {
                        response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
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

        static void Delete(string aClassName, HttpListenerContext aContext, HttpListenerRequest aRequest)
        {
            if (aClassName.CompareTo("Users") == 0)
            {
                var response = aContext.Response;

                string arr = "";

                if (aRequest.QueryString.HasKeys() == true)
                {
                    if (Delete(db, int.Parse(aRequest.QueryString.Get("object_key"))))
                    {
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.ContentType = "application/json";
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
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

        static bool ValidateUserInput(HttpListenerRequest aRequest, Dictionary<string, string> aPair)
        {
            bool retVal = false;
            
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
                        retVal = true;
                        aVal = aVal.Substring(aVal.IndexOf("=") + 1);
                        if (aVal.IndexOf("--Content-Disposition: form-data; name") >= 0)
                        {
                            var sub = aVal.Substring(0, aVal.IndexOf("--Content-Disposition: form-data; name"));
                            string aKey = sub.Substring(aVal.IndexOf("\"") + 1);
                            string aValue = aKey.Substring(aKey.IndexOf("\"") + 1);
                            aKey = aKey.Substring(0, aKey.IndexOf("\""));

                            aPair[aKey] = aValue;
                        }
                        else if (aVal.IndexOf("----") >= 0)
                        {
                            aVal = aVal.Substring(aVal.IndexOf("=") + 1);
                            var sub = aVal.Substring(0, aVal.IndexOf("----"));
                            string aKey = sub.Substring(aVal.IndexOf("\"") + 1);
                            string aValue = aKey.Substring(aKey.IndexOf("\"") + 1);
                            aKey = aKey.Substring(0, aKey.IndexOf("\""));

                            aPair[aKey] = aValue;
                        }
                    }
                    else
                    {
                        aVal = "";
                    }
                }
            }

            return retVal;
        }

        static bool Create(MyContext myDB, User aObject)
        {
            bool retVal = false; 
            int highestKey = 0;

            foreach (var temp in myDB.users.ToList())
            {
                if (temp.object_key > highestKey)
                { 
                    highestKey = temp.object_key;
                }
            }

            aObject.object_key = highestKey + 1;
            aObject.creation_dt = DateTime.Now;
            aObject.modification_dt = DateTime.Now;

            try
            { 
                var res = myDB.Add<User>(aObject);

                myDB.SaveChanges();
                retVal = true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("\n\nError: " + ex.InnerException.Message);
            }
            return retVal;
        }

        static bool Update(MyContext myDB, User aObject)
        {
            bool userFound = false;
            foreach (var user in myDB.users.ToList())
            {
                if (user.object_key == aObject.object_key)
                {
                    userFound = true;
                    myDB.Entry(user).State = EntityState.Detached;
                }
            }
            if (userFound == true)
            { 
                aObject.modification_dt = DateTime.Now;
                try
                {
                    myDB.Update<User>(aObject);
                    myDB.Entry(aObject).State = EntityState.Modified;

                    myDB.SaveChanges();

                    userFound = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                } 
            }

            return userFound;
        }

        static bool Delete(MyContext myDB, int aObjectKey)
        {
            bool userFound = false;
            foreach (var user in myDB.users.ToList())
            {
                if (user.object_key == aObjectKey)
                {
                    myDB.Entry(user).State = EntityState.Detached;
                    try
                    {
                        myDB.Remove<User>(user);
                        myDB.Entry(user).State = EntityState.Deleted;

                        myDB.SaveChanges();

                        userFound = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                    }
                }
            }
            return userFound;
        }
    }
}

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
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching database..");
            var db = new MyContext();
            Console.Clear();

            WriteMenu();
            ConsoleKeyInfo KeyInput = new ConsoleKeyInfo();

            while (KeyInput.Key != ConsoleKey.Escape)
            {
                KeyInput = Console.ReadKey();
                switch (KeyInput.Key)
                {
                    case ConsoleKey.D1:
                        ReadAll(db);
                        break;
                    case ConsoleKey.D2:
                        ReadSpecific(db);
                        break;
                    case ConsoleKey.D3:
                        Create(db);
                        break;
                    case ConsoleKey.D4:
                        Update(db);
                        break;
                    case ConsoleKey.D5:
                        Delete(db);
                        break;
                    default:
                        break;
                }
                Console.Clear();
                WriteMenu();
            }
        }

        static void WriteMenu()
        {
            Console.WriteLine("Select an action: \n");
            Console.WriteLine("1 - Show all users");
            Console.WriteLine("2 - Show distinct user");
            Console.WriteLine("3 - Create new user");
            Console.WriteLine("4 - Update existing user");
            Console.WriteLine("5 - Delete existing user");
            Console.WriteLine("\n You can leave by pressing the escape button");
            Console.WriteLine("Selected option:\n");
        }

        static void ReadAll(MyContext myDB)
        {
            Console.WriteLine("\n\nRetrieving all users..");

            foreach (var user in myDB.users.ToList())
            {
                Console.WriteLine("User:" + user.username + " - Object key: " + user.object_key + "\n");
            }

            Console.WriteLine("\nPress any key to continue..");

            Console.ReadKey();
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

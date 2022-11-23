using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.dataObjects
{
    internal class DoUser
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

        public DoUser() 
        {
            username = "temp";
            password = "temp";
        }

        public DoUser(string Username, string Password)
        {
            username = Username;
            password = Password;
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            bool retVal = true;

            return retVal;  
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if(myDB.users != null)
            {
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
            }
            else
            {
                arr = "";
            }

            return arr;
        }

        public string ReadSpecific(DatabaseContext myDB, int aObjectKey)
        {
            string arr = "[";

            if (myDB.users != null)
            {
                bool userFound = false;
                foreach (var user in myDB.users.ToList())
                {
                    if (user.object_key == aObjectKey)
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
            }
            else
            {
                arr = "";
            }

            return arr;
        }

        public bool Create(DatabaseContext myDB, DoUser aObject)
        {
            bool retVal = false;
            int highestKey = 0;

            if (myDB.users != null)
            {
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
                    var res = myDB.Add<DoUser>(aObject);

                    myDB.SaveChanges();
                    retVal = true;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("\n\nError: " + ex.Message);
                    }
                }
            }

            return retVal;
        }

        public bool Update(DatabaseContext myDB, DoUser aObject)
        {
            bool userFound = false;

            if (myDB.users != null)
            {
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
                        myDB.Update<DoUser>(aObject);
                        myDB.Entry(aObject).State = EntityState.Modified;

                        myDB.SaveChanges();

                        userFound = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                        }
                        else
                        {
                            Console.WriteLine("\n\nError: " + ex.Message);
                        }
                    }
                }
            }

            return userFound;
        }

        public bool Delete(DatabaseContext myDB, int aObjectKey)
        {
            bool userFound = false;

            if (myDB.users != null)
            {
                foreach (var user in myDB.users.ToList())
                {
                    if (user.object_key == aObjectKey)
                    {
                        myDB.Entry(user).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoUser>(user);
                            myDB.Entry(user).State = EntityState.Deleted;

                            myDB.SaveChanges();

                            userFound = true;
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine("\n\nError: " + ex.InnerException.Message);
                            }
                            else
                            {
                                Console.WriteLine("\n\nError: " + ex.Message);
                            }
                        }
                    }
                }
            }

            return userFound;
        }
    }
}

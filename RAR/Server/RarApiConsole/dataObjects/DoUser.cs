using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.dataObjects
{
    internal class DoUser
    {
        [Key, Required]
        public int object_key { get; set; }

        [ForeignKey("DoProfile")]
        public int? fk_profile { get; set; }

        [Column(TypeName = "varchar(100)"), Required]
        public string username { get; set; }

        [Column(TypeName = "varchar(100)"), Required]
        public string password { get; set; }

        [Column(TypeName = "int"), Required]
        public int recruiter { get; set; }

        [Column(TypeName = "timestamp"), Required]
        public DateTime creation_dt { get; set; }

        [Column(TypeName = "timestamp"), Required]
        public DateTime modification_dt { get; set; }

        public DoProfile ?profile;


        public DoUser() 
        {
            username = "temp";
            password = "temp";
        }

        public DoUser(string Username, string Password, int Recruiter)
        {
            username = Username;
            password = Password;
            recruiter = Recruiter;
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            bool retVal = false;

            if (aPair.ContainsKey("username") &&
                aPair["username"].Length > 0 &&
                aPair.ContainsKey("password") &&
                aPair["password"].Length > 0)
            {
                retVal = true;
            }

            return retVal;  
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if(myDB.users != null)
            {
                bool second = false;
                bool found = false;

                foreach (var obj in myDB.users.ToList())
                {
                    found = true;
                    if (second == true)
                    {
                        arr += ",";
                    }

                    foreach (var profile in myDB.profiles.ToList())
                    {
                        if (profile.object_key == obj.fk_profile)
                        {
                            obj.profile = profile;
                        }
                    }    

                    string json = JsonConvert.SerializeObject(obj);

                    if (json.Length > 0)
                    {
                        arr += json;
                    }

                    second = true;
                }

                if (found == true)
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
                bool found = false;
                foreach (var obj in myDB.users.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        found = true;
                        arr += JsonConvert.SerializeObject(obj);
                    }
                }

                if (found == false)
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
            bool found = false;

            if (myDB.users != null)
            {
                foreach (var obj in myDB.users.ToList())
                {
                    if (obj.object_key == aObject.object_key)
                    {
                        found = true;
                        myDB.Entry(obj).State = EntityState.Detached;
                    }
                }
                if (found == true)
                {
                    aObject.modification_dt = DateTime.Now;
                    try
                    {
                        myDB.Update<DoUser>(aObject);
                        myDB.Entry(aObject).State = EntityState.Modified;

                        myDB.SaveChanges();

                        found = true;
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

            return found;
        }

        public bool Delete(DatabaseContext myDB, int aObjectKey)
        {
            bool found = false;

            if (myDB.users != null)
            {
                foreach (var obj in myDB.users.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        myDB.Entry(obj).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoUser>(obj);
                            myDB.Entry(obj).State = EntityState.Deleted;

                            myDB.SaveChanges();

                            found = true;
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

            return found;
        }
    }
}

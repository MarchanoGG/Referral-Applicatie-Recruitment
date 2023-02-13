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

        private DatabaseContext db = new();
        public DoProfile ?profile
        {
            get
            {
                if (db.profiles.Find(fk_profile) != null)
                {
                    return db.profiles.Find(fk_profile);
                }
                else 
                {
                    return new DoProfile();
                }
            }
        }

        [Column(TypeName = "varchar(100)"), Required]
        [NotMapped]
        public string sessiontoken { get; set; } = "";
        [NotMapped]
        public string creation_dt_str
        {
            get
            {
                return creation_dt.ToString(@"yyyy\/MM\/dd");
            }
        }
        [NotMapped]
        public string modification_dt_str
        {
            get
            {
                return modification_dt.ToString(@"yyyy\/MM\/dd");
            }
        }
        public int totalPoints
        {
            get
            {
                int total = 0;
                foreach (var row in db.referrals.Where(a => a.fk_user == object_key))
                {
                    if (row != null && row.task != null)
                    {
                        total+= row.task.points;
                    }
                }
                return total;
            }
        }
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
            creation_dt = DateTime.Now;
            modification_dt = DateTime.Now;
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
        public string ReadByScoreboard(DatabaseContext myDB, int aObjectKey)
        {
            string arr = "[";

            if (myDB.users != null)
            {
                bool second = false;
                bool found = false;
                // all members attached to current obj scoreboard 
                var userRefQuery = from referral in myDB.referrals
                                   join user in myDB.users on referral.fk_user equals user.object_key
                                   where referral.fk_scoreboard == aObjectKey
                                   group user by user.object_key into userRef
                                   select userRef.FirstOrDefault();
                foreach (var obj in userRefQuery)
                {
                    found = true;
                    if (second == true)
                    {
                        arr += ",";
                    }
                    string json = JsonConvert.SerializeObject(obj);

                    if (json.Length > 0)
                    {
                        arr += json;
                    }

                    second = true;
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

        public string ReadSpecific(DatabaseContext myDB, string token)
        {
            string arr = "[";

            if (myDB.users != null)
            {
                bool found = false;
                foreach (var obj in myDB.users.ToList())
                {
                    if (obj.sessiontoken == token)
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

        public DoUser ReadSpecificObject(DatabaseContext myDB, int aObjectKey)
        {
            DoUser retVal = new DoUser();

            if (myDB.users != null)
            {
                foreach (var obj in myDB.users.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        retVal = obj;
                    }
                }
            }

            return retVal;
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

                        myDB.ChangeTracker.Clear();

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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RarApiConsole.dataObjects
{
    internal class DoScoreboard
    {
        [Key]
        public int object_key { get; set; }

        [ForeignKey("DoUser")]
        public int fk_user { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string name { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime start_dt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime ?end_dt { get; set; }
        private DatabaseContext db = new();
        public List<DoUser> ranklist = new List<DoUser>();

        public DoScoreboard()
        {
            fk_user = 0;
            name = "temp";
            start_dt = DateTime.Now;
        }

        public DoScoreboard(int UserKey, string Name, DateTime StartDate)
        {
            fk_user = UserKey;
            name = Name;
            start_dt = StartDate;
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            bool retVal = false;

            if (aPair.ContainsKey("fk_user") &&
                aPair["fk_user"].Length > 0 && 
                aPair.ContainsKey("name") &&
                aPair["name"].Length > 0 &&
                aPair.ContainsKey("start_dt") &&
                aPair["start_dt"].Length > 0)
            {
                retVal = true;
            }

            return retVal;
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if (myDB.scoreboards != null)
            {
                bool second = false;
                bool found = false;

                foreach (var obj in myDB.scoreboards.ToList())
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
        public string ReadByUserId(DatabaseContext myDB, int aObjectKey)
        {
            string arr = "[";

            if (myDB.scoreboards != null)
            {
                bool second = false;
                bool found = false;
                // all scoreboards attached to aObjectKey 
                var refQuery = from referral in myDB.referrals
                               where referral.fk_user == aObjectKey
                               group referral.scoreboard by referral.fk_scoreboard into sbgroup
                               select sbgroup.FirstOrDefault();
                foreach (var obj in refQuery.ToList())
                {
                    if (second == true)
                    {
                        arr += ",";
                    }
                    // all members attached to current obj scoreboard 
                    var userRefQuery = from referral in myDB.referrals
                                       where referral.fk_scoreboard == obj.object_key
                                       group referral.user by referral.fk_user into userRef
                                       select userRef.FirstOrDefault();
                    obj.ranklist = new List<DoUser>();
                    foreach (var userRef in userRefQuery.ToList())
                    {
                        if (userRef != null && userRef.recruiter == 0)
                        {
                            obj.ranklist.Add(userRef);
                        }
                    }
                    found = true;
                    arr += JsonConvert.SerializeObject(obj);
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

            if (myDB.scoreboards != null)
            {
                bool found = false;
                foreach (var obj in myDB.scoreboards.ToList())
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

        public bool Create(DatabaseContext myDB, DoScoreboard aObject)
        {
            bool retVal = false;
            int highestKey = 0;

            if (myDB.scoreboards != null)
            {
                foreach (var temp in myDB.scoreboards.ToList())
                {
                    if (temp.object_key > highestKey)
                    {
                        highestKey = temp.object_key;
                    }
                }

                aObject.object_key = highestKey + 1;

                try
                {
                    var res = myDB.Add<DoScoreboard>(aObject);

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

        public bool Update(DatabaseContext myDB, DoScoreboard aObject)
        {
            bool found = false;

            if (myDB.scoreboards != null)
            {
                foreach (var obj in myDB.scoreboards.ToList())
                { 
                    if (obj.object_key == aObject.object_key)
                    {
                        found = true;
                        myDB.Entry(obj).State = EntityState.Detached;
                    }
                }
                if (found == true)
                {
                    try
                    {
                        myDB.Update<DoScoreboard>(aObject);
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

            if (myDB.scoreboards != null)
            {
                foreach (var obj in myDB.scoreboards.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        myDB.Entry(obj).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoScoreboard>(obj);
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

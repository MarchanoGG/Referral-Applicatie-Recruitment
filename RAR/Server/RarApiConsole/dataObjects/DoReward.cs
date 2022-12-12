using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.dataObjects
{
    internal class DoReward
    {
        [Key]
        public int object_key { get; set; }

        [ForeignKey("DoUser")]
        public int fk_user { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string name { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime award_dt { get; set; }

        public DoReward()
        {
            fk_user = 0;
            name = "temp";
            award_dt = DateTime.Now;
        }

        public DoReward(int UserKey, string Name, DateTime AwardDate)
        {
            fk_user = UserKey;
            name = Name;
            award_dt = AwardDate;
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            bool retVal = false;

            if (aPair.ContainsKey("fk_user") &&
                aPair["fk_user"].Length > 0 &&
                aPair.ContainsKey("name") &&
                aPair["name"].Length > 0 &&
                aPair.ContainsKey("award_dt") &&
                aPair["award_dt"].Length > 0)
            {
                retVal = true;
            }

            return retVal;
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if (myDB.rewards != null)
            {
                bool second = false;
                bool found = false;

                foreach (var obj in myDB.rewards.ToList())
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

        public string ReadSpecific(DatabaseContext myDB, int aObjectKey)
        {
            string arr = "[";

            if (myDB.rewards != null)
            {
                bool found = false;
                foreach (var obj in myDB.rewards.ToList())
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

        public bool Create(DatabaseContext myDB, DoReward aObject)
        {
            bool retVal = false;
            int highestKey = 0;

            if (myDB.rewards != null)
            {
                foreach (var temp in myDB.rewards.ToList())
                {
                    if (temp.object_key > highestKey)
                    {
                        highestKey = temp.object_key;
                    }
                }

                aObject.object_key = highestKey + 1;

                try
                {
                    var res = myDB.Add<DoReward>(aObject);

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

        public bool Update(DatabaseContext myDB, DoReward aObject)
        {
            bool found = false;

            if (myDB.rewards != null)
            {
                foreach (var task in myDB.rewards.ToList())
                { 
                    if (task.object_key == aObject.object_key)
                    {
                        found = true;
                        myDB.Entry(task).State = EntityState.Detached;
                    }
                }
                if (found == true)
                {
                    try
                    {
                        myDB.Update<DoReward>(aObject);
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

            if (myDB.rewards != null)
            {
                foreach (var obj in myDB.rewards.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        myDB.Entry(obj).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoReward>(obj);
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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.dataObjects
{
    internal class DoTask
    {
        [Key]
        public int object_key { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string name { get; set; }

        [Column(TypeName = "int")]
        public int points { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string ? description { get; set; }

        public DoTask()
        {
            name = "temp";
            points = 0;
        }

        public DoTask(string Name, int Points)
        {
            name = Name;
            points = Points;
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            bool retVal = false;

            if (aPair.ContainsKey("name") &&
                aPair["name"].Length > 0)
            {
                retVal = true;
            }

            return retVal;
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if (myDB.tasks != null)
            {
                bool second = false;
                bool found = false;

                foreach (var obj in myDB.tasks.ToList())
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

            if (myDB.tasks != null)
            {
                bool found = false;
                foreach (var obj in myDB.tasks.ToList())
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

        public string ReadByScoreboard(DatabaseContext myDB, int aObjectKey)
        {
            string arr = "[";

            if (myDB.tasks != null)
            {
                var items = from task in myDB.tasks
                            join referral in myDB.referrals on task.object_key equals referral.fk_task
                            where referral.fk_scoreboard == aObjectKey
                            group task by task.object_key into reftask
                            select reftask.FirstOrDefault();
                bool second = false;
                bool found = false;
                foreach (var obj in items)
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

        public bool Create(DatabaseContext myDB, DoTask aObject)
        {
            bool retVal = false;
            int highestKey = 0;

            if (myDB.tasks != null)
            {
                foreach (var temp in myDB.tasks.ToList())
                {
                    if (temp.object_key > highestKey)
                    {
                        highestKey = temp.object_key;
                    }
                }

                aObject.object_key = highestKey + 1;

                try
                {
                    var res = myDB.Add<DoTask>(aObject);

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

        public bool Update(DatabaseContext myDB, DoTask aObject)
        {
            bool found = false;

            if (myDB.tasks != null)
            {
                foreach (var obj in myDB.tasks.ToList())
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
                        myDB.Update<DoTask>(aObject);
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

            if (myDB.tasks != null)
            {
                foreach (var obj in myDB.tasks.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        myDB.Entry(obj).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoTask>(obj);
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

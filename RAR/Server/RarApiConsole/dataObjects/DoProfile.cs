using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.dataObjects
{
    internal class DoProfile
    {
        [Key]
        public int object_key { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? initials
        {
            get
            {
                return $"{name?[0]}.{surname?[0]}.";
            }
            set
            {
                initials = $"{name?[0]}.{surname?[0]}.";
            }
        }

        [Column(TypeName = "varchar(40)")]
        public string? name { get; set; }

        [Column(TypeName = "varchar(70)")]
        public string? surname { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string? email { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? phone_number { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? address { get; set; }
        public string? fullname { 
            get { return $"{name} {surname}"; }
        }


        public DoProfile()
        {
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            return true;
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if (myDB.profiles != null)
            {
                bool second = false;
                bool found = false;

                foreach (var obj in myDB.profiles.ToList())
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

            if (myDB.profiles != null)
            {
                bool found = false;
                foreach (var obj in myDB.profiles.ToList())
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

        public bool Create(DatabaseContext myDB, DoProfile aObject)
        {
            bool retVal = false;
            int highestKey = 0;

            if (myDB.profiles != null)
            {
                foreach (var temp in myDB.profiles.ToList())
                {
                    if (temp.object_key > highestKey)
                    {
                        highestKey = temp.object_key;
                    }
                }

                aObject.object_key = highestKey + 1;

                try
                {
                    var res = myDB.Add<DoProfile>(aObject);

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

        public bool Update(DatabaseContext myDB, DoProfile aObject)
        {
            bool found = false;

            if (myDB.profiles != null)
            {
                foreach (var task in myDB.profiles.ToList())
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
                        myDB.Update<DoProfile>(aObject);
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

            if (myDB.profiles != null)
            {
                foreach (var obj in myDB.profiles.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        myDB.Entry(obj).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoProfile>(obj);
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

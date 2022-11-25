﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;
using Microsoft.EntityFrameworkCore;

namespace RarApiConsole.dataObjects
{
    internal class DoReferral
    {
        [Key]
        public int object_key { get; set; }

        [Column(TypeName = "int")]
        public int fk_user { get; set; }

        [Column(TypeName = "int")]
        public int fk_task { get; set; }

        [Column(TypeName = "int")]
        public int fk_candidate { get; set; }

        [Column(TypeName = "int")]
        public int fk_scoreboard { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime creation_dt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime modification_dt { get; set; }

        public DoReferral()
        {
            fk_user = 0;
            fk_task = 0;
            fk_candidate = 0;
            fk_scoreboard = 0;
            creation_dt = DateTime.Now;
            modification_dt = DateTime.Now;
        }

        public DoReferral(int UserKey, int TaskKey, int CandidateKey, int ScoreboardKey, DateTime CreationDate, DateTime ModificationDate)
        {
            fk_user = UserKey;
            fk_task = TaskKey;
            fk_candidate = CandidateKey;
            fk_scoreboard = ScoreboardKey;
            creation_dt = CreationDate;
            modification_dt = ModificationDate;
        }

        public bool ValidateInput(Dictionary<string, string> aPair)
        {
            bool retVal = false;

            if (aPair.ContainsKey("fk_user") &&
                aPair["fk_user"].Length > 0 &&
                aPair.ContainsKey("fk_task") &&
                aPair["fk_task"].Length > 0 &&
                aPair.ContainsKey("fk_candidate") &&
                aPair["fk_candidate"].Length > 0 &&
                aPair.ContainsKey("fk_scoreboard") &&
                aPair["fk_scoreboard"].Length > 0 &&
                aPair.ContainsKey("creation_dt") &&
                aPair["creation_dt"].Length > 0 &&
                aPair.ContainsKey("modification_dt") &&
                aPair["modification_dt"].Length > 0)
            {
                retVal = true;
            }

            return retVal;
        }

        public string ReadAll(DatabaseContext myDB)
        {
            string arr = "[";

            if (myDB.referrals != null)
            {
                bool second = false;
                bool found = false;

                foreach (var obj in myDB.referrals.ToList())
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

            if (myDB.referrals != null)
            {
                bool found = false;
                foreach (var obj in myDB.referrals.ToList())
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

        public bool Create(DatabaseContext myDB, DoReferral aObject)
        {
            bool retVal = false;
            int highestKey = 0;

            if (myDB.referrals != null)
            {
                foreach (var temp in myDB.referrals.ToList())
                {
                    if (temp.object_key > highestKey)
                    {
                        highestKey = temp.object_key;
                    }
                }

                aObject.object_key = highestKey + 1;

                try
                {
                    var res = myDB.Add<DoReferral>(aObject);

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

        public bool Update(DatabaseContext myDB, DoReferral aObject)
        {
            bool found = false;

            if (myDB.referrals != null)
            {
                foreach (var obj in myDB.referrals.ToList())
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
                        myDB.Update<DoReferral>(aObject);
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

            if (myDB.tasks != null)
            {
                foreach (var obj in myDB.referrals.ToList())
                {
                    if (obj.object_key == aObjectKey)
                    {
                        myDB.Entry(obj).State = EntityState.Detached;
                        try
                        {
                            myDB.Remove<DoReferral>(obj);
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

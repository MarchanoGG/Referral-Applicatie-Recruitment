using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RarApiConsole.providers;

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

        public DoUser(string Username, string Password)
        {
            username = Username;
            password = Password;
        }

        public string ReadAll(DatabaseContext myDB)
        {
            String arr = "[";

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
            String arr = "[";

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
    }
}

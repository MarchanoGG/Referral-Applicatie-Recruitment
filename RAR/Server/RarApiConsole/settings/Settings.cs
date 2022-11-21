using System;
using Newtonsoft.Json;
using System.IO;

namespace RAR
{
	public class TSettings
	{
		private static TSettings s_instance = null;
		private string d_path = Directory.GetCurrentDirectory() + "\\config\\";

		public static TSettings Instance()
		{
			if (s_instance == null)
			{
				s_instance = new TSettings();
			}
			return s_instance;
		}

		private TSettings()
		{
			if (!Directory.Exists(d_path))
			{
				Directory.CreateDirectory(d_path);
			}

			if (!File.Exists(d_path + "application.json"))
			{
				string conf = "{\"Protocol\" : \"http\", \"Server\" : \"localhost\", \"Port\" : 8001}";
				File.WriteAllText(d_path + "application.json", conf);
			}

			if (!File.Exists(d_path + "database.json"))
			{
				string conf = "{\"UserID\" : \"postgres\", \"Password\" : \"Fewnbd6g\", \"Host\" : \"localhost\", \"port\" : 5432, \"Database\" : \"Project C\", \"Pooling\" : true}";
				File.WriteAllText(d_path + "database.json", conf);
			}
		}

		public ApplicationConf GetApplication()
		{			
			using (StreamReader r = new StreamReader(d_path + "application.json"))
			{
				string json = r.ReadToEnd();
				var obj = JsonConvert.DeserializeObject<ApplicationConf>(json);
				return obj;
			}
			return null;

		}

		public DatabaseConf GetDataBase()
		{
			using (StreamReader r = new StreamReader(d_path + "database.json"))
			{
				string json = r.ReadToEnd();
				var obj = JsonConvert.DeserializeObject<DatabaseConf>(json);
				return obj;
			}
			return null;

		}

		public class ApplicationConf
		{
			public string Protocol { get; set; }
			public string Server { get; set; }
			public int Port { get; set; }	}

		public class DatabaseConf
		{
			public string UserID { get; set; }
			public string Password { get; set; }
			public string Host { get; set; }
			public string Port { get; set; }
			public string DataBase { get; set; }
			public bool Pooling { get; set; }

		}
	}
}

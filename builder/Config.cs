using System.IO;
using System.Text;

namespace JarToExeBuilder
{
	class Config
	{
		public static string cfgPath = Program.APP_PATH + "cfg.properties";

		private string javaPath = "java";

		private string errorMsg = "未能找到可用的java运行环境（jre）！请先安装java8运行环境！";

		private string preArgs = "";

		private bool writeLogToFile = false;

		private string logFilePath = "error.log";

		private bool winapp = true;

		/// <summary>
		/// 全局配置实例
		/// </summary>
		private static Config configInstance = new Config();

		public string JavaPath
		{
			get
			{
				return javaPath;
			}

			set
			{
				javaPath = value;
			}
		}

		public string ErrorMsg
		{
			get
			{
				return errorMsg;
			}

			set
			{
				errorMsg = value;
			}
		}

		public string PreArgs
		{
			get
			{
				return preArgs;
			}

			set
			{
				preArgs = value;
			}
		}

		public bool WriteLogToFile
		{
			get
			{
				return writeLogToFile;
			}

			set
			{
				writeLogToFile = value;
			}
		}

		public string LogFilePath
		{
			get
			{
				return logFilePath;
			}

			set
			{
				logFilePath = value;
			}
		}

		public bool Winapp
		{
			get
			{
				return winapp;
			}

			set
			{
				winapp = value;
			}
		}

		private Config()
		{

		}

		/// <summary>
		/// 获取全局配置
		/// </summary>
		/// <returns>全局配置</returns>
		public static Config GetConfig()
		{
			string[] cfgs = File.ReadAllLines(cfgPath, Encoding.UTF8);
			foreach (string cfg in cfgs)
			{
				if (cfg.Trim().StartsWith("#") || !cfg.Contains("="))
				{
					continue;
				}
				string key = cfg.Substring(0, cfg.IndexOf("=")).Trim();
				string value = "";
				if (!cfg.Trim().EndsWith("="))
				{
					value = cfg.Substring(cfg.IndexOf("=") + 1).Trim();
				}
				if (key.Equals("javapath"))
				{
					configInstance.javaPath = value;
				}
				if (key.Equals("errormsg"))
				{
					configInstance.errorMsg = value;
				}
				if (key.Equals("preArgs"))
				{
					configInstance.preArgs = value;
				}
				if (key.Equals("logFilePath"))
				{
					configInstance.logFilePath = value;
				}
				if (key.Equals("writeLogToFile"))
				{
					if (value.ToLower().Equals("true"))
					{
						configInstance.writeLogToFile = true;
					}
					else
					{
						configInstance.writeLogToFile = false;
					}
				}
				if (key.Equals("winapp"))
				{
					if (value.ToLower().Equals("true"))
					{
						configInstance.winapp = true;
					}
					else
					{
						configInstance.winapp = false;
					}
				}
			}
			return configInstance;
		}
	}
}
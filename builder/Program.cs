using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JarToExeBuilder
{
	class Program
	{

		/// <summary>
		/// 自己所在目录
		/// </summary>
		public static readonly string APP_PATH = AppDomain.CurrentDomain.BaseDirectory;

		/// <summary>
		/// 构建临时目录
		/// </summary>
		private static readonly string BUILD_TMP_PATH = APP_PATH + "tmp\\";

		/// <summary>
		/// 是否是32位应用程序
		/// </summary>
		private static bool isWin32App = true;

		/// <summary>
		/// 是否是引用模式
		/// </summary>
		private static bool isInclude = false;

		/// <summary>
		/// 是否有图标
		/// </summary>
		private static bool hasIcon = false;

		/// <summary>
		/// g++命令的参数列表
		/// </summary>
		private static List<string> gccArgList = new List<string>();

		/// <summary>
		/// objdump命令参数列表
		/// </summary>
		private static List<string> objdumpArgList = new List<string>();

		/// <summary>
		/// windres命令参数列表
		/// </summary>
		private static List<string> windresArgList = new List<string>();

		static void Main(string[] args)
		{
			int cfgOption = Array.IndexOf(args, "-c");
			if (cfgOption != -1 && cfgOption != args.Length - 1)
			{
				string cfgFile = args[cfgOption + 1];
				if (File.Exists(cfgFile))
				{
					Config.cfgPath = cfgFile;
				}
			}
			Config cfg = Config.GetConfig();
			int jarFileOption = Array.IndexOf(args, "-j");
			int outputFileOption = Array.IndexOf(args, "-o");
			int platformOption = Array.IndexOf(args, "-p");
			int reModeOption = Array.IndexOf(args, "-re");
			int inModeOption = Array.IndexOf(args, "-in");
			if (reModeOption != -1 && inModeOption == -1)
			{
				isInclude = false;
			}
			else if (reModeOption == -1 && inModeOption != -1)
			{
				isInclude = true;
			}
			else
			{
				Console.WriteLine("-in和-re必须有且只能指定一个！");
				return;
			}
			if (jarFileOption == -1 || jarFileOption == args.Length - 1 || outputFileOption == -1 || outputFileOption == args.Length - 1 || platformOption != -1 || platformOption == args.Length - 1)
			{
				Console.WriteLine("参数错误！必须指定参数-j,-o,-p以及它们的值！\r\n");
				JarUtils.printHelp();
				return;
			}
			gccArgList.Add("-finput-charset=UTF-8");
			gccArgList.Add("-fexec-charset=GBK");
			string jarFilePath = args[jarFileOption + 1];
			string outputPath = args[outputFileOption + 1];
			gccArgList.Add("-o");
			gccArgList.Add(outputPath);
			string platformValue = args[platformOption + 1];
			if (platformValue.Equals("i386"))
			{
				if (isInclude)
				{
					Console.WriteLine("构建32位应用程序时不可以使用包含模式！");
					return;
				}
				isWin32App = true;
				gccArgList.Add("-m32");
			}
			else if (platformOption.Equals("x64"))
			{
				isWin32App = false;
				gccArgList.Add("-m64");
			}
			else
			{
				Console.WriteLine("架构只能是i386或者x64！");
				return;
			}
			int iconOption = Array.IndexOf(args, "-i");
			string iconPath;
			if (iconOption != -1)
			{
				if (iconOption == args.Length - 1)
				{
					Console.WriteLine("请给图标(-i)指定参数值或者不使用-i选项！");
					return;
				}
				hasIcon = true;
				iconPath = args[iconOption + 1];
			}
			//开始执行构建
			Directory.CreateDirectory(BUILD_TMP_PATH);
			File.Copy(APP_PATH + "cppsrc", BUILD_TMP_PATH + "jarUtils.h");
			List<string> cppValues = new List<string>();
			cppValues.Add("# ifndef JARVALUES_H");
			cppValues.Add("#define JARVALUES_H");
			cppValues.Add("# include <iostream>");
			cppValues.Add("# include <fstream>");
			cppValues.Add("# include <windows.h>");
			cppValues.Add("using namespace std;");
			string jarFileName = jarFilePath;
			if (jarFilePath.Contains("\\"))
			{
				jarFileName = jarFilePath.Substring(jarFilePath.LastIndexOf("\\") + 1);
			}
			cppValues.Add("string WIN32_JAR_PATH = \"" + jarFileName + "\";");
			cppValues.Add("string JAVA_PATH = \"" + cfg.JavaPath + "\";");
			cppValues.Add("string ERR_MSG = \"" + cfg.ErrorMsg + "\";");
			cppValues.Add("string PRE_ARGS = \"" + cfg.PreArgs + "\";");
			cppValues.Add("string LOGFILE_PATH = \"" + cfg.LogFilePath + "\";");
			if (cfg.WriteLogToFile)
			{
				cppValues.Add("bool WRITE_ERROR_TO_FILE = " + "true;");
			}
			else
			{
				cppValues.Add("bool WRITE_ERROR_TO_FILE = " + "false;");
			}
			if (cfg.Winapp)
			{
				cppValues.Add("bool WINDOW_APP = " + "true;");
			}
			else
			{
				cppValues.Add("bool WINDOW_APP = " + "false;");
			}
			cppValues.Add("#endif");
			File.WriteAllLines(BUILD_TMP_PATH + "jarValues.h", cppValues.ToArray(), Encoding.UTF8);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JarToExeBuilder
{
	class JarUtils
	{
		/// <summary>
		/// 配置信息类
		/// </summary>
		private static Config cfg = Config.GetConfig();

		/// <summary>
		/// 是否是32位应用程序
		/// </summary>
		public static bool IsWin32App = true;

		/// <summary>
		/// 是否是引用模式
		/// </summary>
		public static bool IsInclude = false;

		/// <summary>
		/// 是否有图标
		/// </summary>
		public static bool HasIcon = false;

		/// <summary>
		/// 是否最后进行strip命令去除调试符号
		/// </summary>
		public static bool DoStrip = false;

		/// <summary>
		/// jar文件路径
		/// </summary>
		public static string JarPath = "mainJar.jar";

		/// <summary>
		/// 图标路径
		/// </summary>
		public static string IconPath;

		/// <summary>
		/// g++命令的参数列表
		/// </summary>
		public static List<string> GccArgList = new List<string>();

		/// <summary>
		/// objcopy命令参数列表
		/// </summary>
		public static List<string> ObjcopyArgList = new List<string>();

		/// <summary>
		/// windres命令参数列表
		/// </summary>
		public static List<string> WindresArgList = new List<string>();

		/// <summary>
		/// 显示帮助
		/// </summary>
		public static void printHelp()
		{
			Console.WriteLine("命令行使用方法：\r\nbuildexe -re/-in -j jar文件路径 -o 输出exe路径 -p 架构 [-i ico图标文件路径] [-c 指定配置文件] [-s]\r\n");
			Console.WriteLine("上述命令中中括号括起来部分是可选参数，实际加上这些可选参数执行时不需要写中括号\r\n");
			Console.WriteLine("-re和-in必须要指定一个，且不能共存，它们含义如下：");
			Console.WriteLine("-re --- 引用模式，这时会把源jar文件和构建的exe文件一同放到输出目录，exe相当于启动jar的作用，支持32位和64位程序\r\n");
			Console.WriteLine("-in --- 包含模式，这时会把源jar文件嵌入至构建的exe文件，生成的exe文件可以单独存在，这种模式只支持64位程序\r\n");
			Console.WriteLine("-s --- 是否去除调试信息以减小生成exe大小，默认不去除\r\n");
			Console.WriteLine("架构(-p)参数可选值如下：");
			Console.WriteLine("i386 --- 32位应用程序\r\nx64 --- 64位程序\r\n");
			Console.WriteLine("当没有指定-c时，程序会默认使用buildexe所在目录下的cfg.properties作为配置文件");
		}

		/// <summary>
		/// 双引号包围字符串
		/// </summary>
		/// <param name="origin">原字符串</param>
		/// <returns>被双引号包围的字符串</returns>
		public static string SurroundByQuote(string origin)
		{
			return "\"" + origin + "\"";
		}

		/// <summary>
		/// 执行命令
		/// </summary>
		/// <param name="command">命令</param>
		/// <param name="argList">命令参数列表</param>
		public static void RunCmd(string command, List<string> argList)
		{
			string totalArgs = "";
			foreach (string eachArg in argList)
			{
				string eachArgTrim = eachArg.Trim();
				if (eachArgTrim.Contains(" "))
				{
					totalArgs = totalArgs + " " + SurroundByQuote(eachArgTrim);
				}
				else
				{
					totalArgs = totalArgs + " " + eachArgTrim;
				}
			}
			Process process = new Process();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = totalArgs.Trim();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			try
			{
				process.Start();
				Console.WriteLine(process.StandardOutput.ReadToEnd());
				Console.WriteLine(process.StandardError.ReadToEnd());
				process.WaitForExit();
			}
			catch (Exception)
			{
				Console.WriteLine("找不到命令：" + command);
			}
			finally
			{
				process.Close();
			}
		}

		/// <summary>
		/// 构建头文件
		/// </summary>
		public static void buildHeader()
		{
			List<string> cppValues = new List<string>();
			cppValues.Add("# ifndef JARVALUES_H");
			cppValues.Add("#define JARVALUES_H");
			cppValues.Add("# include <iostream>");
			cppValues.Add("# include <fstream>");
			cppValues.Add("# include <windows.h>");
			cppValues.Add("using namespace std;");
			string jarFileName = JarPath;
			if (JarPath.Contains("\\"))
			{
				jarFileName = JarPath.Substring(JarPath.LastIndexOf("\\") + 1);
			}
			cppValues.Add("string WIN32_JAR_PATH = " + SurroundByQuote(jarFileName) + ";");
			cppValues.Add("string JAVA_PATH = " + SurroundByQuote(cfg.JavaPath) + ";");
			cppValues.Add("string ERR_MSG = " + SurroundByQuote(cfg.ErrorMsg) + ";");
			cppValues.Add("string PRE_ARGS = " + SurroundByQuote(cfg.PreArgs) + ";");
			cppValues.Add("string LOGFILE_PATH = " + SurroundByQuote(cfg.LogFilePath) + ";");
			if (cfg.WriteLogToFile)
			{
				cppValues.Add("bool WRITE_ERROR_TO_FILE = true;");
			}
			else
			{
				cppValues.Add("bool WRITE_ERROR_TO_FILE = false;");
			}
			if (cfg.Winapp)
			{
				cppValues.Add("bool WINDOW_APP = true;");
			}
			else
			{
				cppValues.Add("bool WINDOW_APP = false;");
			}
			cppValues.Add("#endif");
			File.WriteAllLines(Program.BUILD_TMP_PATH + "jarValues.h", cppValues.ToArray(), Encoding.UTF8);
		}

		/// <summary>
		/// 构建图标文件
		/// </summary>
		public static void buildIcon()
		{
			File.Copy(IconPath, Program.BUILD_TMP_PATH + "icon.ico", true);
			File.WriteAllText(Program.BUILD_TMP_PATH + "icon.rc", "1 ICON icon.ico");
			WindresArgList.Add("-i");
			WindresArgList.Add(Program.BUILD_TMP_PATH + "icon.rc");
			WindresArgList.Add("-o");
			WindresArgList.Add(Program.BUILD_TMP_PATH + "icon.o");
			WindresArgList.Add("-F");
			if (IsWin32App)
			{
				WindresArgList.Add("pe-i386");
			}
			else
			{
				WindresArgList.Add("pe-x86-64");
			}
			string originCurrentPath = Environment.CurrentDirectory;
			Environment.CurrentDirectory = Program.BUILD_TMP_PATH;
			RunCmd("windres", WindresArgList);
			WindresArgList.Clear();
			Environment.CurrentDirectory = originCurrentPath;
		}

		/// <summary>
		/// 构建jar文件的链接
		/// </summary>
		public static void buildJarLinkFile()
		{
			File.Copy(JarPath, Program.BUILD_TMP_PATH + "mainJar.jar", true);
			ObjcopyArgList.Add("-I");
			ObjcopyArgList.Add("binary");
			ObjcopyArgList.Add("-O");
			ObjcopyArgList.Add("pe-x86-64");
			ObjcopyArgList.Add("-B");
			ObjcopyArgList.Add("i386:x86-64");
			ObjcopyArgList.Add("mainJar.jar");
			ObjcopyArgList.Add("mainJar.o");
			string originCurrentPath = Environment.CurrentDirectory;
			Environment.CurrentDirectory = Program.BUILD_TMP_PATH;
			RunCmd("objcopy", ObjcopyArgList);
			ObjcopyArgList.Clear();
			Environment.CurrentDirectory = originCurrentPath;
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;

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
		public static readonly string BUILD_TMP_PATH = APP_PATH + "tmp\\";

		/// <summary>
		/// 源代码所在目录
		/// </summary>
		public static readonly string SRC_PATH = APP_PATH + "cppsrc\\";

		static void Main(string[] args)
		{
			//开始读入命令行参数并进行解析
			int cfgOption = Array.IndexOf(args, "-c");
			if (cfgOption != -1 && cfgOption != args.Length - 1)
			{
				string cfgFile = args[cfgOption + 1];
				if (File.Exists(cfgFile))
				{
					Config.cfgPath = cfgFile;
				}
			}
			int jarFileOption = Array.IndexOf(args, "-j");
			int outputFileOption = Array.IndexOf(args, "-o");
			int platformOption = Array.IndexOf(args, "-p");
			int reModeOption = Array.IndexOf(args, "-re");
			int inModeOption = Array.IndexOf(args, "-in");
			if (reModeOption != -1 && inModeOption == -1)
			{
				JarUtils.IsInclude = false;
			}
			else if (reModeOption == -1 && inModeOption != -1)
			{
				JarUtils.IsInclude = true;
			}
			else
			{
				Console.WriteLine("-in和-re必须有且只能指定一个！");
				JarUtils.printHelp();
				return;
			}
			if (jarFileOption == -1 || jarFileOption == args.Length - 1 || outputFileOption == -1 || outputFileOption == args.Length - 1 || platformOption == -1 || platformOption == args.Length - 1)
			{
				Console.WriteLine("参数错误！必须指定参数-j,-o,-p以及它们的值！\r\n");
				JarUtils.printHelp();
				return;
			}
			JarUtils.GccArgList.Add("-finput-charset=UTF-8");
			JarUtils.GccArgList.Add("-fexec-charset=GBK");
			JarUtils.JarPath = args[jarFileOption + 1];
			string outputPath = args[outputFileOption + 1];
			JarUtils.GccArgList.Add("-o");
			JarUtils.GccArgList.Add(outputPath);
			string platformValue = args[platformOption + 1];
			if (platformValue.Equals("i386"))
			{
				if (JarUtils.IsInclude)
				{
					Console.WriteLine("构建32位应用程序时不可以使用包含模式！");
					JarUtils.printHelp();
					return;
				}
				JarUtils.IsWin32App = true;
				JarUtils.GccArgList.Add("-m32");
			}
			else if (platformValue.Equals("x64"))
			{
				JarUtils.IsWin32App = false;
				JarUtils.GccArgList.Add("-m64");
			}
			else
			{
				Console.WriteLine("架构只能是i386或者x64！");
				JarUtils.printHelp();
				return;
			}
			int iconOption = Array.IndexOf(args, "-i");
			if (iconOption != -1)
			{
				if (iconOption == args.Length - 1)
				{
					Console.WriteLine("请给图标(-i)指定参数值或者不使用-i选项！");
					JarUtils.printHelp();
					return;
				}
				JarUtils.HasIcon = true;
				JarUtils.IconPath = args[iconOption + 1];
			}
			if (Array.IndexOf(args, "-s") != -1)
			{
				JarUtils.DoStrip = true;
			}
			//开始执行构建
			Directory.CreateDirectory(BUILD_TMP_PATH);
			File.Copy(SRC_PATH + "jarUtils.h", BUILD_TMP_PATH + "jarUtils.h", true);
			Console.WriteLine("正在应用参数...");
			JarUtils.buildHeader();
			if (!JarUtils.IsInclude)
			{
				Console.WriteLine("注意！当前是引用模式！原jar和构建的exe将会一同被放入输出目录，exe不能离开jar单独存在且jar不能改名！");
				File.Copy(SRC_PATH + "j2e-refer.cpp", BUILD_TMP_PATH + "j2e-refer.cpp", true);
				string jarFileName = JarUtils.JarPath;
				if (JarUtils.JarPath.Contains("\\"))
				{
					jarFileName = JarUtils.JarPath.Substring(JarUtils.JarPath.LastIndexOf("\\") + 1);
				}
				string outputDirectory = outputPath;
				if (outputPath.Contains("\\"))
				{
					outputDirectory = outputPath.Substring(0, outputPath.LastIndexOf("\\") + 1);
				}
				JarUtils.GccArgList.Add(BUILD_TMP_PATH + "j2e-refer.cpp");
				if (JarUtils.HasIcon)
				{
					Console.WriteLine("正在构建图标...");
					JarUtils.buildIcon();
					JarUtils.GccArgList.Add(BUILD_TMP_PATH + "icon.o");
				}
				File.Copy(JarUtils.JarPath, outputDirectory + jarFileName, true);
			}
			else
			{
				Console.WriteLine("注意！当前是包含模式！原jar会被包含至构建的exe中，exe可以单独存在！但是包含模式只能生成64位应用程序，32位系统无法运行！");
				File.Copy(SRC_PATH + "j2e-include.cpp", BUILD_TMP_PATH + "j2e-include.cpp", true);
				JarUtils.GccArgList.Add(BUILD_TMP_PATH + "j2e-include.cpp");
				Console.WriteLine("正在链接jar文件...");
				JarUtils.buildJarLinkFile();
				JarUtils.GccArgList.Add(BUILD_TMP_PATH + "mainJar.o");
				if (JarUtils.HasIcon)
				{
					Console.WriteLine("正在构建图标...");
					JarUtils.buildIcon();
					JarUtils.GccArgList.Add(BUILD_TMP_PATH + "icon.o");
				}
			}
			Console.WriteLine("正在构建exe...");
			JarUtils.RunCmd("g++", JarUtils.GccArgList);
			JarUtils.GccArgList.Clear();
			if (JarUtils.DoStrip)
			{
				Console.WriteLine("正在执行strip...");
				JarUtils.RunCmd("strip", new List<string>() { outputPath });
			}
			Console.WriteLine("已构建完成！");
			Console.WriteLine("清理临时文件...");
			Directory.Delete(BUILD_TMP_PATH, true);
		}
	}
}
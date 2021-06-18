using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JarToExeBuilder
{
	class JarUtils
	{
		public static void printHelp()
		{
			Console.WriteLine("命令行使用方法：\r\nbuildexe -re/-in -j jar文件路径 -o 输出exe路径 -p 架构 [-i ico图标文件路径] [-c 指定配置文件]\r\n");
			Console.WriteLine("上述命令中中括号括起来部分是可选参数，实际加上这些可选参数执行时不需要写中括号\r\n");
			Console.WriteLine("-re和-in必须要指定一个，且不能共存，它们含义如下：");
			Console.WriteLine("-re --- 引用模式，这时会把源jar文件和构建的exe文件一同放到输出目录，exe相当于启动jar的作用，支持32位和64位程序\r\n");
			Console.WriteLine("-in --- 包含模式，这时会把源jar文件嵌入至构建的exe文件，生成的exe文件可以单独存在，这种模式只支持64位程序\r\n");
			Console.WriteLine("架构(-p)参数可选值如下：");
			Console.WriteLine("i386 --- 32位应用程序\r\nx64 --- 64位程序\r\n");
			Console.WriteLine("当没有指定-c时，程序会默认使用buildexe所在目录下的cfg.properties作为配置文件");
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
				if (eachArg.Contains(" "))
				{
					totalArgs = totalArgs + " \"" + eachArg + "\"";
				}
				else
				{
					totalArgs = totalArgs + " " + eachArg;
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
				//none
			}
			finally
			{
				process.Close();
			}
		}
	}
}
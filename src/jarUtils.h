#ifndef JARUTILS_H
#define JARUTILS_H
#include "jarValues.h"

/**
 * 运行时隐藏控制台的函数
 **/
void hideWindow()
{
	HWND hwnd = GetForegroundWindow();
	if (hwnd)
	{
		ShowWindow(hwnd, SW_HIDE);
	}
}

/**
 * string转char*
 * s - 原字符串
 **/
char *toChar(string s)
{
	char *cs = (char *)malloc(s.length() + 1);
	strcpy(cs, s.c_str());
	return cs;
}

/**
 * 引号包围字符串
 * s - 原字符串
 **/
string surByQut(string s)
{
	return "\"" + s + "\"";
}

/**
 * 获取当前时间以字符串形式返回
 **/
string getLocalTime()
{
	SYSTEMTIME sysTime;
	GetLocalTime(&sysTime);
	WORD year = sysTime.wYear;
	WORD month = sysTime.wMonth;
	WORD day = sysTime.wDay;
	WORD hour = sysTime.wHour;
	WORD minute = sysTime.wMinute;
	WORD second = sysTime.wSecond;
	WORD millionSecond = sysTime.wMilliseconds;
	return to_string(year) + to_string(month) + to_string(day) + to_string(hour) + to_string(minute) + to_string(second) + to_string(millionSecond);
}

/**
 * 检查jre是否存在
 **/
int checkJre()
{
	int isexists = 1;
	char result[4];
	string checkArg = " -version && echo yes || echo no";
	string checkCmd = JAVA_PATH + checkArg;
	FILE *fp = popen(toChar(checkCmd), "r");
	fgets(result, sizeof(result), fp);
	pclose(fp);
	system("cls");
	string str = result;
	if (str != "yes")
	{
		isexists = 0;
		string vbsMsgCmd = "echo MsgBox \"" + ERR_MSG + "\", 16, \"错误\">msg.vbs && msg.vbs && del / q msg.vbs";
		system(toChar(vbsMsgCmd));
	}
	return isexists;
}

/**
 * 组合程序总运行参数
 * argc 参数个数，包含自己
 * argv 参数数组
 **/
string getArgs(int argc, char *argv[])
{
	string args = " " + PRE_ARGS;
	if (argc != 1)
	{
		for (int i = 1; i < argc; i++)
		{
			args = args + " " + argv[i];
		}
	}
	return args;
}

/**
 * 释放文件至当前目录
 * head - 文件头指针
 * size - 文件大小（字节）
 * fileName - 释放后目标文件名
 **/
void extractFile(char *head, int size, string fileName)
{
	ofstream fp;
	fp.open(fileName, ios::binary | ios::out);
	fp.write(head, size);
	fp.close();
}

/**
 * 运行命令
 * cmd - 命令
 **/
void runCmd(string cmd)
{
	if (WRITE_ERROR_TO_FILE)
	{
		system(toChar(cmd + " 2>>" + surByQut(LOGFILE_PATH)));
	}
	else
	{
		system(toChar(cmd));
	}
}

#endif
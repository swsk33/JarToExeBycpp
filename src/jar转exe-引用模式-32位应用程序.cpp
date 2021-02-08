#include <iostream>
#include <windows.h>
using namespace std;

//可修改，java的运行路径，默认安装了java的电脑直接使用java命令即可，便携式jre需要在此指定
string javaPath = "java";

//运行时隐藏控制台的函数
void hideWindow() {
	HWND hwnd = GetForegroundWindow();
	if (hwnd) {
		ShowWindow(hwnd, SW_HIDE);
	}
}

//string转char*
char *toChar(string s) {
	char *cs = (char *)malloc(s.length() + 1);
	strcpy(cs, s.c_str());
	return cs;
}

//引号包围字符串
string surByQut(string s) {
	return "\"" + s + "\"";
}

//获取当前时间以字符串形式返回
string getLocalTime() {
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

//检查jre是否存在
int checkJre(string title, string msg) {
	int isexists = 1;
	char result[4];
	string checkArg = " -version && echo yes || echo no";
	string checkCmd = javaPath + checkArg;
	FILE *fp = popen(toChar(checkCmd), "r");
	fgets(result, sizeof(result), fp);
	pclose(fp);
	system("cls");
	string str = result;
	if (str != "yes") {
		isexists = 0;
		MessageBox(NULL, toChar(msg), toChar(title), MB_OK + 16);
	}
	return isexists;
}

//组合程序运行参数
string getArgs(int argc, char *argv[]) {
	string args;
	if (argc == 1) {
		args = "";
	} else {
		args = " ";
		args = args + argv[1];
		for (int i = 2; i < argc; i++) {
			args = args + " " + argv[i];
		}
	}
	return args;
}

int main(int argc, char *argv[]) {
	hideWindow();	//此处可去除，去除后为控制台应用程序，否则为后台服务或者窗口程序
	if (checkJre("错误", "未能找到可用的java运行环境（jre）！请先安装java8运行环境！")) {	//可改变，改变其中的参数设置没有检测到jre时的提示内容
		string args = getArgs(argc, argv);
		string filePath = "";	//需填入，jar文件相对路径
		string preArgs = ""; //可修改，附加参数。即双击exe时自动加上的参数，先于命令行传给exe的参数。
		int writeErrorToLog = 0; //可修改，是否把程序的标准错误输出重定向到本地文件。0代表否，1代表是。建议控制台应用程序不要开启此项。
		string logFileLocation = getLocalTime() + "_error.log"; //可修改，标准错误输出文件位置，可以用%TEMP%代表临时目录，若上面变量writeErrorToLog为0，则此变量无效。
		string cmd = javaPath + " -jar " + surByQut(filePath) + " " + preArgs + args;
		if (writeErrorToLog) {
			system(toChar(cmd + " 2>>" + logFileLocation));
		} else {
			system(toChar(cmd));
		}
	}
}
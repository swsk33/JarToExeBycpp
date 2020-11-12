#include <iostream>
#include <fstream>
#include <windows.h>
#include <stdlib.h>
#include <time.h>
using namespace std;

//文件名统一为mainJar.jar，再使用objcopy命令链接联合编译
extern char _binary_mainJar_jar_start[];
extern char _binary_mainJar_jar_end[];

void hideWindow() {	//运行时隐藏控制台的函数
	HWND hwnd = FindWindow("ConsoleWindowClass", NULL);
	if (hwnd) {
		ShowWindow(hwnd, SW_HIDE);
	}
}

char *toChar(string s) {	//string转char*
	char *cs = (char *)malloc(s.length() + 1);
	strcpy(cs, s.c_str());
	return cs;
}

string surByQut(string s) {	//引号包围字符串
	return "\"" + s + "\"";
}

string getLocalTime() {	//获取当前时间以字符串形式返回
	time_t now;
	struct tm *tm_now;
	time(&now);
	tm_now = localtime(&now);
	int year = tm_now->tm_year;
	int mon = tm_now->tm_mon;
	int day = tm_now->tm_mday;
	int hour = tm_now->tm_hour;
	int min = tm_now->tm_min;
	int sec = tm_now->tm_min;
	string time = to_string(year) + to_string(mon) + to_string(day) + to_string(hour) + to_string(min) + to_string(sec);
	return time;
}

int checkJre(string title, string msg) {	//检查jre是否存在
	int isexists = 1;
	char result[4];
	FILE *fp = popen("java -version && echo yes || echo no", "r");
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

string getArgs(int argc, char *argv[]) {	//组合程序运行参数
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
	if (checkJre("错误", "未能找到可用的java运行环境（jre）！请先安装java8运行环境！")) {	//可改变，改变其中的形参设置没有检测到jre时的提示内容
		int size = _binary_mainJar_jar_end - _binary_mainJar_jar_start;
		string args = getArgs(argc, argv);
		string fileName = getLocalTime() + ".jar";
		string filePath = "%TEMP%\\" + fileName;
		string javaPath = "java";	//可修改，java的运行路径，默认安装了java的电脑直接使用java命令即可，便携式jre需要在此指定
		ofstream fp;
		fp.open(fileName, ios::binary | ios::out);
		fp.write(_binary_mainJar_jar_start, size);
		fp.close();
		string cmd = "move /y " + surByQut(fileName) + " " + surByQut(filePath) + " && cls && " + javaPath + " -jar " + surByQut(filePath) + args;
		system(toChar(cmd));
	}
}
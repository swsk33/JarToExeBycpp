#include <iostream>
#include <fstream>
#include <windows.h>
#include <time.h>
using namespace std;

//�ļ���ͳһΪmainJar.jar����ʹ��objcopy�����������ϱ���
extern char _binary_mainJar_jar_start[];
extern char _binary_mainJar_jar_end[];
//���޸ģ�java������·����Ĭ�ϰ�װ��java�ĵ���ֱ��ʹ��java����ɣ���Яʽjre��Ҫ�ڴ�ָ��
string javaPath = "java";

void hideWindow() {	//����ʱ���ؿ���̨�ĺ���
	HWND hwnd = FindWindow("ConsoleWindowClass", NULL);
	if (hwnd) {
		ShowWindow(hwnd, SW_HIDE);
	}
}

char *toChar(string s) {	//stringתchar*
	char *cs = (char *)malloc(s.length() + 1);
	strcpy(cs, s.c_str());
	return cs;
}

string surByQut(string s) {	//���Ű�Χ�ַ���
	return "\"" + s + "\"";
}

string getLocalTime() {	//��ȡ��ǰʱ�����ַ�����ʽ����
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

int checkJre(string title, string msg) {	//���jre�Ƿ����
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

string getArgs(int argc, char *argv[]) {	//��ϳ������в���
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
	hideWindow();	//�˴���ȥ����ȥ����Ϊ����̨Ӧ�ó��򣬷���Ϊ��̨������ߴ��ڳ���
	if (checkJre("����", "δ���ҵ����õ�java���л�����jre�������Ȱ�װjava8���л�����")) {	//�ɸı䣬�ı����е��β�����û�м�⵽jreʱ����ʾ����
		int size = _binary_mainJar_jar_end - _binary_mainJar_jar_start;
		string args = getArgs(argc, argv);
		string fileName = getLocalTime() + ".jar";
		string filePath = "%TEMP%\\" + fileName;
		ofstream fp;
		fp.open(fileName, ios::binary | ios::out);
		fp.write(_binary_mainJar_jar_start, size);
		fp.close();
		string cmd = "move /y " + surByQut(fileName) + " " + surByQut(filePath) + " && cls && " + javaPath + " -jar " + surByQut(filePath) + args;
		system(toChar(cmd));
	}
}
#include <iostream>
#include <fstream>
#include <windows.h>
using namespace std;

//�ļ���ͳһΪmainJar.jar����ʹ��objcopy�����������ϱ���
extern char _binary_mainJar_jar_start[];
extern char _binary_mainJar_jar_end[];
//���޸ģ�java������·����Ĭ�ϰ�װ��java�ĵ���ֱ��ʹ��java����ɣ���Яʽjre��Ҫ�ڴ�ָ��
string javaPath = "java";

//����ʱ���ؿ���̨�ĺ���
void hideWindow() {
	HWND hwnd = GetForegroundWindow();
	if (hwnd) {
		ShowWindow(hwnd, SW_HIDE);
	}
}

//stringתchar*
char *toChar(string s) {
	char *cs = (char *)malloc(s.length() + 1);
	strcpy(cs, s.c_str());
	return cs;
}

//���Ű�Χ�ַ���
string surByQut(string s) {
	return "\"" + s + "\"";
}

//��ȡ��ǰʱ�����ַ�����ʽ����
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

//���jre�Ƿ����
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

//��ϳ������в���
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
	hideWindow();	//�˴���ȥ����ȥ����Ϊ����̨Ӧ�ó��򣬷���Ϊ��̨������ߴ��ڳ���
	if (checkJre("����", "δ���ҵ����õ�java���л�����jre�������Ȱ�װjava8���л�����")) {	//�ɸı䣬�ı����еĲ�������û�м�⵽jreʱ����ʾ����
		int size = _binary_mainJar_jar_end - _binary_mainJar_jar_start;
		string args = getArgs(argc, argv);
		string fileName = getLocalTime() + ".jar";
		string filePath = "%TEMP%\\" + fileName;
		ofstream fp;
		fp.open(fileName, ios::binary | ios::out);
		fp.write(_binary_mainJar_jar_start, size);
		fp.close();
		string preArgs = ""; //���޸ģ��������в�������˫��exe���Զ����ϵ������в����������������������������exeʱ���ϵĲ�����
		int writeErrorToLog = 0; //���޸ģ��Ƿ�ѳ���ı�׼��������ض��򵽱����ļ���0�����1�����ǡ��������̨Ӧ�ó���Ҫ�������
		string logFileLocation = getLocalTime() + "_error.log"; //���޸ģ���׼��������ļ�λ�ã�������%TEMP%������ʱĿ¼�����������writeErrorToLogΪ0����˱�����Ч��
		string cmd = "move /y " + surByQut(fileName) + " " + surByQut(filePath) + " && cls && " + javaPath + " -jar " + surByQut(filePath) + " " + preArgs + args;
		if (writeErrorToLog) {
			system(toChar(cmd + " 2>>" + logFileLocation));
		} else {
			system(toChar(cmd));
		}
	}
}
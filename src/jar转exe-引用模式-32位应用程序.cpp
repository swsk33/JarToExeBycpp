#include <iostream>
#include <windows.h>
using namespace std;

//���޸ģ�java������·����Ĭ�ϰ�װ��java�ĵ���ֱ��ʹ��java����ɣ���Яʽjre��Ҫ�ڴ�ָ��
string javaPath = "java";

//����ʱ���ؿ���̨�ĺ���
void hideWindow() {
	HWND hwnd = FindWindow("ConsoleWindowClass", NULL);
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
		string args = getArgs(argc, argv);
		string filePath = "";	//�����룬jar�ļ����·��
		string preArgs = ""; //���޸ģ����Ӳ�������˫��exeʱ�Զ����ϵĲ��������������д���exe�Ĳ�����
		string cmd = javaPath + " -jar " + surByQut(filePath) + " " + preArgs + args;
		system(toChar(cmd));
	}
}
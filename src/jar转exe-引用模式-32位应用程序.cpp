#include <iostream>
#include <windows.h>
using namespace std;

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

int checkJre(string title, string msg) {	//���jre�Ƿ����
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
		string args = getArgs(argc, argv);
		string filePath = "";	//�����룬jar�ļ����·��
		string javaPath = "java";	//���޸ģ�java������·����Ĭ�ϰ�װ��java�ĵ���ֱ��ʹ��java����ɣ���Яʽjre��Ҫ�ڴ�ָ��
		string cmd = javaPath + " -jar " + surByQut(filePath) + args;
		system(toChar(cmd));
	}
}
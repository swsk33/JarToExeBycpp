#include "jarUtils.h"

//文件名统一为mainJar.jar，再使用objcopy命令链接联合编译
extern char _binary_mainJar_jar_start[];
extern char _binary_mainJar_jar_end[];

int main(int argc, char *argv[])
{
	if (WINDOW_APP)
	{
		hideWindow();
	}
	if (checkJre())
	{
		int size = _binary_mainJar_jar_end - _binary_mainJar_jar_start;
		string args = getArgs(argc, argv);
		string fileName = getLocalTime() + ".jar";
		string filePath = "%TEMP%\\" + fileName;
		extractFile(_binary_mainJar_jar_start, size, fileName);
		string cmd = "move /y " + surByQut(fileName) + " " + surByQut(filePath) + " && cls && " + JAVA_PATH + " -jar " + surByQut(filePath) + args + " && del /q " + surByQut(filePath);
		runCmd(cmd);
	}
}
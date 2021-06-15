#include "jarUtils.h"

int main(int argc, char *argv[])
{
	if (WINDOW_APP)
	{
		hideWindow();
	}
	if (checkJre())
	{
		string args = getArgs(argc, argv);
		string cmd = JAVA_PATH + " -jar " + surByQut(WIN32_JAR_PATH) + args;
		runCmd(cmd);
	}
}
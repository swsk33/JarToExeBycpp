#ifndef JARVALUES_H
#define JARVALUES_H
#include <iostream>
#include <fstream>
#include <windows.h>
using namespace std;

/**
 * 下列几个全局变量可以改变
 **/
//32位程序需要根据实际情况修改/填入：jar文件相对路径，64位程序忽略此项
string WIN32_JAR_PATH = "mainJar.jar";
//java的运行路径，默认安装了java的电脑直接使用java命令即可，便携式jre需要在此指定
string JAVA_PATH = "java";
//没有检测到jre时的提示内容
string ERR_MSG = "未能找到可用的java运行环境（jre）！请先安装java8运行环境！";
//可修改，附带运行参数。即双击exe后自动加上的命令行参数。这个参数会先于命令行运行exe时加上的参数。
string PRE_ARGS = "";
//可修改，是否把程序的标准错误输出重定向到本地文件，建议控制台应用程序不要开启此项。
bool WRITE_ERROR_TO_FILE = false;
//可修改，标准错误输出文件位置，可以用%TEMP%代表临时目录，若上面变量WRITE_ERROR_TO_FILE为false，则此变量无效。
string LOGFILE_PATH = "error.log";
//是否是窗口应用程序
bool WINDOW_APP = true;

#endif
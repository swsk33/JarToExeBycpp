# C++实现jar转exe程序模板

### 介绍
一个简单的C++源码，64位应用程序通过把这个源码稍加修改并与链接后的jar文件（通过objcopy）联合编译即可实现jar套上exe的壳；32位程序则是直接通过exe启动jar而无需使用命令行运行jar。<br>
TDM-GCC下载地址：<br>
[官网下载](https://jmeubank.github.io/tdm-gcc/)<br>
[备用下载](https://swsk33.lanzoui.com/b0bqvyq4d)<br>

### 使用说明
先需要安装TDM-GCC并配置好环境变量（一般安装完成就配置好了）。<br>
然后下载或者git clone项目源码进行操作。<br>
#### 64位应用程序方案-包含模式
*64位应用程序方案是把jar包含到exe里面，然后编译出来的exe点击可直接运行jar，生成了exe后就可以删除原来的jar文件了。*<br>
然后先把要打包的jar文件改名为mainJar.jar，然后在jar文件所在目录下使用objcopy命令链接：<br>
```
objcopy -I binary -O pe-x86-64 -B i386:x86-64 mainJar.jar 输出文件名.o
```
输出文件名自己随意。<br>
这样就得到了链接后的jar文件，扩展名为o。<br>
若要给编译出来的exe加上图标，还需新建一个rc文件，用文本编辑器打开并加入：<br>
```
1 ICON ico文件路径
```
然后使用windres命令编译图标资源文件：<br>
```
windres rc文件路径 输出图标文件.o
```
然后把cppsrc文件夹里面的"j2e-include.cpp"、"jarUtils.h"和"jarValues.h"这三个源文件复制出来放在链接后的jar文件和链接后的图标文件同目录下，使用g++命令一起编译即可：<br>
```
g++ -m64 -finput-charset=UTF-8 -fexec-charset=GBK j2e-include.cpp 链接后的jar文件 [链接的图标文件] -o 输出编译文件.exe
```
编译时必须保证cpp文件和两个头文件以及链接的jar等链接文件在一个文件夹下，然后就得到了打包好了的jar文件了！<br>
上述过程中链接图标资源文件不是必须，如果没有链接图标文件，使用g++命令时也不需要编译链接的图标文件，只需要源文件和链接后的jar文件即可。<br>
#### 32位应用程序方案-引用模式
*32位应用程序方案是通过编译出一个exe来间接启动jar文件。32位无法实现把jar嵌入到exe文件里面去，所以只能外部引用启动，所以生成了exe后不能删除jar文件。*<br>
先准备好你的jar文件，然后把项目cppsrc文件夹里面的"j2e-refer.cpp"、"jarUtils.h"和"jarValues.h"这三个源文件复制出来，并修改"jarValues.h"文件中**12**行的```WIN32_JAR_PATH```变量，修改为你的jar文件的相对路径（相对于cpp生成最终的exe文件的路径）<br>
例如我cpp编译后的exe文件将生成（输出）到C:\Test目录下，我的main.jar文件也将放到这个目录下，那么相对路径就是"main.jar"。这时修改**12**行内容最终如下：<br>
```
string WIN32_JAR_PATH = "main.jar";
```
然后进行编译：<br>
```
g++ -m32 -finput-charset=UTF-8 -fexec-charset=GBK j2e-refer.cpp -o 输出编译文件.exe
```
要给exe加图标的步骤和上文64位方案相同。<br>
编译时必须保证cpp文件和两个头文件在一个目录下，且需要保证生成的exe文件和jar文件的相对路径与编译前自己修改的"jarValues.h"文件中的WIN32_JAR_PATH变量的值一致！<br>
#### 64位应用程序方案-引用模式
按照上述方法生成的64位程序是包含了jar的，如果在64位平台上不想包含jar进exe只是让exe调用命令启动jar，只需要用64位模式编译"j2e-refer.cpp"即可：
```
g++ -m64 -finput-charset=UTF-8 -fexec-charset=GBK j2e-refer.cpp -o 输出编译文件.exe
```
注意这样的话也需要和上述“32位应用程序方案-引用模式”一样，对"jarValues.h"里面```WIN32_JAR_PATH```进行修改设定为你的jar文件相对路径。<br>
可以说此“64位应用程序方案-引用模式”中除了编译命令有个参数不一样之外，其余步骤和“32位应用程序方案-引用模式”是一模一样的。<br>

**所有变量存放在"jarValues.h"中且是可以修改的，可以根据其中注释修改。**<br>

若编译出来的exe文件太大，可参考方案：[地址](https://blog.csdn.net/yanhanhui1/article/details/109631544)<br>
g++给exe加图标方案参考：[地址](https://blog.csdn.net/yanhanhui1/article/details/110238429)<br>
>最后更新：2021.6.15
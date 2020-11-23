# C++实现jar转exe程序模板

#### 介绍
一个简单的C++源码，64位应用程序通过把这个源码稍加修改并与链接后的jar文件（通过objcopy）联合编译即可实现jar套上exe的壳；32位程序则是直接通过exe启动jar而无需使用命令行运行jar。<br>
TDM-GCC下载地址：<br>
[官网下载](https://jmeubank.github.io/tdm-gcc/)<br>
[备用下载](https://swsk33.lanzoux.com/b0bqvyq4d)<br>

#### 使用说明
先需要安装TDM-GCC并配置好环境变量（一般安装完成就配置好了）。<br>
**64位应用程序方案**<br>
*64位应用程序方案是把jar包含到exe里面，然后编译出来的exe点击可直接运行jar，生成了exe后就可以删除原来的jar文件了。*<br>
然后先把要打包的jar文件改名为mainJar.jar，然后使用objcopy命令链接：<br>
```objcopy -I binary -O elf64-x86-64 -B i386:x86-64 mainJar.jar 输出文件名.o```<br>
输出文件名自己随意。<br>
这样就得到了链接后的jar文件，扩展名为o。<br>
若要给编译出来的exe加上图标，还需新建一个rc文件，用文本编辑器打开并加入：<br>
```1 ICON ico文件路径```<br>
然后使用windres命令编译图标资源文件：<br>
```windres rc文件路径 输出图标文件.o```<br>
然后把src文件夹里面的"jar转exe-包含模式-64位应用程序.cpp"这个源文件放在链接后的jar文件和链接后的图标文件同目录下，使用g++命令一起编译即可：<br>
```g++ -m64 cpp源文件 链接后的jar文件 链接的图标文件 -o 输出编译文件.exe```<br>
然后就得到了打包好了的jar文件了！<br>
上述过程中链接图标资源文件不是必须，如果没有链接图标文件，使用g++命令时也不需要编译链接的图标文件，只需要源文件和链接后的jar文件即可。<br>
**32位应用程序方案**<br>
*32位应用程序方案是通过编译出一个exe来间接启动jar文件。32位无法实现把jar嵌入到exe文件里面去，所以只能外部引用启动，所以生成了exe后不能删除jar文件。*<br>
先准备好你的jar文件，然后把项目src文件夹里面的"jar转exe-引用模式-32位应用程序.cpp"复制出来，并修改60行的filePath变量，修改为你的jar文件的相对路径（相对于cpp生成最终的exe文件的路径）<br>
例如我cpp编译后的exe文件将生成（输出）到C:\Test目录下，我的main.jar文件也将放到这个目录下，那么相对路径就是"main.jar"。这时修改60行内容最终如下：<br>
```string filePath = "main.jar";```<br>
然后进行编译：<br>
```g++ -m32 cpp源文件 -o 输出编译文件.exe```<br>
要给exe加图标的步骤和上文64位方案相同。<br>
请务必保证生成的exe文件和jar文件的相对路径与编译前自己修改的cpp文件中的filePath变量的值一致！<br>
若编译出来的exe文件太大，可参考方案：[地址](https://blog.csdn.net/yanhanhui1/article/details/109631544)<br>
g++给exe加图标方案参考：[地址](https://mzlogin.blog.csdn.net/article/details/6647460)<br>
>最后更新：2020.11.23
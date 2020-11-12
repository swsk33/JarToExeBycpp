# C++套壳jar文件程序模板

#### 介绍
一个简单的C++源码，把这个源码稍加修改并与链接后的jar文件（通过objcopy）联合编译即可实现jar套上exe的壳。<br>
TDM-GCC下载地址：<br>
[官网下载](https://jmeubank.github.io/tdm-gcc/)<br>
[备用下载](https://swsk33.lanzoux.com/b0bqvyq4d)<br>

#### 使用说明
先需要安装TDM-GCC并配置好环境变量（一般安装完成就配置好了）。<br>
**64位系统方案**<br>
然后先把要打包的jar文件改名为mainJar.jar，然后使用objcopy命令链接：<br>
```objcopy -I binary -O elf64-x86-64 -B i386:x86-64 mainJar.jar 输出文件名.o```<br>
输出文件名自己随意。<br>
这样就得到了链接后的jar文件，扩展名为o。<br>
若要给编译出来的exe加上图标，还需新建一个rc文件，用文本编辑器打开并加入：<br>
```1 ICON ico文件路径```<br>
然后使用windres命令编译图标资源文件：<br>
```windres rc文件路径 输出图标文件.o```<br>
然后把src文件夹里面的"java打包cpp方案-64位系统.cpp"这个源文件放在链接后的jar文件和链接后的图标文件同目录下，使用g++命令一起编译即可：<br>
```g++ cpp源文件 链接的jar文件 链接的图标文件 -o 输出编译文件.exe```<br>
然后就得到了打包好了的jar文件了！<br>
上述过程中链接图标资源文件不是必须，如果没有链接图标文件，使用g++命令时也不需要编译链接的图标文件，只需要源文件和链接后的jar文件即可。<br>
若编译出来的exe文件太大，可参考方案：[地址](https://blog.csdn.net/yanhanhui1/article/details/109631544)<br>
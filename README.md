# C++实现jar转exe程序模板

### 介绍
一个简单的C++源码，64位应用程序通过把这个源码稍加修改并与链接后的jar文件（通过objcopy）联合编译即可实现jar套上exe的壳；32位程序则是直接通过exe启动jar而无需使用命令行运行jar。<br>
TDM-GCC下载地址：<br>
[官网下载](https://jmeubank.github.io/tdm-gcc/)<br>
[备用下载](https://swsk33.lanzoui.com/b0bqvyq4d)<br>

### 使用说明
#### 1，环境配置
先需要安装TDM-GCC并配置好环境变量（一般安装完成就配置好了）。<br>
然后[下载](https://gitee.com/swsk33/jarToExeBycpp/releases)并解压右侧发行版/Release里面j2ec-xxx.7z文件（xxx代表版本号，下载最新版即可）进行操作。<br>
#### 2，按需修改配置文件
在解压的文件夹中有一个"cfg.properties"文件，可以使用文本编辑器打开，这是全局配置文件，没有特殊需要可以不修改，不过大多数时候可能需要修改，里面配置值代表如下：
- javapath=java的运行路径，默认安装了java的电脑直接填java即可，便携式jre需要在此指定
- errormsg=没有检测到jre时的提示内容
- preArgs=附带运行参数。即双击exe后自动加上的命令行参数,这个参数会先于命令行运行exe时加上的参数
- writeLogToFile=是否把程序的标准错误输出重定向到本地文件，建议控制台应用程序不要开启此项，开启填true否则为false
- logFilePath=标准错误输出文件位置，若上面变量writeLogToFile为false，则此变量无效
- winapp=是否是窗口应用程序，窗口程序填true，控制台程序填false

配置文件中以#开头的一行是注释，构建exe时不会读取注释内容，默认情况下配置文件是全部被注释的状态。可根据自己需要去掉配置值的注释并填入配置值。
#### 3，打开命令行/cmd调用buildexe进行jar到exe的构建
使用命令行/cmd的cd命令进入到解压的文件夹，输入命令调用buildexe.exe文件，命令形式如下：
```
buildexe -re/-in -j jar文件路径 -o 输出exe路径 -p 架构 [-i ico图标文件路径] [-c 指定配置文件] [-s]
```
上述命令中中括号括起来部分是可选参数，实际加上这些可选参数执行时不需要写中括号。<br>
-re和-in必须要指定一个，且不能共存，它们含义下：
- -re --- 引用模式，这时会把源jar文件和构建的exe文件一同放到输出目录，exe相当于启动jar的作用，支持32位和64位程序
- -in --- 包含模式，这时会把源jar文件嵌入至构建的exe文件，生成的exe文件可以单独存在，这种模式只支持64位程序

-s --- 是否去除调试信息以减小生成exe大小，默认不去除

架构(-p)参数可选值如下：                                    
i386 --- 32位应用程序                                  
x64 --- 64位程序

当没有指定-c时，程序会默认使用buildexe所在目录下的cfg.properties作为配置文件<br>
上述参数顺序可以任意写。<br>
#### 5，实例
**将E:\\中转\\a.jar打包为main.exe的64位应用程序放到用户下载文件夹：**<br>
```
buildexe -in -j "E:\中转\a.jar" -p x64 -o "C:\Users\%username%\Downloads\main.exe"
```
**构建32位应用程序引用模式的main.exe用于启动E:\\中转\\a.jar，放到用户下载文件夹，并指定exe图标为C:\\icon\\ex.ico：**<br>
```
buildexe -re -p i386 -j "E:\中转\a.jar" -o "C:\Users\%username%\Downloads\main.exe" -i "C:\icon\ex.ico"
```
#### 6，其它
若编译出来的exe文件太大，可参考方案：[地址](https://blog.csdn.net/yanhanhui1/article/details/109631544)<br>
**该程序原理可以参考：**<br>
C语言或者C++中隐藏控制台窗口：[地址](https://blog.csdn.net/yanhanhui1/article/details/109271169)<br>
在C语言/C++中把资源编译进exe可执行文件，并运行时释放资源：[地址](https://blog.csdn.net/yanhanhui1/article/details/109235349)<br>
g++给exe加图标：[地址](https://blog.csdn.net/yanhanhui1/article/details/110238429)<br>
>最后更新：2021.6.18
@echo off
setlocal enabledelayedexpansion
cd %~dp0
set dmode=%1
set version=%2
set dest=C:\Users\%username%\Downloads
set copyDir=%dest%\j2ec
set fileName=j2ec
if defined dmode (
	if %dmode%==t (
		mkdir %copyDir%
		mkdir %copyDir%\cppsrc
		copy /y "cppsrc\*" "%copyDir%\cppsrc"
		copy /y "builder\buildexe.exe" "%copyDir%\buildexe.exe"
		copy /y "builder\cfg.properties" "%copyDir%\cfg.properties"
		echo 已复制所需文件至%copyDir%文件夹中！
		goto end
	) else (
		if %dmode%==r (
			if defined version (
				set fileName=%fileName%-%version%
			)
			7z a -t7z -mx9 "%dest%\!fileName!.7z" "cppsrc" ".\builder\buildexe.exe" ".\builder\cfg.properties"
			echo 部署发行版完成至%dest%\!fileName!.7z！
			goto end
		) else (
			echo 错误！
			goto help
		)
	)
) else (
	echo 请指定模式！
	goto help
)
:help
echo 用法：
echo deploy 模式 [版本号]
echo 模式：
echo t --- 测试模式，只会复制所需文件至用户下载文件夹以供用户测试，无需指定版本号
echo r --- 发行模式，压缩所需文件至用户下载文件夹，可以指定版本号以作为压缩文件后缀
:end
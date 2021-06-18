@echo off
cd %~dp0
set mode=%1
set suffix=%2
set dest=C:\Users\%username%\Downloads
set copyDir=%dest%\j2ec
set fileName=j2ec
if defined mode (
	if %mode%==t (
		mkdir %copyDir%
		mkdir %copyDir%\cppsrc
		copy /y "cppsrc\*" "%copyDir%\cppsrc"
		copy /y "builder\buildexe.exe" "%copyDir%\buildexe.exe"
		copy /y "builder\cfg.properties" "%copyDir%\cfg.properties"
		echo 已复制所需文件至%copyDir%文件夹中！
		goto end
	) else (
		if %mode%==r (
			if defined suffix (
				set fileName=%fileName%-%suffix%
			)
			7z a -t7z -mx9 "%dest%\%fileName%.7z" "cppsrc" ".\builder\buildexe.exe" ".\builder\cfg.properties"
			echo 部署发行版完成至%dest%\%fileName%.7z！
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
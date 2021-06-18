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
		echo �Ѹ��������ļ���%copyDir%�ļ����У�
		goto end
	) else (
		if %dmode%==r (
			if defined version (
				set fileName=%fileName%-%version%
			)
			7z a -t7z -mx9 "%dest%\!fileName!.7z" "cppsrc" ".\builder\buildexe.exe" ".\builder\cfg.properties"
			echo �����а������%dest%\!fileName!.7z��
			goto end
		) else (
			echo ����
			goto help
		)
	)
) else (
	echo ��ָ��ģʽ��
	goto help
)
:help
echo �÷���
echo deploy ģʽ [�汾��]
echo ģʽ��
echo t --- ����ģʽ��ֻ�Ḵ�������ļ����û������ļ����Թ��û����ԣ�����ָ���汾��
echo r --- ����ģʽ��ѹ�������ļ����û������ļ��У�����ָ���汾������Ϊѹ���ļ���׺
:end
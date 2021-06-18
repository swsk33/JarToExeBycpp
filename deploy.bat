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
		echo �Ѹ��������ļ���%copyDir%�ļ����У�
		goto end
	) else (
		if %mode%==r (
			if defined suffix (
				set fileName=%fileName%-%suffix%
			)
			7z a -t7z -mx9 "%dest%\%fileName%.7z" "cppsrc" ".\builder\buildexe.exe" ".\builder\cfg.properties"
			echo �����а������%dest%\%fileName%.7z��
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
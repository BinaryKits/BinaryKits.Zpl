@ECHO OFF
setlocal enabledelayedexpansion
color 00
title NSSM�����Զ���װ��������(��Ҫϵͳ����ԱȨ�޲�������)
::������װ����
@echo ����������������NSSM.exe�����д�����,�뽫��Ҫ��װ��exe����NSSM.exe������������������ͬһ���ļ��С�


PUSHD %~DP0 & cd /d "%~dp0"
%1 %2
mshta vbscript:createobject("shell.application").shellexecute("%~s0","goto :runas","","runas",1)(window.close)&goto :eof
:runas


echo.
echo.
echo ######################��ѡ��Ҫִ�еĲ���######################
echo ----------------------1.��װ������Ŀ���Զ�����----------------------
echo ----------------------2.ж�ظ÷���----------------------------------
echo.
echo.
echo ��ѡ��Ҫִ�еĲ���
set /p sel=
@set clientname=BinaryKitsZPL
@set clientexe=D:\Program Files (x86)\BinaryKits\BinaryKits.Zpl.Viewer.WebApi.exe
::@set clientDescript=ZPLԤ������������ͽӿڿ�ͨ��http://localhost:8019��ַ����-- Description=%clientDescript%

@echo on
if %sel% equ 1 (
	::for /f "tokens=* delims=" %%a in ('sc query %clientname% | findstr /i "δ��װ"') do set output=%%a
	::if %output% equ ''(
	nssm install %clientname% %clientexe% 
      if %ERRORLEVEL% equ 1 (
			 ECHO ִ��nssm install�������
			 pause>nul
			exit 0
		)
	
   
   nssm start %clientname%
)

 if %sel% equ 2 (
   nssm stop %clientname%
   echo %ERRORLEVEL%
   if %ERRORLEVEL% equ 1 (
      ECHO ִ��nssm stop�������
	  pause>nul
      exit 0
   )
   nssm remove %clientname% confirm
   if %ERRORLEVEL% NEQ 1 (
      ECHO ִ��nssm remove�������
	  pause>nul
      exit 0
   )
)
echo ִ�����,������˳�...
pause>nul
exit 0

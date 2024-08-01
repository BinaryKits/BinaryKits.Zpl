@ECHO OFF
setlocal enabledelayedexpansion
color 00
title NSSM服务自动安装启动工具(需要系统管理员权限才能启动)
::批处理安装工具
@echo 本批处理工具依赖于NSSM.exe命令行处理工具,请将需要安装的exe程序、NSSM.exe、及本批处理程序放入同一个文件夹。


PUSHD %~DP0 & cd /d "%~dp0"
%1 %2
mshta vbscript:createobject("shell.application").shellexecute("%~s0","goto :runas","","runas",1)(window.close)&goto :eof
:runas


echo.
echo.
echo ######################请选择要执行的操作######################
echo ----------------------1.安装服务项目并自动启动----------------------
echo ----------------------2.卸载该服务----------------------------------
echo.
echo.
echo 请选择要执行的操作
set /p sel=
@set clientname=BinaryKitsZPL
@set clientexe=D:\Program Files (x86)\BinaryKits\BinaryKits.Zpl.Viewer.WebApi.exe
::@set clientDescript=ZPL预览服务，浏览器和接口可通过http://localhost:8019地址访问-- Description=%clientDescript%

@echo on
if %sel% equ 1 (
	::for /f "tokens=* delims=" %%a in ('sc query %clientname% | findstr /i "未安装"') do set output=%%a
	::if %output% equ ''(
	nssm install %clientname% %clientexe% 
      if %ERRORLEVEL% equ 1 (
			 ECHO 执行nssm install命令出错
			 pause>nul
			exit 0
		)
	
   
   nssm start %clientname%
)

 if %sel% equ 2 (
   nssm stop %clientname%
   echo %ERRORLEVEL%
   if %ERRORLEVEL% equ 1 (
      ECHO 执行nssm stop命令出错
	  pause>nul
      exit 0
   )
   nssm remove %clientname% confirm
   if %ERRORLEVEL% NEQ 1 (
      ECHO 执行nssm remove命令出错
	  pause>nul
      exit 0
   )
)
echo 执行完毕,任意键退出...
pause>nul
exit 0

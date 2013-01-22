@echo off

pushd "%~dp0"

rem Define locations for known sys calls
set _nunit="c:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0\nunit-console.exe"
rem set _nunit="C:\Program Files (x86)\Nunit 2.6\bin\nunit-console.exe"

copy .\lib\nunit.framework.dll .\bin\nunit.framework.dll

%_nunit% /framework=4.0 .\bin\tests.dll

rem Clear any variables set in this scope
set _nunit=
popd

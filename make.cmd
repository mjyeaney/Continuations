@echo off

pushd "%~dp0"

rem Define locations for sys calls
set _csc="C:\windows\microsoft.net\framework\v4.0.30319\csc.exe"

echo Building source...

rem This is for the Windows .NET install - still need a Mono-compatible option
%_csc% /nologo /t:library /out:.\Bin\continuations.dll /d:TRACE /debug .\Src\*.cs

echo Building tests...

%_csc% /nologo /t:library /out:.\Bin\tests.dll /r:.\bin\continuations.dll,.\lib\nunit.framework.dll /d:TRACE /debug .\Tests\*.cs

rem Clear all variables used in this scope
set _csc=
popd

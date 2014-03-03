echo on
if "%~1" == "" goto usage
if "%~2" == "" goto usage
if "%~3" == "" goto usage
if "%~1" == "Debug" goto skip
call "%VS90COMNTOOLS%vsvars32.bat"
cd "%~2"
setlocal

set DLLS=FSharp.Core.dll ja\*.dll
set TM=%time: =0%
set DEPLOY=deploy_%date:~-10,4%%date:~-5,2%%date:~-2,2%_%TM:~0,2%%TM:~3,2%%TM:~6,2%
if exist %DEPLOY% (
rd /s /q %DEPLOY%
del %DEPLOY%
)
mkdir %DEPLOY%
ILMerge /wildcards /v4 /ndebug /out:%DEPLOY%\%~3 %~3 %DLLS%
copy *.txt %DEPLOY%
copy Presentation.dll %DEPLOY%

endlocal
goto end
:usage
echo Usage: package {Debug^|Release} Dir File
echo;
:skip
echo Skipped packaging.
:end

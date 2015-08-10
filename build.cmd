@ECHO OFF

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
cd src

IF EXIST packages goto run
..\.nuget\NuGet.exe restore


:run

@echo Compiling...
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /property:Configuration=Debug /property:Platform="Any CPU" /m /v:m /nologo /p:VisualStudioVersion=14.0
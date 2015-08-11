@ECHO OFF

CALL BUILD

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto pack
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul


:pack

if not exist _GeneratedNuGetPackages MD _GeneratedNuGetPackages
del _GeneratedNuGetPackages\*.*
CD src\Brick.FluentNHibernate.Conventions
..\..\.nuget\nuget pack -OutputDirectory ..\..\_GeneratedNuGetPackages -Symbols
CD ..\..\_GeneratedNuGetPackages

rem for /f %%X IN ('dir /b *.nupkg') do ..\.nuget\NuGet.exe push "%%~fX"
CD ..
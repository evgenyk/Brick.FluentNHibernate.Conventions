CD _GeneratedNuGetPackages

for /f %%X IN ('dir /b *.nupkg') do ..\.nuget\NuGet.exe push "%%~fX" -Source https://www.nuget.org/api/v2/package
CD ..
CD _GeneratedNuGetPackages

for /f %%X IN ('dir /b *.nupkg') do ..\.nuget\NuGet.exe push "%%~fX"
CD ..
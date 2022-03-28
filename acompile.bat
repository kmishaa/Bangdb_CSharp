
cls

@Echo off

@SET DOTNET_CLI_TELEMETRY_OPTOUT=1

@SET DOT="C:\Program Files\dotnet\dotnet.exe"


%DOT% build -r:win10-x64 -o:dlls source\BangDBcs.csproj 

rem %DOT% build -r:win7-x86 -o:BERKDB\x86 proj\berkDB1.csproj 


@rd /s /q source\obj


@Forfiles /M "*.pdb" /S /C "cmd /c del @file" 

"C:\DB\CORE\dlls\BangDBcs.exe"

@pause>nul
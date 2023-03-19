@echo off
set shortcutName=Live Stream Genie.lnk
set appPath="%cd%\bin\Release\net6.0-windows\publish\win-x64\Live Stream Genie.exe"
set startupFolder="%appdata%\Microsoft\Windows\Start Menu\Programs\Startup"

:: Create a shortcut to the application
powershell -Command "$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%shortcutName%'); $Shortcut.TargetPath = '%appPath%'; $Shortcut.WorkingDirectory = '%cd%'; $Shortcut.Save()"

:: Copy the shortcut to the Startup folder
xcopy /y "%cd%\%shortcutName%" %startupFolder%

echo "Live Stream Genie has been added to the Startup folder."
pause
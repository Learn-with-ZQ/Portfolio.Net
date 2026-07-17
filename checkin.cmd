@echo off
setlocal
cd /d "%~dp0"

rem Use the commit message passed as an argument, otherwise ask for one
set "MSG=%~1"
if "%MSG%"=="" set /p "MSG=Commit message: "
if "%MSG%"=="" set "MSG=Update %date% %time%"

git add .
git commit -m "%MSG%"
if errorlevel 1 (
    echo.
    echo Nothing to commit or commit failed - not pushing.
    pause
    exit /b 1
)

git push
if errorlevel 1 (
    echo.
    echo Push failed - check your connection or GitHub sign-in.
    pause
    exit /b 1
)

echo.
echo Done - changes are on GitHub.
pause

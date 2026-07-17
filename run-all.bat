@echo off
title Personal Professional Platform

echo Starting API...
start "Portfolio API" cmd /k "dotnet run --project "D:\.Net Core Learning\Portfolio\src\Portfolio.Api""

echo Starting Public Website...
start "Portfolio Web" cmd /k "cd /d "D:\.Net Core Learning\Portfolio\portfolio-web" && npm start"

echo Starting Admin Portal...
start "Portfolio Admin" cmd /k "cd /d "D:\.Net Core Learning\Portfolio\portfolio-admin" && npx ng serve --port 4300"

echo.
echo All services are starting...
echo.
echo API:        http://localhost:5014/swagger
echo Public:     http://localhost:4200
echo Admin:      http://localhost:4300
pause
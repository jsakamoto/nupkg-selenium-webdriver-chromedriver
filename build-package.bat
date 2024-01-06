@echo off
pushd %~dp0

dotnet pack .\buildTools\Selenium.WebDriver.ChromeDriver.csproj

echo Complete!
echo.
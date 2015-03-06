@echo off
pushd %~dp0

echo Downloading %fname%...
powershell -noprof -exec unrestricted -c ".\download-driver.ps1"
echo.
:SKIP_DOWNLOAD

echo Packaging...
.\NuGet.exe pack .\Selenium.WebDriver.ChromeDriver.nuspec

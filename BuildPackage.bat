@echo off
pushd %~dp0

echo Downloading %fname%...
powershell -noprof -exec unrestricted -c ".\buildTools\download-driver.ps1"
echo.
:SKIP_DOWNLOAD

echo Packaging...
.\buildTools\NuGet.exe pack .\src\Selenium.WebDriver.ChromeDriver.nuspec -Out .\dist

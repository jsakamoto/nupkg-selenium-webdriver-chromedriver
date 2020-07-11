@echo off
pushd %~dp0

echo Downloading %fname%...
powershell -noprof -exec unrestricted -c ".\buildTools\download-driver.ps1"
echo.
:SKIP_DOWNLOAD

echo Build .targets file from .src...
powershell -noprof -exec unrestricted -c ".\buildTools\preprocess-targets-file.ps1"
echo.

echo Packaging...
.\buildTools\NuGet.exe pack .\src\Selenium.WebDriver.ChromeDriver.nuspec -OutputDirectory .\dist
echo.

echo Complete!
echo.
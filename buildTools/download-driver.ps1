# constants
$version = "2.28"
$driverName = "chromedriver.exe"
$zipName = "chromedriver_win32.$version.zip"
$downloadUrl = "https://chromedriver.storage.googleapis.com/$version/chromedriver_win32.zip"

# move current folder to where contains this .ps1 script file.
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
pushd $scriptDir
cd ..

$currentPath = Convert-Path "."
$downloadsDir = Join-Path $currentPath "downloads"
$zipPath = Join-Path $downloadsDir $zipName
$driverPath = Join-Path $downloadsDir $driverName

# download driver .zip file if not exists.
if (-not (Test-Path $zipPath)){
    (New-Object Net.WebClient).Downloadfile($downloadurl, $zipPath)
    if (Test-Path $driverPath) {
        del $driverPath 
    }
}

# Decompress .zip file to extract driver .exe file.
if (-not (Test-Path $driverPath)) {
    $shell = New-Object -com Shell.Application
    $zipFile = $shell.NameSpace($zipPath)

    $zipFile.Items() | `
    where {(Split-Path $_.Path -Leaf) -eq $driverName} | `
    foreach {
        $extractTo = $shell.NameSpace($downloadsDir)
        $extractTo.copyhere($_.Path)
    }
    sleep(2)
}

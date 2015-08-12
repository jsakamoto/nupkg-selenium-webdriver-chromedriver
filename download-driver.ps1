# constants
$version = "2.17"
$driverName = "chromedriver.exe"
$zipName = "chromedriver_win32.$version.zip"
$downloadUrl = "https://chromedriver.storage.googleapis.com/$version/chromedriver_win32.zip"

# move current folder to where contains this .ps1 script file.
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
pushd $scriptDir

$currentPath = Convert-Path "."
$zipPath = Join-Path $currentPath $zipName

# download driver .zip file if not exists.
if (-not (Test-Path ".\$zipName")){
    (New-Object Net.WebClient).Downloadfile($downloadurl, $zipPath)
    if (Test-Path ".\$driverName") { del ".\$driverName" }
}

# Decompress .zip file to extract driver .exe file.
if (-not (Test-Path ".\$driverName")) {
    $shell = New-Object -com Shell.Application
    $zipFile = $shell.NameSpace($zipPath)

    $zipFile.Items() | `
    where {(Split-Path $_.Path -Leaf) -eq $driverName} | `
    foreach {
        $cuurentDir = $shell.NameSpace((Convert-Path "."))
        $cuurentDir.copyhere($_.Path)
    }
    sleep(2)
}

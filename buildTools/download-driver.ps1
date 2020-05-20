# constants
$version = "83.0.4103.39"
$downloadUrlBase = "https://chromedriver.storage.googleapis.com"

$drivers = @(
    [ordered]@{
        platform = "win32";
        fileName = "chromedriver.exe";
    }
    ,
    [ordered]@{
        platform = "mac64";
        fileName = "chromedriver";
    }
    ,
    [ordered]@{
        platform = "linux64";
        fileName = "chromedriver";
    }
)

# move current folder to where contains this .ps1 script file.
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
pushd $scriptDir
cd ..
$currentPath = Convert-Path "."
$downloadsBaseDir = Join-Path $currentPath "downloads"

$drivers | % {
    $driver = $_
    $platform = $driver.platform

    $downloadDir = Join-Path $downloadsBaseDir $driver.platform
    if (-not (Test-Path $downloadDir -PathType Container)) {
        mkdir $downloadDir > $null
    }

    $driverName = $driver.fileName
    $driverPath = Join-Path $downloadDir $driverName

    # download driver .zip file if not exists.
    $zipName = "chromedriver_$platform.$version.zip"
    $zipPath = Join-Path $downloadDir $zipName
    if (-not (Test-Path $zipPath)) {
        $downloadUrl = "$downloadUrlBase/$version/chromedriver_$platform.zip"
        (New-Object Net.WebClient).Downloadfile($downloadurl, $zipPath)
    }

    # Decompress .zip file to extract driver file.
    if (Test-Path $driverPath) { Remove-Item  $driverPath }
    Expand-Archive $zipPath $downloadDir
}

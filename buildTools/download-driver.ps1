# constants
$version = "106.0.5249.61"
$downloadUrlBase = "https://chromedriver.storage.googleapis.com"

$drivers = @(
    [ordered]@{
        platform = "win32";
        folder   = "win32";
        fileName = "chromedriver.exe";
    }
    ,
    [ordered]@{
        platform = "mac64";
        folder   = "mac64";
        fileName = "chromedriver";
    }
    ,
    [ordered]@{
        platform = "mac_arm64";
        folder   = "mac64arm";
        fileName = "chromedriver";
    }
    ,
    [ordered]@{
        platform = "linux64";
        folder   = "linux64";
        fileName = "chromedriver";
    }
)

# move current folder to where contains this .ps1 script file.
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
Push-Location $scriptDir
Set-Location ..
$currentPath = Convert-Path "."
$downloadsBaseDir = Join-Path $currentPath "downloads"

$drivers | ForEach-Object {
    $driver = $_
    $platform = $driver.platform
    $folder = $driver.folder

    $downloadDir = Join-Path $downloadsBaseDir $folder
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

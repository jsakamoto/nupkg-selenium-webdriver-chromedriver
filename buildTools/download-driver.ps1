# constants
$version = "121.0.6167.16"
$downloadUrlBase = "https://edgedl.me.gvt1.com/edgedl/chrome/chrome-for-testing"

$drivers = @(
    [ordered]@{
        platform = "win32";
        folder   = "win32";
        fileName = "chromedriver.exe";
    }
    ,
    [ordered]@{
        platform = "mac-x64";
        folder   = "mac64";
        fileName = "chromedriver";
    }
    ,
    [ordered]@{
        platform = "mac-arm64";
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

    # download driver .zip file if not exists.
    $zipName = "chromedriver_$platform.$version.zip"
    $zipPath = Join-Path $downloadDir $zipName
    if (-not (Test-Path $zipPath)) {
        $downloadUrl = "$downloadUrlBase/$version/$platform/chromedriver-$platform.zip"
        (New-Object Net.WebClient).Downloadfile($downloadurl, $zipPath)
    }
    
    # Decompress .zip file to extract driver file.
    $extractedDir = Join-Path $downloadDir ("chromedriver-" + $platform)
    if (Test-Path $extractedDir) { Remove-Item  $extractedDir -Recurse -Force }
    Expand-Archive $zipPath $downloadDir -Force
    Move-Item (Join-Path $extractedDir "*") $downloadDir -Force
    Remove-Item  $extractedDir -Recurse -Force
}

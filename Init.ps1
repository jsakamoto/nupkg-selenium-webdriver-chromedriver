param($installPath, $toolsPath, $package, $project)

$driverFile = "chromedriver.exe"
$downloadUrl = "https://chromedriver.storage.googleapis.com/2.8/chromedriver_win32.zip"
$contentPath = Join-Path $installPath "content"
$driverPath = Join-Path $contentPath $driverFile

if ((Test-Path $driverPath) -eq $false) {

    # Download selenium driver zip file.
    $tmpFilePath = [IO.Path]::GetTempFileName() + ".zip"
    (new-object Net.WebClient).DownloadFile($downloadUrl, $tmpFilePath)

    $shell = new-object -com Shell.Application
    $zipFile = $shell.NameSpace($tmpFilePath)

    $zipFile.Items() | `
    where {(Split-Path $_.Path -Leaf) -eq $driverFile} | `
    foreach {
        $contentFolder = $shell.NameSpace($contentPath)
        $contentFolder.copyhere($_.Path)
    }

    rm $tmpFilePath
}

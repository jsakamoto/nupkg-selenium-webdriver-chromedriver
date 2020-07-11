$propFile = Get-ChildItem .\src\DefineProperties*.targets
$propContents = Get-Content $propFile

$targetsSrcFile = Get-ChildItem .\src\*.targets.src
$targetsSrcContents = Get-Content $targetsSrcFile

$targetsOutFilePath = $targetsSrcFile.FullName -replace "\.src$", ""

if (Test-Path $targetsOutFilePath) { Remove-Item $targetsOutFilePath }

foreach ($line in $targetsSrcContents) {
    if ($line.Trim() -eq "<!-- insert DefineProperties.targets -->") {
        foreach ($propLine in $propContents[1..($propContents.Count - 2)]) {
            Write-Output "  $propLine".TrimEnd() | Out-File $targetsOutFilePath -Append -Encoding utf8
        }
    }
    else {
        Write-Output $line | Out-File $targetsOutFilePath -Append -Encoding utf8
    }
}

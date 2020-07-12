$targetsSrcFiles = Get-ChildItem .\src\*.targets.src

foreach ($targetsSrcFile in $targetsSrcFiles) {
    
    $targetsOutFilePath = $targetsSrcFile.FullName -replace "\.src$", ""
    if (Test-Path $targetsOutFilePath) { Remove-Item $targetsOutFilePath }
    
    $targetsSrcContents = Get-Content $targetsSrcFile
    foreach ($line in $targetsSrcContents) {
        if ($line.Trim() -match "<!-- insert ([^ ]+.targets) -->") {
            $propFile = Join-Path $targetsSrcFile.DirectoryName $Matches[1]
            $propContents = Get-Content $propFile
            foreach ($propLine in $propContents[1..($propContents.Count - 2)]) {
                Write-Output "  $propLine".TrimEnd() | Out-File $targetsOutFilePath -Append -Encoding utf8
            }
        }
        else {
            Write-Output $line | Out-File $targetsOutFilePath -Append -Encoding utf8
        }
    }
}
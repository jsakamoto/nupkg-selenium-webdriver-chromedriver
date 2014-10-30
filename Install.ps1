param($installPath, $toolsPath, $package, $project)

$driverFile = "chromedriver.exe"
$label = "ChromeDriver"

$markerFile = "README-$label.txt"
$contentPath = Join-Path $installPath "content"
$driverPath = Join-Path $contentPath $driverFile
$nupkgName = Split-Path $installPath -Leaf

$projectUri = [uri]$project.FullName;
$drivertUri = [uri]$driverPath;
$driverRelativePath = $projectUri.MakeRelative($drivertUri) -replace "/","\"

# delete marker file.
$project.ProjectItems.Item($markerFile).Delete()

# Add content item as a link that is WebDriver binary saved at higher folder.
# (This is normal case.)
if ($driverRelativePath -like "..\*") {
	$project.ProjectItems.AddFromFile($driverPath)
	$project.ProjectItems.Item($driverFile).Properties.Item("CopyToOutputDirectory").Value = 2
}
$project.Save()

# Treat the project file as a MSBuild script xml instead of DTEnv object model.
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
$projectXml = ([Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | select -First 1).Xml

# Add content item as a link that is WebDriver binary saved at lower folder.
# (This is the case that .csproj file and packages folder are in a same folder.)
if ($driverRelativePath -notlike "..\*") {
	$itemGrp = $projectXml.CreateItemGroupElement()
	$projectXml.AppendChild($itemGrp)
	$item = $itemGrp.AddItem("Content", $driverRelativePath)
	$item.AddMetadata("Link", $driverFile)
	$item.AddMetadata("CopyToOutputDirectory", "PreserveNewest")
}

# Add properties for download task.
$propGrp = $projectXml.CreatePropertyGroupElement()
$projectXml.AppendChild($propGrp)
$propGrp.Label = "Download$label`BuildTask"
$propGrp.AddProperty("CoreBuildDependsOn", "Download$label`;`$(CoreBuildDependsOn)")
$propGrp.AddProperty("$label`InstallPath", "`$(SolutionDir)packages\$nupkgName\")
$propGrp.AddProperty("$label`ToolsPath", "`$($label`InstallPath)tools\")
$propGrp.AddProperty("$label`InitScriptPath", "`$($label`ToolsPath)Init.ps1")
$propGrp.AddProperty("$label`Path", "`$($label`InstallPath)content\$driverFile")
$propPsExe = $propGrp.AddProperty("PowerShellExe","%WINDIR%\System32\WindowsPowerShell\v1.0\powershell.exe")
$propPsExe.Condition = "'`$(PowerShellExe)'==''"

# Add download task body.
$target = $projectXml.CreateTargetElement("Download$label")
$projectXml.AppendChild($target)
$target.Label = ("Download$label`BuildTask")
$task = $target.AddTask("Exec")
$task.Condition = "!Exists('`$($label`Path)')"
$task.SetParameter("Command", "`$(PowerShellExe) -NonInteractive -executionpolicy Unrestricted -command `"& { &'`$($label`InitScriptPath)' '`$($label`InstallPath)' '`$($label`ToolsPath)'} `"")

$project.Save()

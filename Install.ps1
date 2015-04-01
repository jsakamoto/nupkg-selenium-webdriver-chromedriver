param($installPath, $toolsPath, $package, $project)

$driverFile = "chromedriver.exe"
$label = "ChromeDriver"

$markerFile = "README-$label.txt"
$driverPath = Join-Path (Join-Path $installPath "driver") $driverFile

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

$project.Save()

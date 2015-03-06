param($installPath, $toolsPath, $package, $project)

$driverFile = "chromedriver.exe"
$label = "ChromeDriver"

$driverPath = Join-Path (Join-Path $installPath "driver") $driverFile

$projectUri = [uri]$project.FullName;
$drivertUri = [uri]$driverPath;
$driverRelativePath = $projectUri.MakeRelative($drivertUri) -replace "/","\"

# Delete content item that is WebDriver binary saved at higher folder.
# (This is normal case.)
if ($driverRelativePath -like "..\*") {
	$project.ProjectItems.Item($driverFile).Delete()
	$project.Save()
}

# Treat the project file as a MSBuild script xml instead of DTEnv object model.
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
$projectXml = ([Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | select -First 1).Xml

# Delete content item that is WebDriver binary saved at lower folder.
# (This is the case that .csproj file and packages folder are in a same folder.)
if ($driverRelativePath -notlike "..\*") {
	$projectXml.Children | `
	where { $_ -is [Microsoft.Build.Construction.ProjectItemGroupElement] } | `
	foreach { $_.Children } | `
	where { ($_.Children | where {$_.Name -eq "Link" -and $_.Value -eq $driverFile}) -ne $null } | `
	foreach {
		$itemGrp = $_.Parent
		$itemGrp.RemoveChild($_)
		if ($itemGrp.Children.Count -eq 0) { $projectXml.RemoveChild($itemGrp) }
	}
}

$project.Save()

param($installPath, $toolsPath, $package, $project)

$driverFile = "chromedriver.exe"
$label = "ChromeDriver"

$project.ProjectItems.Item($driverFile).Delete()
$project.Save()

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
$constructor = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | select -First 1
$constructor.Xml.Children | `
where {$_.Label -eq "Download$label`BuildTask"} | `
foreach { $constructor.Xml.RemoveChild($_) }

$project.Save()

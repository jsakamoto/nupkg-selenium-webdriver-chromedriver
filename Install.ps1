param($installPath, $toolsPath, $package, $project)

$driverFile = "chromedriver.exe"
$label = "ChromeDriver"

$markerFile = "README-$label.txt"
$contentPath = Join-Path $installPath "content"
$driverPath = Join-Path $contentPath $driverFile
$nupkgName = Split-Path $installPath -Leaf

$project.ProjectItems.AddFromFile($driverPath)
$project.ProjectItems.Item($driverFile).Properties.Item("CopyToOutputDirectory").Value = 2
$project.ProjectItems.Item($markerFile).Delete()
$project.Save()

Add-Type -AssemblyName "Microsoft.Build"
$projectXml = ([Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | select -First 1).Xml

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

$target = $projectXml.CreateTargetElement("Download$label")
$projectXml.AppendChild($target)
$target.Label = ("Download$label`BuildTask")
$task = $target.AddTask("Exec")
$task.Condition = "!Exists('`$($label`Path)')"
$task.SetParameter("Command", "`$(PowerShellExe) -NonInteractive -executionpolicy Unrestricted -command `"& { &'`$($label`InitScriptPath)' '`$($label`InstallPath)' '`$($label`ToolsPath)'} `"")

$project.Save()

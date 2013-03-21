echo "Start..."

$projName = "Cooking"
$quickStartProjName = "QuickStartProject"

function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

$source = Get-ScriptDirectory
$dest = "$source\Output\$projName"

#copy template
Remove-Item "$source\Output" -Recurse -Force
robocopy $source $dest /s /xd _ReSharper.QuickStartProject .git Output /xf *bin*.exe *obj*.dll

#rename files content
$exludeRenameFiles = @('*.exe', '*.dll', '*.png', '*.jpg', '*.suo')
$files = Get-ChildItem $dest -Rec -Exclude $exludeRenameFiles | where {!$_.PsIsContainer -and !$_.Fullname.Contains("package") -and !$_.Fullname.Contains("\bin\") -and !$_.Fullname.Contains("\obj\")}
foreach ($file in $files)
{
	echo "Renaming $file"
	(Get-Content $file.PSPath) | Foreach-Object {$_ -replace $quickStartProjName , $projName} | Set-Content $file.PSPath
}

#rename directories & files
$fileSystemObjects = Get-ChildItem $dest -Rec | Sort Fullname -Descending
foreach ($fsObj in $fileSystemObjects)
{
	echo "Renaming $fsObj"
	$oldName = $fsObj.Name
	if ($oldName.Contains($quickStartProjName))
	{
		$newName = $oldName.Replace($quickStartProjName, $projName)
		Rename-Item $fsObj.Fullname $newName
	}
}

echo "Done."
Write-Host "Compiling solution in Release mode..."
msbuild FlexibleConfiguration.sln /t:Rebuild /p:Configuration=Release /verbosity:quiet

Write-Host "Compiling nuget packages..."
$currentDir = (Resolve-Path .\).Path
Get-ChildItem $currentDir -Filter *.nuspec | `
Foreach-Object {
	nuget pack $_.FullName
}

Write-Host "Pushing packages to local folder..."
Get-ChildItem $currentDir -Filter *.nupkg | `
Foreach-Object {
	nuget add $_.FullName -Source $env:local_nuget_path
}

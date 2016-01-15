Write-Host "Compiling solution in Release mode..."
msbuild FlexibleConfiguration.sln /t:Rebuild /p:Configuration=Release /verbosity:quiet

Write-Host "Compiling nuget packages..."
$currentDir = (Resolve-Path .\).Path
Get-ChildItem $currentDir -Filter *.nuspec | `
Foreach-Object {
	nuget pack $_.FullName
}

Write-Host "Pushing packages to MyGet..."
Get-ChildItem $currentDir -Filter *.nupkg | `
Foreach-Object {
	nuget push $_.FullName -ApiKey $env:flexconfig_myget_api_key -Source "https://www.myget.org/F/flexibleconfiguration/api/v2/package"
}
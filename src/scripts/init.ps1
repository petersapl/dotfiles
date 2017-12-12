param([string]$apiKey)

$downloadLocation = [System.IO.Path]::GetTempPath() + "dotfiles"
#create folder in TEMP path if not exists
mkdir -Force $downloadLocation | Out-Null

#download DotfilesWrapper
Write-Output "Downloading DotfilesWrapper..."
Invoke-Expression ".\download-artifact.ps1 -apiKey $apiKey"

#extract DotfilesWrapper
Write-Output "Extracting DotfilesWrapper..."
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory("$downloadLocation\dotfiles.zip", $downloadLocation)

Remove-Item "$downloadLocation\dotfiles.zip"

#copy YAML files
Get-ChildItem -Path "..\templates" -Recurse | Copy-Item -Destination $downloadLocation

#install Chocolatey
Write-Output "Installing Chocolatey..."
Invoke-Expression ((New-Object System.Net.WebClient).DownloadString("https://chocolatey.org/install.ps1"))

#start process
Write-Output "Invoking DotfilesWrapper..."
Invoke-Expression "$downloadLocation\DotfilesWrapper.exe $downloadLocation\choco.yaml $downloadLocation\commands.yaml"

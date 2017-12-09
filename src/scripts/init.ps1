#create folder if not exists
mkdir -Force C:\dotfiles | Out-Null

#download DotfilesWrapper
Write-Output "Downloading DotfilesWrapper..."
Invoke-Expression ".\download-artifact.ps1"

#extract DotfilesWrapper
Write-Output "Extracting DotfilesWrapper..."
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory("C:\dotfiles\dotfiles.zip", "C:\dotfiles")

Remove-Item "C:\dotfiles\dotfiles.zip"

#copy YAML files
Get-ChildItem -Path "..\templates" -Recurse | Copy-Item -Destination "C:\dotfiles"

#install Chocolatey
Write-Output "Installing Chocolatey..."
Invoke-Expression ((New-Object System.Net.WebClient).DownloadString(('https://chocolatey.org/install.ps1'))

#start process
Write-Output "Invoking DotfilesWrapper..."
Invoke-Expression "C:\dotfiles\DotfilesWrapper.exe C:\dotfiles\choco.yaml C:\dotfiles\commands.yaml"

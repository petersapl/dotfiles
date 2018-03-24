param([string]$apiKey)

Set-ExecutionPolicy Bypass -Scope Process -Force

$downloadLocation = [System.IO.Path]::GetTempPath() + "dotfiles"
#create folder in TEMP path if not exists
mkdir -Force $downloadLocation | Out-Null

#download YAML files
Write-Output "Downloading YAML files..."
Invoke-RestMethod "https://raw.githubusercontent.com/DiXN/dotfiles/master/src/templates/choco.yaml" | Out-File "choco.yaml"
Invoke-RestMethod "https://raw.githubusercontent.com/DiXN/dotfiles/master/src/templates/commands.yaml" | Out-File "commands.yaml"

#download scripts
Write-Output "Downloading script files..."
Invoke-RestMethod "https://raw.githubusercontent.com/DiXN/dotfiles/master/src/scripts/default-apps.ps1" | Out-File "default-apps.ps1"
Invoke-RestMethod "https://raw.githubusercontent.com/DiXN/dotfiles/master/src/scripts/download-artifact.ps1" | Out-File "download-artifact.ps1"

#download DotfilesWrapper
Write-Output "Downloading DotfilesWrapper..."
Invoke-Expression ".\download-artifact.ps1 -apiKey $apiKey"

#extract DotfilesWrapper
Write-Output "Extracting DotfilesWrapper..."
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory("$downloadLocation\dotfiles.zip", $downloadLocation)

Remove-Item "$downloadLocation\dotfiles.zip"

#disable UAC
Write-Output 'Disabling UAC ...'
Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows\CurrentVersion\Policies\System" -Name "ConsentPromptBehaviorAdmin" -Value "0"
Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows\CurrentVersion\Policies\System" -Name "ConsentPromptBehaviorUser" -Value "0"
Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows\CurrentVersion\Policies\System" -Name "EnableLUA" -Value "1"
Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows\CurrentVersion\Policies\System" -Name "PromptOnSecureDesktop" -Value "0"

#install Chocolatey
Write-Output "Installing Chocolatey..."
Invoke-Expression ((New-Object System.Net.WebClient).DownloadString("https://chocolatey.org/install.ps1"))

#start process
Write-Output "Invoking DotfilesWrapper..."
Invoke-Expression "$downloadLocation\DotfilesWrapper.exe $downloadLocation\choco.yaml $downloadLocation\commands.yaml"

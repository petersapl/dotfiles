function Create-File {
  $file = $args[0]
  if($file -eq $null) {
    throw "No filename supplied"
  }

  if(Test-Path $file) {
    (Get-ChildItem $file).LastWriteTime = Get-Date
  }
  else {
    Write-Output $null > $file
  }
}

Set-Alias touch Create-File
New-Alias which get-command

# Chocolatey profile
$ChocolateyProfile = "$env:ChocolateyInstall\helpers\chocolateyProfile.psm1"
if (Test-Path($ChocolateyProfile)) {
  Import-Module "$ChocolateyProfile"
}

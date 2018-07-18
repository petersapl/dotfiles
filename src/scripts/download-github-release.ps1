[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
$latestRelease = Invoke-WebRequest -Uri https://github.com/DiXN/dotfiles/releases/latest -UseBasicParsing -Headers @{"Accept"="application/json"}

$json = $latestRelease.Content | ConvertFrom-Json
$latestVersion = $json.tag_name
$downloadLocation = [System.IO.Path]::GetTempPath() + "dotfiles"

Invoke-WebRequest -Uri "https://github.com/DiXN/dotfiles/releases//download/$($latestVersion)/dotfiles.zip" -UseBasicParsing -OutFile "$downloadLocation\dotfiles.zip"

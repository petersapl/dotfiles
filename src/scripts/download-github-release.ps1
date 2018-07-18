[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
$latestRelease = Invoke-WebRequest -Uri https://github.com/DiXN/dotfiles/releases/latest -UseBasicParsing -Headers @{"Accept"="application/json"}

$json = $latestRelease.Content | ConvertFrom-Json
$latestVersion = $json.tag_name

Invoke-WebRequest -Uri "https://github.com/DiXN/dotfiles/releases//download/$($latestVersion)/dotfiles.zip" -UseBasicParsing -OutFile "dotfiles.zip"

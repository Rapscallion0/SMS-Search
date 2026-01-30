param(
    [string]$MarkerPath
)

$startup = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Startup)
$lnk = Join-Path $startup 'SMSSearchLauncher.lnk'

if (Test-Path $lnk) {
    New-Item -ItemType File -Path $MarkerPath -Force | Out-Null
    Remove-Item $lnk -Force -ErrorAction SilentlyContinue
}

Stop-Process -Name 'SMSSearch','SMSSearchLauncher','SMS Search Launcher' -Force -ErrorAction SilentlyContinue

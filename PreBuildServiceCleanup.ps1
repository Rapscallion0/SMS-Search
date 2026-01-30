param(
    [string]$MarkerPath
)

$startup = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Startup)
$lnk = Join-Path $startup 'SMSSearchLauncher.lnk'

if (Test-Path $lnk) {
    New-Item -ItemType File -Path $MarkerPath -Force | Out-Null
    Remove-Item $lnk -Force -ErrorAction SilentlyContinue
}

$processNames = 'SMSSearch','SMSSearchLauncher','SMS Search Launcher'
$processes = Get-Process -Name $processNames -ErrorAction SilentlyContinue

if ($processes) {
    $processes | Stop-Process -Force -ErrorAction SilentlyContinue
    # Wait for the processes to actually exit to avoid file locks during build
    $processes | Wait-Process -Timeout 10 -ErrorAction SilentlyContinue
}

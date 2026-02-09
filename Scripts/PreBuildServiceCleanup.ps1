param(
    [string]$MarkerPath
)

$startup = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Startup)
$lnk = Join-Path $startup 'SMSSearchLauncher.lnk'

# 1. Unregister Service (Remove Shortcut)
if (Test-Path $lnk) {
    Write-Host "Found startup shortcut. Creating marker and removing..."
    New-Item -ItemType File -Path $MarkerPath -Force | Out-Null
    Remove-Item $lnk -Force -ErrorAction SilentlyContinue
}

# 2. Kill Processes Loop
$processNames = 'SMSSearch','SMSSearchLauncher','SMS Search Launcher','SMS Search'
$timeoutSeconds = 15
$sw = [System.Diagnostics.Stopwatch]::StartNew()

Write-Host "Checking for running processes: $($processNames -join ', ')..."

while ($sw.Elapsed.TotalSeconds -lt $timeoutSeconds) {
    $processes = Get-Process -Name $processNames -ErrorAction SilentlyContinue

    if (-not $processes) {
        Write-Host "All processes stopped."
        break
    }

    foreach ($p in $processes) {
        Write-Host "Stopping process: $($p.ProcessName) ($($p.Id))..."
        Stop-Process -InputObject $p -Force -ErrorAction SilentlyContinue
    }

    Start-Sleep -Milliseconds 500
}

$sw.Stop()

# 3. Final Verification
$remaining = Get-Process -Name $processNames -ErrorAction SilentlyContinue
if ($remaining) {
    Write-Error "Failed to stop the following processes after $timeoutSeconds seconds: $($remaining.ProcessName -join ', '). Build may fail due to file locks."
    # We exit with 0 to allow the build to proceed as requested, forcing the MSBuild file lock error if it happens.
    exit 0
} else {
    Write-Host "Cleanup complete."
}

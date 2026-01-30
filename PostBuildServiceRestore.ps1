param(
    [string]$MarkerPath,
    [string]$TargetPath,
    [string]$TargetDir
)

if (Test-Path $MarkerPath) {
    $startup = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Startup)
    $lnk = Join-Path $startup 'SMSSearchLauncher.lnk'

    $WshShell = New-Object -comObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut($lnk)
    $Shortcut.TargetPath = $TargetPath
    $Shortcut.Arguments = '--listener'
    $Shortcut.WorkingDirectory = $TargetDir
    $Shortcut.Save()

    Start-Process -FilePath $TargetPath -ArgumentList '--listener'

    Remove-Item $MarkerPath -Force -ErrorAction SilentlyContinue
}

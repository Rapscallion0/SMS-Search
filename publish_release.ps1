param(
    [string]$AssemblyInfoPath,
    [string]$ExePath
)

$ErrorActionPreference = "Stop"

try {
    Write-Host "Starting GitHub Release process..."

    # Check if files exist
    if (-not (Test-Path $AssemblyInfoPath)) {
        Throw "AssemblyInfo.cs not found at $AssemblyInfoPath"
    }
    if (-not (Test-Path $ExePath)) {
        Throw "Executable not found at $ExePath"
    }

    # Extract Version from AssemblyInfo.cs
    # We look for [assembly: AssemblyVersion("...")]
    $content = Get-Content $AssemblyInfoPath -Raw
    if ($content -match '\[assembly: AssemblyVersion\("([^"]+)"\)\]') {
        $version = $matches[1]
        Write-Host "Detected version: $version"
    } else {
        Throw "Could not find AssemblyVersion in $AssemblyInfoPath"
    }

    # Check if release already exists using gh CLI
    # We allow the error stream to go to null, we just care about the exit code.
    # Note: running an external command like gh inside PowerShell updates $LASTEXITCODE
    gh release view $version 2>&1 | Out-Null

    if ($LASTEXITCODE -eq 0) {
        Write-Warning "Release $version already exists. Skipping creation."
        exit 0
    }

    # Create Release
    Write-Host "Creating release $version and uploading artifact..."
    # --generate-notes automatically creates release notes based on pull requests/commits
    gh release create $version $ExePath --generate-notes

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully created release $version."
    } else {
        Throw "Failed to create release. Please check if 'gh' is authenticated and has permissions."
    }

} catch {
    Write-Error "Error during release process: $($_.Exception.Message)"
    exit 1
}

param(
    [string]$AssemblyInfoPath,
    [string]$ExePath
)

$ErrorActionPreference = "Stop"

# Prompt user for confirmation before proceeding with publish
Add-Type -AssemblyName System.Windows.Forms
$confirm = [System.Windows.Forms.MessageBox]::Show(
    "Do you want to publish this release to GitHub?" + [Environment]::NewLine +
    "This will bump version, push to git, and create a release.",
    "Confirm Publish",
    [System.Windows.Forms.MessageBoxButtons]::YesNo,
    [System.Windows.Forms.MessageBoxIcon]::Question
)

if ($confirm -ne 'Yes') {
    Write-Host "Publish cancelled by user."
    exit 0
}

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

    # Commit and Push AssemblyInfo.cs if changed
    $gitStatus = git status "$AssemblyInfoPath" --porcelain
    if ($gitStatus) {
        Write-Host "Changes detected in AssemblyInfo.cs. Committing and pushing..."

        git add "$AssemblyInfoPath"
        if ($LASTEXITCODE -ne 0) { Throw "Failed to git add $AssemblyInfoPath" }

        git commit -m "Bump version to $version"
        if ($LASTEXITCODE -ne 0) { Throw "Failed to git commit" }

        git push origin HEAD
        if ($LASTEXITCODE -ne 0) { Throw "Failed to git push" }

        Write-Host "Successfully committed and pushed version bump."
    } else {
        Write-Host "No changes detected in AssemblyInfo.cs."
    }

    # Check if gh CLI is available
    if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
        Write-Warning "GitHub CLI ('gh') is not found in the system PATH. Skipping release creation."
        Write-Warning "Current PATH: $env:PATH"
        exit 0
    }

    # Check if release already exists using gh CLI
    # We allow the error stream to go to null, we just care about the exit code.
    # Note: running an external command like gh inside PowerShell updates $LASTEXITCODE
    $releaseExists = $false
    try {
        gh release view $version 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            $releaseExists = $true
        }
    } catch {
        # If gh release view fails, it usually means the release doesn't exist.
        # We proceed to create it.
    }

    if ($releaseExists) {
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

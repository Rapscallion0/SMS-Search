param(
    [string]$AssemblyInfoPath,
    [string]$InputPath
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

    # Check if AssemblyInfo exists
    if (-not (Test-Path $AssemblyInfoPath)) {
        Throw "AssemblyInfo.cs not found at $AssemblyInfoPath"
    }

    # Resolve ExePath
    if (Test-Path $InputPath -PathType Container) {
        $ExePath = Join-Path $InputPath "SMS Search.exe"
        Write-Host "Input is a directory. Looking for executable at: $ExePath"
    } else {
        $ExePath = $InputPath
        Write-Host "Input is a file: $ExePath"
    }

    if (-not (Test-Path $ExePath)) {
        Throw "Executable not found at $ExePath"
    }

    # Extract Version from AssemblyInfo.cs
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
    $releaseExists = $false
    try {
        gh release view $version 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            $releaseExists = $true
        }
    } catch {
        # Ignore
    }

    if ($releaseExists) {
        Write-Warning "Release $version already exists. Skipping creation."
        exit 0
    }

    # Create Zip file
    $zipPath = Join-Path (Split-Path $ExePath) "SMS_Search.zip"
    Write-Host "Creating zip archive at $zipPath..."

    if (Test-Path $zipPath) {
        Remove-Item $zipPath -Force
    }

    # Compress only the executable
    Compress-Archive -Path $ExePath -DestinationPath $zipPath -Force

    # Create Release
    Write-Host "Creating release $version and uploading artifact..."
    # Upload the zip file
    gh release create $version $zipPath --generate-notes

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully created release $version."
    } else {
        Throw "Failed to create release. Please check if 'gh' is authenticated and has permissions."
    }

} catch {
    Write-Error "Error during release process: $($_.Exception.Message)"
    exit 1
}

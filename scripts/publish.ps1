param(
  [string]$Version
)

$ErrorActionPreference = 'Stop'
$RepoRoot = Resolve-Path "$PSScriptRoot\.."
Set-Location $RepoRoot

# Read version from csproj if not provided
if (-not $Version) {
  [xml]$cs = Get-Content "$RepoRoot\TheBackyard.csproj"
  $Version = ($cs.Project.PropertyGroup | Where-Object { $_.Version } | Select-Object -First 1).Version
  if (-not $Version) { throw "Could not determine version. Pass -Version 1.0.x" }
}

# Ensure we're on main
$branch = (git rev-parse --abbrev-ref HEAD).Trim()
if ($branch -ne 'main') { throw "You are on '$branch'. Switch to 'main' before releasing." }

# Prevent duplicate tag
$tagName = "v$Version"
$existingTag = git tag --list $tagName
if ($existingTag) { throw "Tag $tagName already exists. Bump Version or delete the tag." }

# Stage/commit only if csproj is modified and not yet committed
$status = git status --porcelain
$csprojChanged = ($status | Select-String 'TheBackyard.csproj')
if ($csprojChanged) {
  git add .\TheBackyard.csproj
  git commit -m "chore(release): $Version"
} else {
  Write-Host "No changes to TheBackyard.csproj; skipping commit."
}

# Create and push tag, then push branch
git tag $tagName
git push origin main
git push origin $tagName

Write-Host "Pushed tag $tagName. Release workflow will build, pack, and publish."
param(
  [string]$Owner = 'davewil',
  [string]$Repo = 'Ex_TicketBuddy_ModularMonolith_To_Microservices',
  [switch]$DryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Require-GhCli {
  if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    throw 'GitHub CLI (gh) is required. Install from https://cli.github.com/ and run "gh auth login".'
  }
}

Require-GhCli

$repoSlug = "$Owner/$Repo"
Write-Host "[PLAN] Updating issues in $repoSlug" -ForegroundColor Cyan

# Completed issues from migrations_checklist.md
$issueNumbers = @(1,3,4,5,6,14,15,16,17,18,19,27,55)

foreach ($n in $issueNumbers) {
  $state = $null
  try {
    $state = gh issue view $n --repo $repoSlug --json state --jq .state 2>$null
  } catch {
    Write-Host "[SKIP] #$n not found" -ForegroundColor Yellow
    continue
  }

  if ($state -eq 'OPEN') {
    if ($DryRun) {
      Write-Host "[DO]   Would close #$n" -ForegroundColor Green
    } else {
      gh issue close $n --repo $repoSlug --comment 'Closed via migration checklist update (2025-09-10).' | Out-Null
      Write-Host "[DONE] Closed #$n" -ForegroundColor Green
    }
  } else {
    Write-Host "[INFO]  Already $state #$n" -ForegroundColor DarkGray
  }
}

Write-Host "Done." -ForegroundColor Green

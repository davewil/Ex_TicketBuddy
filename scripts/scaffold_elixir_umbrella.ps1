<#!
.SYNOPSIS
Scaffolds the Phoenix umbrella and apps for Ticket Buddy.

.DESCRIPTION
Creates the umbrella structure and apps:
  - api_gateway (Phoenix, no Ecto/assets)
  - core_events, core_users, core_tickets (domain libs)
  - messaging (Broadway/Oban placeholder)
  - shared_telemetry (OpenTelemetry bootstrap)

Prerequisites: Elixir/Erlang, Phoenix installer, Node (for Phoenix), Postgres (dev), RabbitMQ (optional).

.EXAMPLE
pwsh -File .\scripts\scaffold_elixir_umbrella.ps1
#>

param(
  [string]$BaseDir = 'Elixir',
  [string]$UmbrellaName = 'ticket_buddy_umbrella'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Assert-Command($name) {
  if (-not (Get-Command $name -ErrorAction SilentlyContinue)) {
    throw "Required command '$name' not found in PATH. Install it and retry."
  }
}

function Run($cmd) {
  Write-Host "» $cmd" -ForegroundColor Cyan
  & powershell -NoProfile -Command $cmd
  if ($LASTEXITCODE -ne 0) { throw "Command failed: $cmd" }
}

Write-Host 'Validating prerequisites…' -ForegroundColor Yellow
Assert-Command mix
Assert-Command erl

$basePath = Join-Path (Get-Location) $BaseDir
if (-not (Test-Path $basePath)) { New-Item -ItemType Directory -Path $basePath | Out-Null }

$umbrellaPath = Join-Path $basePath $UmbrellaName
if (Test-Path $umbrellaPath) {
  Write-Host "Umbrella already exists at $umbrellaPath" -ForegroundColor Green
} else {
  Push-Location $basePath
  try {
    Run "mix new $UmbrellaName --umbrella"
  } finally { Pop-Location }
}

Push-Location $umbrellaPath
try {
  # Phoenix api_gateway
  if (-not (Test-Path 'apps/api_gateway')) {
    Run "mix phx.new apps/api_gateway --no-ecto --no-html --no-live --no-assets --no-gettext"
  } else { Write-Host 'apps/api_gateway exists' -ForegroundColor DarkGray }

  # Domain libs
  foreach ($app in 'core_events','core_users','core_tickets','messaging','shared_telemetry') {
    if (-not (Test-Path ("apps/$app"))) {
      Run "mix new apps/$app"
    } else { Write-Host "apps/$app exists" -ForegroundColor DarkGray }
  }

  Write-Host "Scaffold complete. Next steps:" -ForegroundColor Yellow
  Write-Host "1) Edit mix.exs in each app to add deps (ecto, postgrex, broadway_rabbitmq, oban, opentelemetry, umbrella deps)." -ForegroundColor Yellow
  Write-Host "2) Create Repo modules in core_* apps and configure :ecto_repos + DATABASE_URL." -ForegroundColor Yellow
  Write-Host "3) Add shared_telemetry application start to bootstrap OTLP exporter." -ForegroundColor Yellow
  Write-Host "4) mix deps.get; mix compile; cd apps/api_gateway; mix phx.server" -ForegroundColor Yellow
} finally {
  Pop-Location
}

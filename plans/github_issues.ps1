<#!
.SYNOPSIS
Creates categorized GitHub labels, milestones, and issues to track the Elixir/BEAM migration.

.DESCRIPTION
This script uses the GitHub CLI (gh) to:
  - Ensure required labels exist
  - Ensure milestones exist
  - Create issues for each task in the migration plan, with labels and milestone assigned

Requires: GitHub CLI (https://cli.github.com/) authenticated to your account with access to the repo.

.PARAMETER Owner
GitHub repository owner. Defaults to 'davewil'.

.PARAMETER Repo
GitHub repository name. Defaults to 'Ex_TicketBuddy_ModularMonolith_To_Microservices'.

.PARAMETER DryRun
When provided, prints actions instead of creating them.

.EXAMPLE
pwsh -File .\plans\github_issues.ps1

.EXAMPLE
pwsh -File .\plans\github_issues.ps1 -Owner myorg -Repo myrepo -DryRun

#>

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

function Write-Plan {
  param([string]$Message)
  Write-Host "[PLAN] $Message" -ForegroundColor Cyan
}

function Write-Do {
  param([string]$Message)
  Write-Host "[DO]   $Message" -ForegroundColor Green
}

$repoSlug = "$Owner/$Repo"
Write-Plan "Target repo: $repoSlug"

Require-GhCli

# -----------------------------
# Labels
# -----------------------------
$labels = @(
  @{ name = 'migration'; color = '5319e7'; description = 'Elixir/BEAM migration' },
  @{ name = 'migration: foundations'; color = '0e8a16'; description = 'Phoenix/Ash foundations' },
  @{ name = 'migration: dev-env'; color = '1d76db'; description = 'Dev env & Docker' },
  @{ name = 'migration: ash-resources'; color = 'fbca04'; description = 'Ash resources & policies' },
  @{ name = 'migration: data-migration'; color = 'f9d0c4'; description = 'SQL Server → Postgres' },
  @{ name = 'migration: api-parity'; color = 'bfdadc'; description = 'API parity & shadowing' },
  @{ name = 'migration: messaging'; color = 'c2e0c6'; description = 'Broadway/Outbox/Oban' },
  @{ name = 'migration: observability'; color = '5319e7'; description = 'OpenTelemetry & tracing' },
  @{ name = 'migration: ci-cd'; color = 'd4c5f9'; description = 'CI/CD & quality gates' },
  @{ name = 'migration: cutover'; color = '006b75'; description = 'Cutover to Elixir' },
  @{ name = 'migration: governance'; color = 'e99695'; description = 'Labels/Milestones/Tracking' },
  @{ name = 'risk'; color = 'e11d21'; description = 'Risk & mitigation' }
)

Write-Plan 'Ensuring labels exist'
$existingLabels = gh label list --repo $repoSlug --limit 200 --json name | ConvertFrom-Json
$existingLabelNames = @()
if ($existingLabels) { $existingLabelNames = $existingLabels.name }

foreach ($l in $labels) {
  if ($existingLabelNames -contains $l.name) {
    Write-Plan "Label exists: $($l.name)"
  } else {
    if ($DryRun) {
      Write-Do "Create label: $($l.name) (#$($l.color))"
    } else {
      gh label create $l.name --repo $repoSlug --color $l.color --description $l.description | Out-Null
      Write-Do "Created label: $($l.name)"
    }
  }
}

# -----------------------------
# Milestones
# -----------------------------
$milestoneDefs = @(
  @{ title = 'Phase 1: Foundations'; description = 'Phoenix/Ash foundations and dev env'; },
  @{ title = 'Phase 2: Ash resources'; description = 'Model resources and baseline policies'; },
  @{ title = 'Phase 3: Data migration'; description = 'Backfill & cutover to Postgres'; },
  @{ title = 'Phase 4: API parity'; description = 'Shadow traffic & parity'; },
  @{ title = 'Phase 5: Messaging & Observability'; description = 'Broadway/Outbox + OTel'; },
  @{ title = 'Phase 6: CI/CD'; description = 'Build, test, contracts & containers'; },
  @{ title = 'Phase 7: Cutover'; description = 'Service-by-service cutover and decommission .NET'; }
)

Write-Plan 'Ensuring milestones exist'
$existingMilestones = gh api repos/$repoSlug/milestones --paginate | ConvertFrom-Json
$milestoneMap = @{}
foreach ($m in $milestoneDefs) {
  $found = $existingMilestones | Where-Object { $_.title -eq $m.title }
  if ($found) {
    $milestoneMap[$m.title] = $found.title  # gh can accept the milestone title
    Write-Plan "Milestone exists: $($m.title)"
  } else {
    if ($DryRun) {
      Write-Do "Create milestone: $($m.title)"
      $milestoneMap[$m.title] = $m.title
    } else {
      $created = gh api repos/$repoSlug/milestones -f title="$($m.title)" -f state='open' -f description="$($m.description)" | ConvertFrom-Json
      $milestoneMap[$m.title] = $created.title
      Write-Do "Created milestone: $($m.title)"
    }
  }
}

# -----------------------------
# Issues
# -----------------------------

function New-Task {
  param(
    [string]$Title,
    [string]$Body,
    [string[]]$Labels,
    [string]$Milestone
  )
  return [pscustomobject]@{ Title=$Title; Body=$Body; Labels=$Labels; Milestone=$Milestone }
}

$tasks = @()

# 1) Foundations
$tasks += New-Task 'Foundations: Create Phoenix umbrella and base apps' @'
Create Phoenix umbrella with apps: core_events, core_users, core_tickets, messaging, shared_telemetry, api_gateway.
Define umbrella structure and compile.
'@ @('migration','migration: foundations') 'Phase 1: Foundations'
$tasks += New-Task 'Foundations: Add core dependencies and compile' @'
Add phoenix, phoenix_ecto, ecto_sql, postgrex, ash, ash_postgres, ash_json_api, broadway, broadway_rabbitmq, oban, opentelemetry, opentelemetry_exporter. Compile successfully.
'@ @('migration','migration: foundations') 'Phase 1: Foundations'
$tasks += New-Task 'Foundations: Configure Repo and Postgres (dev/test)' 'Configure Ecto repo(s) and Postgres connections for dev/test.' @('migration','migration: foundations') 'Phase 1: Foundations'
$tasks += New-Task 'Foundations: Configure Oban and run migrations' 'Configure Oban queues and run required migrations.' @('migration','migration: foundations') 'Phase 1: Foundations'
$tasks += New-Task 'Foundations: Broadway scaffold and RabbitMQ connectivity' 'Add Broadway consumer skeleton and verify RabbitMQ connectivity.' @('migration','migration: foundations') 'Phase 1: Foundations'
$tasks += New-Task 'Foundations: OpenTelemetry setup' 'Configure OpenTelemetry OTLP exporter and verify spans in collector.' @('migration','migration: foundations','migration: observability') 'Phase 1: Foundations'

# 2) Dev environment & Docker
$tasks += New-Task 'Dev Env: Add Postgres to docker-compose' 'Add and configure Postgres service in compose alongside SQL Server.' @('migration','migration: dev-env') 'Phase 1: Foundations'
$tasks += New-Task 'Dev Env: Ensure RabbitMQ access for BEAM services' 'Place BEAM services on same network, validate credentials/connectivity.' @('migration','migration: dev-env') 'Phase 1: Foundations'
$tasks += New-Task 'Dev Env: Compose .NET and Elixir services side-by-side' 'Expose distinct ports; enable concurrent development.' @('migration','migration: dev-env') 'Phase 1: Foundations'
$tasks += New-Task 'Dev Env: Local run scripts/tasks' 'Add scripts/tasks to run .NET + Elixir stacks together.' @('migration','migration: dev-env') 'Phase 1: Foundations'
$tasks += New-Task 'Dev Env: Document env vars and connection strings' 'Document required configuration for both stacks.' @('migration','migration: dev-env','migration: governance') 'Phase 1: Foundations'

# 3) Ash resources
$tasks += New-Task 'Ash: Model Events resources and API' 'Create Ash resources for Events and expose via Ash API.' @('migration','migration: ash-resources') 'Phase 2: Ash resources'
$tasks += New-Task 'Ash: Model Users resources and API' 'Create Ash resources for Users and expose via Ash API.' @('migration','migration: ash-resources') 'Phase 2: Ash resources'
$tasks += New-Task 'Ash: Model Tickets resources and API' 'Create Ash resources for Tickets and expose via Ash API.' @('migration','migration: ash-resources') 'Phase 2: Ash resources'
$tasks += New-Task 'Ash: Generate and apply AshPostgres migrations' 'Generate deterministic migrations and apply to dev/test.' @('migration','migration: ash-resources') 'Phase 2: Ash resources'
$tasks += New-Task 'Ash: Seed scripts and sample data' 'Add seeds for smoke testing resources.' @('migration','migration: ash-resources') 'Phase 2: Ash resources'
$tasks += New-Task 'Ash: Baseline authorization with Ash.Policy' 'Centralize authorization for Users/Tickets actions/attributes.' @('migration','migration: ash-resources') 'Phase 2: Ash resources'

# 4) Data migration
$tasks += New-Task 'Data: Select ETL/CDC approach' 'Choose Debezium, periodic ETL, or custom sync for SQL Server → Postgres.' @('migration','migration: data-migration') 'Phase 3: Data migration'
$tasks += New-Task 'Data: Implement initial backfill job' 'Backfill Postgres from SQL Server and validate.' @('migration','migration: data-migration') 'Phase 3: Data migration'
$tasks += New-Task 'Data: Parity validation' 'Validate counts, invariants, and spot checks between stores.' @('migration','migration: data-migration') 'Phase 3: Data migration'
$tasks += New-Task 'Data: Route new Elixir writes to Postgres (feature-flag)' 'Enable new writes to Postgres with rollback toggle.' @('migration','migration: data-migration') 'Phase 3: Data migration'
$tasks += New-Task 'Data: Gradual read cutover to Postgres' 'Switch reads after validation; monitor and roll back if needed.' @('migration','migration: data-migration') 'Phase 3: Data migration'
$tasks += New-Task 'Data: Decommission SQL Server' 'Retire SQL Server post-stabilization.' @('migration','migration: data-migration') 'Phase 3: Data migration'

# 5) API parity
$tasks += New-Task 'API: Inventory .NET endpoints per context' 'Catalog endpoints for Events, Users, Tickets.' @('migration','migration: api-parity') 'Phase 4: API parity'
$tasks += New-Task 'API: Implement initial Phoenix controllers or AshJsonApi' 'Start with CRUD parity; use controllers for bespoke responses.' @('migration','migration: api-parity') 'Phase 4: API parity'
$tasks += New-Task 'API: Shadow GET traffic to Elixir and compare' 'Mirror GET requests; compare responses and log diffs.' @('migration','migration: api-parity') 'Phase 4: API parity'
$tasks += New-Task 'API: Shared JSON schemas & contract tests' 'Define schemas and add contract tests to CI.' @('migration','migration: api-parity','migration: ci-cd') 'Phase 4: API parity'
$tasks += New-Task 'API: Gradually point UI to Elixir routes' 'Switch UI endpoints as they reach parity.' @('migration','migration: api-parity') 'Phase 4: API parity'
$tasks += New-Task 'API: Achieve full parity for all contexts' 'Track completion when all endpoints match.' @('migration','migration: api-parity') 'Phase 4: API parity'

# 6) Messaging
$tasks += New-Task 'Messaging: Broadway consumers connected' 'Consume from existing RabbitMQ queues/bindings.' @('migration','migration: messaging') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Messaging: Ecto outbox and Oban publisher' 'Create outbox table and publisher worker.' @('migration','migration: messaging') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Messaging: Ash after_action → outbox' 'Enqueue domain events from Ash hooks.' @('migration','migration: messaging','migration: ash-resources') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Messaging: Publisher emits stable JSON contracts' 'Publish with headers and schemas; maintain compatibility.' @('migration','migration: messaging') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Messaging: Contract tests and header propagation' 'Verify message shapes and W3C trace context headers.' @('migration','migration: messaging','migration: observability') 'Phase 5: Messaging & Observability'

# 7) Observability
$tasks += New-Task 'Observability: OTLP exporter to collector' 'Send traces/metrics/logs to existing collector.' @('migration','migration: observability') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Observability: W3C trace context propagation' 'Propagate across HTTP and RabbitMQ.' @('migration','migration: observability') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Observability: Bridge Ash telemetry to OTel' 'Map Ash events to spans; add key spans around actions.' @('migration','migration: observability','migration: ash-resources') 'Phase 5: Messaging & Observability'
$tasks += New-Task 'Observability: Update dashboards and alerts' 'Include BEAM services in dashboards and alert rules.' @('migration','migration: observability') 'Phase 5: Messaging & Observability'

# 8) CI/CD & Quality gates
$tasks += New-Task 'CI/CD: Add Elixir build/test workflow' 'Add BEAM jobs to CI with unit and integration tests.' @('migration','migration: ci-cd') 'Phase 6: CI/CD'
$tasks += New-Task 'CI/CD: Enforce migration gen/verify' 'Ensure ash_postgres.generate_migrations & verify in CI.' @('migration','migration: ci-cd') 'Phase 6: CI/CD'
$tasks += New-Task 'CI/CD: Build container images' 'Build images for umbrella apps as needed.' @('migration','migration: ci-cd') 'Phase 6: CI/CD'
$tasks += New-Task 'CI/CD: Contract tests and smoke tests' 'Add API/message contract tests and smoke tests in pipeline.' @('migration','migration: ci-cd') 'Phase 6: CI/CD'
$tasks += New-Task 'CI/CD: Docs and changelog updates' 'Automate docs/changelog updates per iteration.' @('migration','migration: ci-cd','migration: governance') 'Phase 6: CI/CD'

# 9) Cutover
$tasks += New-Task 'Cutover: Launch Elixir Events API (shadow GET)' 'Deploy Events API and mirror GET traffic.' @('migration','migration: cutover') 'Phase 7: Cutover'
$tasks += New-Task 'Cutover: Switch POST/PUT for Events to Elixir' 'Route write traffic to Elixir for Events.' @('migration','migration: cutover') 'Phase 7: Cutover'
$tasks += New-Task 'Cutover: Switch Users to Elixir' 'Route Users traffic to Elixir.' @('migration','migration: cutover') 'Phase 7: Cutover'
$tasks += New-Task 'Cutover: Switch Tickets to Elixir' 'Route Tickets traffic to Elixir.' @('migration','migration: cutover') 'Phase 7: Cutover'
$tasks += New-Task 'Cutover: Move messaging fully to Elixir' 'Producers/consumers fully on Elixir.' @('migration','migration: cutover','migration: messaging') 'Phase 7: Cutover'
$tasks += New-Task 'Cutover: Decommission .NET services' 'Shut down and archive .NET services per context.' @('migration','migration: cutover') 'Phase 7: Cutover'

# 10) Risks & mitigations
$tasks += New-Task 'Risk: Confirm early Postgres migration' 'Reaffirm decision to move off SQL Server early.' @('migration','risk') 'Phase 3: Data migration'
$tasks += New-Task 'Risk: Enforce contract tests' 'Use schemas and contract tests to prevent drift.' @('migration','risk','migration: ci-cd') 'Phase 4: API parity'
$tasks += New-Task 'Risk: Validate trace propagation' 'Standardize headers and validate traces in dashboards.' @('migration','risk','migration: observability') 'Phase 5: Messaging & Observability'

# 11) Governance / tracking
$tasks += New-Task 'Governance: Create/maintain labels' 'Ensure migration labels exist and are used consistently.' @('migration','migration: governance') 'Phase 1: Foundations'
$tasks += New-Task 'Governance: Create/maintain milestones' 'Use milestones per migration phase.' @('migration','migration: governance') 'Phase 1: Foundations'
$tasks += New-Task 'Governance: Weekly status review' 'Hold weekly progress review and update tracking docs.' @('migration','migration: governance') 'Phase 1: Foundations'

Write-Plan "Preparing to create $($tasks.Count) issues"

$existingIssues = gh issue list --repo $repoSlug --limit 500 --state all --json number,title | ConvertFrom-Json
$existingTitles = @()
if ($existingIssues) { $existingTitles = $existingIssues.title }

foreach ($t in $tasks) {
  if ($existingTitles -contains $t.Title) {
    Write-Plan "Issue exists: $($t.Title)"
    continue
  }

  $labelArgs = @()
  foreach ($ln in $t.Labels) { $labelArgs += @('--label', $ln) }

  $msTitle = $t.Milestone
  if ($DryRun) {
    Write-Do "Create issue: $($t.Title) [Milestone=$msTitle] Labels=[$($t.Labels -join ', ')]"
  } else {
    gh issue create --repo $repoSlug --title $t.Title --body $t.Body @labelArgs --milestone $msTitle | Out-Null
    Write-Do "Created issue: $($t.Title)"
  }
}

Write-Host "Done." -ForegroundColor Green

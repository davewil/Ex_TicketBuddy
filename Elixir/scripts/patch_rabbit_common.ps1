# Applies a minimal fix to rabbit_common for OTP 28 by replacing an undefined macro in rabbit_cert_info.erl
param(
    [string]$UmbrellaPath = "$PSScriptRoot\..\ticket_buddy_umbrella"
)

$ErrorActionPreference = 'Stop'

$target = Join-Path $UmbrellaPath 'deps\rabbit_common\src\rabbit_cert_info.erl'
if (-not (Test-Path $target)) {
  Write-Error "File not found: $target`nRun 'mix deps.get' first."
  exit 1
}

$content = Get-Content -Raw -LiteralPath $target
$before = '{?\'street-address\'               , "STREET"},'
$after  = '{{2,5,4,9}                      , "STREET"},'  # OID for streetAddress

if ($content -like "*${before}*") {
  $content = $content -replace [regex]::Escape($before), [System.Text.RegularExpressions.Regex]::EscapeReplacement($after)
  Set-Content -LiteralPath $target -Value $content -Encoding UTF8
  Write-Host "Patched rabbit_common: replaced 'street-address' macro with OID tuple (2,5,4,9)."
} elseif ($content -like "*${after}*") {
  Write-Host "Patch already applied."
} else {
  Write-Warning "Patch pattern not found. File may have changed; manual review recommended: $target"
}

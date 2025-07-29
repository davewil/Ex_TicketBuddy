#!/bin/sh
set -e

echo "Running database migrations..."
dotnet Host.Migrations.dll

echo "Starting API..."
exec dotnet Host.Api.dll

#!/usr/bin/env bash
set -euo pipefail

dotnet restore

dotnet publish   -c Release -r linux-x64 --self-contained false   -p:PublishSingleFile=false -o ./publish

echo "Publicación en ./publish lista"

#!/usr/bin/env bash
set -euo pipefail

JOB_PATH=${JOB_PATH:-"/opt/jobs/SampleJob/SampleJob.dll"}
export DOTNET_ENV=${DOTNET_ENV:-Production}

if [ ! -f "$JOB_PATH" ]; then
  echo "[dotnet-runner] Job no encontrado en $JOB_PATH" >&2
  ls -la /opt/jobs || true
  exit 1
fi

echo "[dotnet-runner] Ejecutando: dotnet $JOB_PATH $JOB_ARGS"
dotnet "$JOB_PATH" ${JOB_ARGS:-}

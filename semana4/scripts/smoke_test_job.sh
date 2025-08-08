#!/usr/bin/env bash
set -euo pipefail

docker run --rm -v /srv/dotnet-jobs:/opt/jobs:ro   -e JOB_PATH=/opt/jobs/SampleJob/SampleJob.dll   -e JOB_ARGS="--check health"   org/dotnet-runner:8.0

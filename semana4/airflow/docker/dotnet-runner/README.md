Construcción local:

```bash
docker build -t org/dotnet-runner:8.0 .
```

Ejecución local:

```bash
docker run --rm -v /srv/dotnet-jobs:/opt/jobs:ro   -e JOB_PATH=/opt/jobs/SampleJob/SampleJob.dll   org/dotnet-runner:8.0
```

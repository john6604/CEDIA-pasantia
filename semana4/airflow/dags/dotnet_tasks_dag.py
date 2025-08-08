from datetime import datetime, timedelta
from airflow import DAG
from airflow.providers.docker.operators.docker import DockerOperator
from airflow.models import Variable

# Variables de Airflow:
#   DOTNET_JOBS_HOST_PATH -> ruta en el host con los jobs publicados (p.ej. /srv/dotnet-jobs)
#   DOTNET_RUNNER_IMAGE   -> imagen del runner (p.ej. org/dotnet-runner:8.0)

with DAG(
    dag_id="dotnet_tasks",
    description="Ejecución programada de jobs .NET 8 en contenedor Linux",
    start_date=datetime(2025, 8, 1),
    schedule_interval=None,
    catchup=False,
    default_args={
        "owner": "ops",
        "retries": 1,
        "retry_delay": timedelta(minutes=5),
    },
    tags=["dotnet", ".NET", "runner"],
) as dag:

    sample_job = DockerOperator(
        task_id="run_sample_job",
        image=Variable.get("DOTNET_RUNNER_IMAGE", default_var="org/dotnet-runner:8.0"),
        api_version="auto",
        auto_remove=True,
        docker_url="unix://var/run/docker.sock",
        mount_tmp_dir=False,
        command=None,  # usa entrypoint del contenedor
        mounts=[
            {
                "Target": "/opt/jobs",
                "Source": Variable.get("DOTNET_JOBS_HOST_PATH", default_var="/srv/dotnet-jobs"),
                "Type": "bind",
                "ReadOnly": True,
            }
        ],
        environment={
            "JOB_PATH": "/opt/jobs/SampleJob/SampleJob.dll",
            "DOTNET_ENV": "Production",
            "JOB_ARGS": "--check health --timeout 30"
        },
    )

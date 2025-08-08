# Airflow + .NET 8 Runner y Migración MariaDB (Windows → Linux)

Este repo consolida:
- **Ejecución programada** de ejecutables .NET 8 en Linux mediante **Airflow** y un contenedor **dotnet-runner**.
- **Migración** de **MariaDB** desde Windows Server 2019 a **Linux** con Docker.

## Cómo usar

### 1) Construir el runner y publicar jobs
```bash
cd airflow/docker/dotnet-runner && docker build -t org/dotnet-runner:8.0 .
cd ../../.. && cd dotnet-jobs/SampleJob && ./publish.sh
```

### 2) Desplegar jobs al servidor Linux
```bash
REMOTE_HOST=airflow-linux REMOTE_PATH=/srv/dotnet-jobs ./scripts/deploy_jobs_to_linux.sh
```

### 3) Probar un job localmente
```bash
./scripts/smoke_test_job.sh
```

### 4) Configurar Airflow
- Variables:
  - `DOTNET_RUNNER_IMAGE = org/dotnet-runner:8.0`
  - `DOTNET_JOBS_HOST_PATH = /srv/dotnet-jobs`
- Copiar `airflow/dags/dotnet_tasks_dag.py` a la carpeta de DAGs.
- Habilitar el DAG y ejecutar `run_sample_job`.

### 5) Migración MariaDB
1. Backup en Windows (mysqldump/mariabackup).
2. Copiar dump al host Linux.
3. Levantar MariaDB: `cd mariadb && docker compose up -d --build`.
4. Restaurar: `mysql -h 127.0.0.1 -P 3306 -u root -p < /ruta/dump.sql` o usar script/CI.

## Notas
- Para **Samba**: si prefieren compartir `/srv/dotnet-jobs` vía SMB, ajustar `scripts/deploy_jobs_to_linux.sh`.
- Para **registry**: publicar `org/dotnet-runner:8.0` en GHCR/Harbor y referenciar desde Airflow.

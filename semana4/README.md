# Airflow + .NET 8 Runner and MariaDB Migration (Windows → Linux)

This repository includes:
- **Scheduled execution** of .NET 8 executables on Linux using **Airflow** and a dedicated **dotnet-runner** container.
- **Migration** of **MariaDB** from Windows Server 2019 to **Linux** using Docker.

## Usage

### 1) Build the runner and publish jobs
```bash
cd airflow/docker/dotnet-runner && docker build -t org/dotnet-runner:8.0 .
cd ../../.. && cd dotnet-jobs/SampleJob && ./publish.sh
```

### 2) Deploy jobs to the Linux server
```bash
REMOTE_HOST=airflow-linux REMOTE_PATH=/srv/dotnet-jobs ./scripts/deploy_jobs_to_linux.sh
```

### 3) Test a job locally
```bash
./scripts/smoke_test_job.sh
```

### 4) Configure Airflow
- Set Variables:
  - `DOTNET_RUNNER_IMAGE = org/dotnet-runner:8.0`
  - `DOTNET_JOBS_HOST_PATH = /srv/dotnet-jobs`
- Copy `airflow/dags/dotnet_tasks_dag.py` to the Airflow DAGs folder.
- Enable the DAG and execute the `run_sample_job` task.

### 5) MariaDB Migration
1. Create a backup on Windows (mysqldump/mariabackup).
2. Transfer the dump file to the Linux host.
3. Start MariaDB: `cd mariadb && docker compose up -d --build`.
4. Restore: `mysql -h 127.0.0.1 -P 3306 -u root -p < /path/to/dump.sql` or use a script/CI pipeline.

## Notes
- For **Samba**: if you prefer to share `/srv/dotnet-jobs` via SMB, adjust `scripts/deploy_jobs_to_linux.sh`.
- For **registry**: push `org/dotnet-runner:8.0` to GHCR/Harbor and reference it from Airflow.

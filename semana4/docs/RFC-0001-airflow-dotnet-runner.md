# RFC-0001: Airflow + .NET Runner

**Contexto**: Sustituir tareas en Windows Task Scheduler por orquestación en Airflow.

**Decisiones**:
- DockerOperator con volumen `/srv/dotnet-jobs`.
- Imagen `org/dotnet-runner:8.0` basada en runtime oficial .NET 8.
- Jobs publicados `linux-x64` no self-contained.

**Pendientes**:
- Variables/credenciales vía Airflow Connections/Secrets.
- Logs centralizados (ELK/Opensearch) opcional.

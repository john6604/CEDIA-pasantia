Este módulo contiene:
- Un **DAG** que agenda y monitorea la ejecución de ejecutables .NET 8 en Linux.
- Un contenedor **dotnet-runner** que encapsula el runtime de .NET 8 y ejecuta un job específico.

Requisitos:
- Airflow >= 2.6 con acceso a Docker.
- Volumen o ruta compartida con los jobs publicados (por defecto `/srv/dotnet-jobs`).

Objetivo: migrar MariaDB Community de Windows Server 2019 a Linux con Docker.

Pasos sugeridos:
1. Generar **backup** en Windows (mysqldump o mariabackup).
2. Levantar `docker-compose` en Linux.
3. Restaurar el dump dentro del contenedor o desde el host.
4. Validar integridad (usuarios, rutinas, eventos, triggers) y ajustar `sql_mode`, timezones y collation si es necesario.

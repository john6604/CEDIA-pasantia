# RFC-0002: Migración de MariaDB (Windows → Linux Docker)

**Motivación**: Unificar el stack operativo en Linux, facilitar parches y despliegues.

**Alcance**:
- Migración con dump lógico.
- Revisión de collation y `sql_mode`.
- Verificación de usuarios, grants y eventos.

**Riesgos**:
- Diferencias de timezones, path, plugins.
- Dependencias externas.

**Plan de pruebas**:
- Restaurar en staging.
- Ejecutar queries representativos.
- Validar rendimiento y compatibilidad.

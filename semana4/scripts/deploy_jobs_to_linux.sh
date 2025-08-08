#!/usr/bin/env bash
set -euo pipefail

# Ejemplo con rsync+ssh. Para Samba, reemplazar con smbclient o montar CIFS.
REMOTE_HOST=${REMOTE_HOST:-airflow-linux}
REMOTE_PATH=${REMOTE_PATH:-/srv/dotnet-jobs}
LOCAL_PATH=${LOCAL_PATH:-./dotnet-jobs}

rsync -av --delete   --include '*/' --include 'publish/**' --exclude '*'   "$LOCAL_PATH"/ "$REMOTE_HOST:$REMOTE_PATH/"

echo "Despliegue completado en $REMOTE_HOST:$REMOTE_PATH"

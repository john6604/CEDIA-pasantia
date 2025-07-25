# Ruta de salida de la compilación de Genexus
$buildPath = "C:\Users\jchim\Documents\CEDIA\FarmaciaKB\NETFrameworkPostgreSQL003\web"
# Ruta de destino en IIS
$deployPath = "C:\inetpub\wwwroot\FarmaciaKB"
# Ruta para backups
$backupPath = "C:\Backups\FarmaciaKB"
# Ruta del log
$logFile = "C:\logs\deploy-sia.log"

# Obtener fecha para backup y log
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"

# 1. Crear backup
$backupTarget = "$backupPath\Backup-$timestamp"
Write-Host "Creando backup en $backupTarget..."
Copy-Item -Path $deployPath -Destination $backupTarget -Recurse -Force

# 2. Copiar archivos nuevos
Write-Host "Copiando archivos desde $buildPath a $deployPath..."
Copy-Item -Path "$buildPath\*" -Destination $deployPath -Recurse -Force

# 3. Reiniciar App Pool (ajusta el nombre si es distinto)
$AppPoolName = "DefaultAppPool"
Write-Host "Reiniciando App Pool: $AppPoolName..."
Import-Module WebAdministration
Restart-WebAppPool $AppPoolName

# 4. Registrar log
$logEntry = "$timestamp - Despliegue exitoso realizado. Archivos copiados y AppPool '$AppPoolName' reiniciado."
Add-Content -Path $logFile -Value $logEntry

Write-Host "Despliegue completado correctamente."

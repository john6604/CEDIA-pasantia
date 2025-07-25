# FarmaciaKB Auto-Deployment Script (.NET Framework - IIS)

A PowerShell script to automate the deployment of a GeneXus application targeting the .NET Framework on IIS.  
The script copies the build output to the web root, creates backups, restarts the App Pool, and logs the deployment.

## Requirements

* PowerShell 5+
* IIS with `DefaultAppPool` or custom App Pool for your site
* Folder structure:
  * GeneXus build output:
    `C:\Users\jchim\Documents\CEDIA\FarmaciaKB\NETFrameworkPostgreSQL003\web`
  * IIS site directory:
    `C:\inetpub\wwwroot\FarmaciaKB`
  * Backup directory:
    `C:\Backups\FarmaciaKB`
  * Log file:
    `C:\logs\deploy-sia.log`

## Installation

Clone this repository or download the script into your working directory:

```powershell
git clone https://github.com/john6604/CEDIA-pasantia.git
cd CEDIA-pasantia
cd semana2
```

## Usage

Open PowerShell **as Administrator** and run:

```powershell
.\deploy-sia.ps1
```

## What It Does

1. Creates a timestamped backup of the current IIS web directory.
2. Copies the latest GeneXus build output to the IIS root.
3. Restarts the configured Application Pool (`DefaultAppPool` by default).
4. Appends a log entry in `C:\logs\deploy-sia.log` with the deployment status and timestamp.

## Configuration

If your App Pool is not `DefaultAppPool`, you can update the script:

```powershell
$AppPoolName = "YourCustomAppPoolName"
```

You may also modify the paths if your environment differs:

```powershell
$buildPath = "your_genexus_output_path"
$deployPath = "your_iis_web_root"
$backupPath = "your_backup_folder"
$logFile = "your_log_file_path"
```

## Logging

Each deployment generates a line in:

```
C:\logs\deploy-sia.log
```

Example:

```
2025-07-25-142311 - Deployment completed successfully. Files copied and AppPool 'DefaultAppPool' restarted.
```

## Troubleshooting

* Make sure execution policy allows running scripts:
```powershell
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
```

* Ensure all paths exist or create them manually.
* Run PowerShell as Administrator to avoid permission issues.


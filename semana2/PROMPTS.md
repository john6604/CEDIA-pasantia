# PROMPTS.md – Week 2: Deployment Automation

This document contains the AI prompts and guidance used during the creation and testing of the deployment automation script for the FarmaciaKB application.

## Prompts Used

### General Understanding

- "What is a PowerShell script for automating IIS deployment?"
- "How do I create a script that backs up a website, copies files to IIS, and restarts an App Pool?"

### Troubleshooting

- "Why am I getting a 'TerminatorExpectedAtEndOfString' error in PowerShell?"
- "How to fix PowerShell parse errors caused by invalid characters or emoji?"

### Customization

- "Update this PowerShell script with the following real paths:"
  - Build path: `C:\Users\jchim\Documents\CEDIA\FarmaciaKB\NETFrameworkPostgreSQL003\web`
  - Deploy path: `C:\inetpub\wwwroot\FarmaciaKB`
  - Backup path: `C:\Backups\FarmaciaKB`
  - Log file: `C:\logs\deploy-sia.log`

### Validation

- "How can I find the Application Pool name used by my IIS website?"
- "How do I verify that my script copied files and restarted the App Pool correctly?"

### Execution

- "What do I need to run a PowerShell script as administrator?"
- "How do I allow script execution if PowerShell blocks it?"

## Outcome

This series of prompts resulted in the creation of a functional PowerShell deployment script that:

- Automatically backs up the IIS folder
- Copies updated build files
- Restarts the App Pool
- Logs the operation with a timestamp

This script is now used in the FarmaciaKB project as part of the Week 2 deliverables in the Genexus technical internship at CEDIA.

# Cedia.Helpers (.NET 8 + .NET Framework 4.8)

A shared multi-targeted library that provides cross-platform utility helpers for CEDIA applications.  
Supports both **.NET Framework 4.8** and **.NET 8.0+**, ensuring maximum compatibility between legacy and modern projects.

## Requirements

* .NET SDK 8.0+ (for modern builds)
* .NET Framework 4.8 Developer Pack (for legacy compatibility)
* Visual Studio 2022+ or `dotnet` CLI with multi-targeting support

The library supports **multi-targeting** using the following setup in the `.csproj` file:

```xml
<TargetFrameworks>net48;net8.0</TargetFrameworks>
<LangVersion>8.0</LangVersion>
<Nullable>enable</Nullable>
```

## Key Features

* Multi-targeted to `net48` and `net8.0`
* Uses conditional compilation (`#if NET48`, `#if NET8_0_OR_GREATER`) to ensure compatibility
* DRY & KISS principles across helpers
* Compatible with common packages like:
  * `System.Net.Mail`
  * `System.Security.Cryptography`
  * `IronPython` (for .NET Framework only)
  * `iTextSharp` (for .NET Framework only)

## Structure

| File/Helper | Description |
|-------------|-------------|
| `EmailSender.cs` | Sends email using `SmtpClient` with optional To/CC/BCC/attachments, TLS enabled |
| `IronPythonHelper.cs` | Executes Python scripts dynamically (only on .NET Framework) |
| `PdfSignatureHelper.cs` | Reads digital signatures from PDF using iTextSharp (only on .NET Framework) |
| `UrlEncryptionHelper.cs` | Encrypts parameters, generates secure URLs, cross-framework encoding support |

## Installation

```bash
git clone https://github.com/john6604/CEDIA-pasantia.git
cd CEDIA-pasantia
cd semana3
dotnet restore
```

## Usage

Example usage in a multi-targeted consumer app:

```csharp
var result = EmailSender.SendEmail(
    senderEmail: "noreply@cedia.org.ec",
    senderPassword: "secret",
    toRecipients: new List<string> { "admin@cedia.org.ec" },
    subject: "Hello",
    body: "<p>This is a test email</p>",
    smtpHost: "smtp.office365.com",
    smtpPort: 587
);
```

To execute Python scripts (only on .NET Framework):

```csharp
string output = IronPythonHelper.ExecuteScript("script.py", "C:\\python\\libs", "myfunction()");
```

## Building

```bash
dotnet build
```

Build outputs:

- `bin/Debug/net48/Cedia.Helpers.dll`
- `bin/Debug/net8.0/Cedia.Helpers.dll`
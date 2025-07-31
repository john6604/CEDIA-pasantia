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
  * `Docotic.Pdf` (planned for .NET 8 support in future)

## Structure

| File/Helper | Description |
|-------------|-------------|
| `EmailSender.cs` | Sends email using `SmtpClient` with optional To/CC/BCC/attachments, TLS enabled |
| `IronPythonHelper.cs` | Executes Python scripts dynamically (IronPython in .NET Framework, Python.NET optional in .NET 8) |
| `PdfSignatureHelper.cs` | Reads digital signatures from PDFs using iTextSharp (NET48) or Docotic.Pdf (planned for NET8) |
| `UrlEncryptionHelper.cs` | Provides AES-based encryption for secure link generation with base64 and URL-safe encoding |

## Installation

```bash
git clone https://github.com/john6604/CEDIA-pasantia.git
cd CEDIA-pasantia
cd semana3
cd <DLL Selected>
dotnet restore
```

## Usage

### EmailSender

Sends HTML-formatted emails with optional CC, BCC and file attachments:

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

### IronPythonHelper

Executes Python functions in external scripts.  
**Only supported in .NET Framework 4.8 with IronPython**, and optionally with Python.NET in .NET 8 if enabled via `USE_PYTHONNET`.

```csharp
string result = PythonHelper.ExecuteScript("script.py", @"C:\python\libs", "my_function()");
```

### PdfSignatureHelper

Extracts information from digitally signed PDFs, such as the signer name, validity period, and certificate issuer:

```csharp
var certs = PdfSignatureHelper.ReadSignaturesFromPdf("signed.pdf");

foreach (var cert in certs)
{
    Console.WriteLine($"Signed by: {cert.Name}, Issuer: {cert.Issuer}, Valid: {cert.ValidFrom} - {cert.ValidTo}");
}
```

**Note:** Uses `iTextSharp` in .NET Framework and is planned to use `Docotic.Pdf` in .NET 8+ for future compatibility.

### UrlEncryptionHelper

Generates secure links with AES encryption and base64 URL-safe encoding. Includes checksum to verify integrity.

```csharp
string siteKey = UrlEncryptionHelper.GetSiteKey();
string secureUrl = UrlEncryptionHelper.GenerateSecureLink(
    baseUrl: "https://example.com",
    route: "validate",
    code: "abc123",
    siteKey: siteKey
);

// Example output:
// https://example.com/validate?ENCRYPTEDTOKEN
```

## Building

```bash
dotnet build
```

Build outputs:

- `bin/Debug/net48/Cedia.Helpers.dll`
- `bin/Debug/net8.0/Cedia.Helpers.dll`


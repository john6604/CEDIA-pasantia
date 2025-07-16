# Pasantía Técnica CEDIA — Semana 1

Este repositorio contiene:
- Una aplicación básica creada con Genexus.
- Un External Object en C# que integra la librería **Humanizer** para convertir números a texto.

---

# Estructura del repositorio
├── GenexusApp/
│ └── MiAplicacionGenexus.xpz
├── ExternalObject/
│ ├── MyExtensions.csproj
│ ├── TextHelper.cs
│ └── .gitignore
├── README.md
├── PROMPTS.md
└── DocumentoTecnicoSemana1.docx

---

# Aplicación Genexus

- **Archivo:** `MiAplicacionGenexus.xpz`
- Descripción: KB exportada desde el curso oficial de Genexus.
- Para usar:
  1. Abre Genexus.
  2. Importa el `.xpz` desde `Tools > Knowledge Manager > Import`.

---

# External Object en C#

- **Carpeta:** `/ExternalObject/`
- Descripción: Contiene el proyecto de biblioteca de clases `.NET Framework` que usa **Humanizer**.
- Clase principal: `TextHelper.cs` con la función `NumeroATexto`.

---

# Cómo compilar la DLL

1. Abre `MyExtensions.csproj` en Visual Studio (VS 2022 recomendado).
2. Restaura las dependencias NuGet:
dotnet restore
o usa `Build > Restaurar NuGet Packages` en VS.
3. Compila:
dotnet build
Esto generará `MyExtensions.dll` en `/bin/Debug/`.

---

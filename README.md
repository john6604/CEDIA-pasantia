# Pasantía Técnica CEDIA — Semana 1

Este repositorio contiene:
- Una aplicación básica creada con Genexus.
- Un External Object en C# que integra la librería **Humanizer** para convertir números a texto.

---

# Aplicación Genexus

- **Archivo:** `Farmacia.gxw`
- Descripción: KB exportada desde el curso oficial de Genexus.
- Para usar:
  1. Abre Genexus.
  2. Importa el `.gxw` desde `File > Open > Knowledge Base`.

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

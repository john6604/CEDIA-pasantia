# GeneXus + Humanizer Integration (.NET Framework)

A basic Genexus application extended with a C# External Object that uses the Humanizer library to convert numbers into words. Developed during Week 1 of the CEDIA Technical Internship Program.

---

## 📋 Requirements

* Genexus Trial or Licensed IDE (tested with Genexus 18)
* Visual Studio 2022 with **.NET Framework 4.8** development workload
* NuGet installed
* Git

---

##  Repository Structure

```
/week1/
 ├── GenexusApp/
 │   └── Farmacia.xpz      
 ├── HumanizerUtils/   
 │   ├── packages/ 
 │   ├── .vs/HumanizerLibrary/ 
 │   └── HumanizerLibrary/    
 │   	├── Properties/ 
 │   	├── HumanizerLibrary.csproj          
 │   	├── Class1.cs  
 │   	├── packages.config                    
 │   	└── .gitignore      
 ├── README.md
 ├── PROMPTS.md                           
 └── TechnicalIntegration_ExternalObject_Week1.docx         
```

---

##  External Object in C#

**Directory:** `/HumanizerUtils/HumanizerLibrary/`  
**Main class:** `TextHelper`  
**Method exposed:**  
```csharp
public string NumberToText(int numero)
```

**NuGet dependency:** [Humanizer](https://github.com/Humanizr/Humanizer)  
Installed via:
```bash
dotnet add package Humanizer
```

###  Build Instructions

```bash
cd HumanizerUtils
cd HumanizerLibrary
dotnet restore
dotnet build
```

The DLL will be located in `/bin/Debug/`.

---

##  Genexus Integration

1. Import `Farmacia.xpz` into Genexus:  
   Go to `Knowledge Manager > Import` and select the file.

2. Copy the compiled `HumanizerLibrary.dll` to the KB's references folder.

3. Create an **External Object** in Genexus:
   - Object name: `TextHelper`
   - Method: `NumberToText` with one input parameter (`int`) and a `string` return type.

4. Use it inside a **Procedure** with the `CSHARP` command.

####  Sample Genexus Code

```genexus
&Salida=&HumanizerUtilityTextHelper.NumeroATexto(&NumeroPrueba)
CSHARP System.Console.WriteLine([!&Salida!]);
```

---

## Intern

- John David Chimbo Pintado

## Tutors

- Jonnathan Sanango
- Xavier Espinoza

# GeneXus + Humanizer Integration (.NET Framework)

A basic Genexus application extended with a C# External Object that uses the Humanizer library to convert numbers into words. Developed during Week 1 of the CEDIA Technical Internship Program.

---

## 📋 Requirements

* Genexus Trial or Licensed IDE (tested with Genexus 17)
* Visual Studio 2022 with **.NET Framework 4.8** development workload
* NuGet installed
* Git

---

## 📁 Repository Structure

```
/week1/
 ├── GenexusApp/
 │   └── Farmacia.xpz                     # Exported Genexus KB
 ├── HumanizerUtils/
 │   ├── HumanizerLibrary.csproj          # C# Class Library (.NET Framework 4.8)
 │   ├── TextHelper.cs                    # Exposes NumberToText(int) method
 │   └── .gitignore
 ├── README.md
 ├── PROMPTS.md                           # AI prompts used during development
 └── TechnicalDocument_Week1.docx         # Technical documentation (in progress)
```

---

## 🧩 External Object in C#

**Directory:** `/HumanizerUtils/`  
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

### 🔧 Build Instructions

```bash
cd HumanizerUtils
dotnet restore
dotnet build
```

The DLL will be located in `/bin/Debug/`.

---

## 🧠 Genexus Integration

1. Import `Farmacia.xpz` into Genexus:  
   Go to `Knowledge Manager > Import` and select the file.

2. Copy the compiled `HumanizerLibrary.dll` to the KB's references folder.

3. Create an **External Object** in Genexus:
   - Object name: `TextHelper`
   - Method: `NumberToText` with one input parameter (`int`) and a `string` return type.

4. Use it inside a **Procedure** with the `CSHARP` command.

#### 🧪 Sample Genexus Code

```genexus
&result = TextHelper.NumberToText(123)
msg(&result) // Outputs: "one hundred and twenty-three"
```

---

## ✅ Deliverables for Week 1

- [x] Genexus application created and exported
- [x] External Object in C# using Humanizer
- [x] Functional integration via `CSHARP`
- [x] Source code versioned on GitHub
- [ ] `PROMPTS.md` documented
- [ ] Technical report for Week 1

---

## 👨‍💻 Intern

- [Your Full Name]

## 👨‍🏫 Tutors

- Jonnathan Sanango
- Xavier Espinoza

---

## License

MIT

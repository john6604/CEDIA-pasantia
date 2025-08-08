Coloca aquí los proyectos de consola (.NET 8). Publicar con:

```bash
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
```

Estructura esperada en el servidor Linux (montada en `/srv/dotnet-jobs`):

```
/srv/dotnet-jobs/
  SampleJob/
    SampleJob.dll
    *.deps.json
    *.runtimeconfig.json
    ...
```

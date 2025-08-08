var env = Environment.GetEnvironmentVariable("DOTNET_ENV") ?? "Development";
Console.WriteLine($"[SampleJob] Starting in {env}...");

var argsEnv = Environment.GetEnvironmentVariable("JOB_ARGS") ?? string.Empty;
Console.WriteLine($"[SampleJob] JOB_ARGS = {argsEnv}");

// Lógica mínima de ejemplo: interpretar un flag genérico --check <target>
if (argsEnv.Contains("--check"))
{
    Console.WriteLine("[SampleJob] Check mode: realizando verificación de salud genérica...");
    await Task.Delay(500);
    Console.WriteLine("[SampleJob] OK");
}

Console.WriteLine("[SampleJob] Done.");

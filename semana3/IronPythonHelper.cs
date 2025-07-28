using System;
using System.Collections.Generic;

#if NET48
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
#endif

namespace Cedia.Common.Helpers
{
    public static class IronPythonHelper
    {
        public static string ExecuteScript(string scriptPath, string searchPath, string expression)
        {
#if NET48
            try
            {
                Console.WriteLine($"[INFO] Script path: {scriptPath}");
                Console.WriteLine($"[INFO] Python expression: {expression}");

                var engine = Python.CreateEngine();

                // Add search path for module imports
                var paths = engine.GetSearchPaths();
                paths.Add(searchPath);
                engine.SetSearchPaths(paths);

                var source = engine.CreateScriptSourceFromFile(scriptPath);
                var compiled = source.Compile();
                var scope = engine.CreateScope();

                compiled.Execute(scope);

                var result = engine.Execute(expression, scope) as string;
                return result ?? string.Empty;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return $"ERROR: {ex.Message}";
            }
#else
            Console.WriteLine("[WARN] IronPython is not supported in .NET 8.0. This method is only functional on .NET Framework.");
            return "ERROR: Unsupported in .NET 8";
#endif
        }

        private static void LogException(Exception ex)
        {
            Console.WriteLine($"[ERROR] Message: {ex.Message}");
            Console.WriteLine($"[ERROR] Type: {ex.GetType().Name}");
            Console.WriteLine($"[ERROR] Method: {ex.TargetSite?.Name}");
            Console.WriteLine($"[ERROR] StackTrace:\n{ex.StackTrace}");
        }
    }
}

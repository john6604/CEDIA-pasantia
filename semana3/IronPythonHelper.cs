using System;

#if NET48 || IRONPYTHON
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
#elif NET8_0_OR_GREATER && USE_PYTHONNET
using Python.Runtime;
#endif

namespace Cedia.Common.Helpers
{
    public static class PythonHelper
    {
        public static string ExecuteScript(string scriptPath, string searchPath, string expression)
        {
#if NET48
            var engine = Python.CreateEngine();
            var paths = engine.GetSearchPaths();
            paths.Add(searchPath);
            engine.SetSearchPaths(paths);
            var scope = engine.CreateScope();
            engine.ExecuteFile(scriptPath, scope);
            var result = engine.Execute(expression, scope) as string;
            return result ?? string.Empty;

#elif NET8_0_OR_GREATER
#if USE_PYTHONNET
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic scope = Py.Import(scriptPath);
                dynamic func = scope.__dict__.GetItem(expression);
                dynamic res = func();
                return res?.ToString() ?? string.Empty;
            }
#else
            Console.WriteLine("[WARN] Python execution not supported on .NET 8 (no Python runtime enabled).");
            return "ERROR: Unsupported on .NET 8";
#endif
#else
            return "ERROR: Unsupported platform";
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Cedia.ExecuteScript
{
    public class ExecuteScriptHelper
    {
        public static string RunPythonProcedure(string scriptPath, string libPath, string procedure)
        {
            try
            {
                ScriptEngine pythonEngine = Python.CreateEngine();

                ICollection<string> paths = pythonEngine.GetSearchPaths();
                paths.Add(libPath);
                pythonEngine.SetSearchPaths(paths);

                ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromFile(scriptPath);
                CompiledCode code = pythonScript.Compile();

                ScriptScope scope = pythonEngine.CreateScope();

                code.Execute(scope);

                var result = pythonEngine.Execute(procedure, scope);

                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}\nTYPE: {ex.GetType().Name}\nMETHOD: {ex.TargetSite}\nSTACKTRACE: {ex.StackTrace}";
            }
        }
    }
}

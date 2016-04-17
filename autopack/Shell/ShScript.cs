using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace autopack
{
    public class ShScript : IScript
    {
        public void runCommand(string nWorkDirectory, string nCommand)
        {
            if (!mStop) return;

            mStop = false;

            mProcess = new Process();
            mProcess.StartInfo.FileName = nCommand;
            mProcess.StartInfo.WorkingDirectory = nWorkDirectory;
            mProcess.StartInfo.UseShellExecute = false;
            mProcess.StartInfo.RedirectStandardOutput = true;

            mProcess.OutputDataReceived += new DataReceivedEventHandler(outputDataReceived);
            mProcess.EnableRaisingEvents = true;
            mProcess.Exited += new EventHandler(processExited);

            mProcess.Start();

            mProcess.BeginOutputReadLine();
        }
        public void runInit()
        {
            mStop = true;
        }
        public bool isStop()
        {
            return mStop;
        }
        private void outputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                CommandMgr commandMgr = CommandMgr.instance();
                commandMgr.mQueue.Enqueue(e.Data);
            }
        }
        private void processExited(object sender, EventArgs e)
        {
            mProcess.Dispose();

            mStop = true;
        }

        Process mProcess;
        bool mStop;
    }
}

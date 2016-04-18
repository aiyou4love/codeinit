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
            if (null == e.Data) return;
            string value_ = e.Data.Trim();
            if ("" == value_) return;
            CommandMgr commandMgr = CommandMgr.instance();
            commandMgr.mQueue.Enqueue(value_);
            
        }
        private void processExited(object sender, EventArgs e)
        {
            CommandMgr commandMgr = CommandMgr.instance();
            commandMgr.mCommand = null;
            mProcess.Dispose();
            mStop = true;
        }

        Process mProcess;
        bool mStop;
    }
}

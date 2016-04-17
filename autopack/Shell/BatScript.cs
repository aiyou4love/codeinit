using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Web;

namespace autopack
{
    public class BatScript : IScript
    {
        public void runCommand(string nWorkDirectory, string nCommand)
        {
            if (!mStop) return;

            CommandMgr commandMgr = CommandMgr.instance();
            commandMgr.mQueue.Enqueue("$$$$命令执行开始$$$$");

            mStop = false;

            mOutput.Clear();

            string root_ = Path.GetPathRoot(nWorkDirectory);
            mPowerShell.AddScript(root_);
            mPowerShell.AddScript("cd " + nWorkDirectory);
            mPowerShell.AddScript(nCommand);

            mPowerShell.BeginInvoke<PSObject, PSObject>(null, mOutput);
        }
        public void runInit()
        {
            mStop = true;

            mPowerShell = PowerShell.Create();
            mPowerShell.InvocationStateChanged +=
                new EventHandler<PSInvocationStateChangedEventArgs>(invocationStateChanged);

            mOutput = new PSDataCollection<PSObject>();
            mOutput.DataAdded += new EventHandler<DataAddedEventArgs>(dataAdded);
        }
        public bool isStop()
        {
            return mStop;
        }
        void dataAdded(object sender, DataAddedEventArgs e)
        {
            Collection<PSObject> outputs_ = mOutput.ReadAll();

            foreach (PSObject i in outputs_)
            {
                string value_ = i.ToString();
                value_ = value_.Trim();
                if ("" == value_) continue;
                CommandMgr commandMgr = CommandMgr.instance();
                commandMgr.mQueue.Enqueue(value_);
            }
        }
        void invocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
        {
            if (e.InvocationStateInfo.State == PSInvocationState.Completed)
            {
                CommandMgr commandMgr = CommandMgr.instance();
                commandMgr.mQueue.Enqueue("$$$$命令执行完成$$$$");
                commandMgr.mQueue.Enqueue("$end$");
                
                mStop = true;
            }
        }

        PSDataCollection<PSObject> mOutput;

        PowerShell mPowerShell;
        bool mStop;
    }
}

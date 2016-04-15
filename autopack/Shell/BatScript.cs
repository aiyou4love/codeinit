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

            mQueue.Clear();
            mOutput.Clear();

            string root_ = Path.GetPathRoot(nWorkDirectory);
            mPowerShell.AddScript(root_);
            mPowerShell.AddScript("cd " + nWorkDirectory);
            mPowerShell.AddScript(nCommand);

            mAsyncResult = mPowerShell.BeginInvoke<PSObject, PSObject>(null, mOutput);

            mStop = false;
        }

        public void runInit()
        {
            mQueue = new Queue<string>();

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
                mQueue.Enqueue(i.ToString());
            }
        }

        void invocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
        {
            if (e.InvocationStateInfo.State == PSInvocationState.Completed)
            {
                mPowerShell.EndInvoke(mAsyncResult);
                mAsyncResult = null;
                mStop = true;
            }
        }

        Queue<string> mQueue;

        PSDataCollection<PSObject> mOutput;
        IAsyncResult mAsyncResult;

        PowerShell mPowerShell;
        bool mStop;
    }
}

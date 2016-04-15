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
            mProcess = new Process();
            mProcess.StartInfo.FileName = nCommand;
            mProcess.StartInfo.WorkingDirectory = nWorkDirectory;
            mProcess.StartInfo.UseShellExecute = false;
            mProcess.StartInfo.RedirectStandardOutput = true;
            mProcess.Start();

            mStreamReader = mProcess.StandardOutput;

            mThread = new Thread(outputInterpreter);
            mThread.IsBackground = true;
            mThread.Start();
        }

        public void runInit()
        {
            mQueue = new Queue<string>();
        }

        public bool isStop()
        {
            return false;
        }
        void outputInterpreter()
        {
            string line_;
            while ((line_ = mStreamReader.ReadLine()) != null)
            {
                mQueue.Enqueue(line_);
            }
        }

        Queue<string> mQueue;

        StreamReader mStreamReader;

        Process mProcess;
        Thread mThread;
    }
}

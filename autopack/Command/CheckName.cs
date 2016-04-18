using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace autopack
{
    [Serializable]
    public class CheckName : ICommand
    {
        private void runCheckDirectory(string nPath, string nKey)
        {
            string path_ = Path.GetFullPath(nPath);
            if (path_.EndsWith(@"\"))
            {
                path_ = Path.GetDirectoryName(path_);
            }
            mNames.Clear();
            DirectoryInfo directoryInfo_ = new DirectoryInfo(path_);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                runCheckName(Path.Combine(nKey, fileInfo_.Name));
            }
            foreach (DirectoryInfo suDirectoryInfo_ in directoryInfo_.GetDirectories())
            {
                runCheckDirectory(suDirectoryInfo_.FullName,
                    Path.Combine(nKey, suDirectoryInfo_.Name));
            }
        }

        private void runCheckName(string nName)
        {
            string name_ = Path.GetFileNameWithoutExtension(nName);
            name_ = name_.ToLower();
            if (!mNames.ContainsKey(name_))
            {
                mNames[name_] = nName;
                return;
            }
            CommandMgr commandMgr = CommandMgr.instance();
            string value_ = "checkName:{";
            value_ += nName;
            value_ += "}{";
            value_ += mNames[name_];
            value_ += "}";
            commandMgr.mQueue.Enqueue(value_);
        }

        public void runCommand()
        {
            if (!mStop) return;
            CommandMgr commandMgr = CommandMgr.instance();
            commandMgr.mQueue.Enqueue("$$$$命令执行开始$$$$");
            mStop = false;
            foreach (string i in mCheckNameDirectorys)
            {
                runCheckDirectory(Path.Combine(mSourceDirectory, i), i);
            }
            commandMgr.mQueue.Enqueue("$$$$命令执行完成$$$$");
            commandMgr.mQueue.Enqueue("$end$");
            mStop = true;
        }

        public bool isStop()
        {
            return mStop;
        }

        public List<string> mCheckNameDirectorys { get; set; }

        public string mSourceDirectory { get; set; }

        Dictionary<string, string> mNames = new Dictionary<string, string>();
        bool mStop = true;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace autopack
{
    public class CheckNameOnce
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

        public void runCheck()
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourceDirectory_ = stringDirectory_.mDirectorys[mSourceDirectory];

            foreach (string i in mCheckNameDirectorys)
            {
                runCheckDirectory(Path.Combine(sourceDirectory_, i), i);
            }
        }

        public List<string> mCheckNameDirectorys { get; set; }

        public string mSourceDirectory { get; set; }

        Dictionary<string, string> mNames = new Dictionary<string, string>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class CheckNameMgr : SerailizeJson
    {
        Dictionary<string, CheckName> mCheckNames;
        public void runInit(HttpServerUtilityBase nServer)
        {
            if (null != mCheckNames)
            {
                mCheckNames.Clear();
            }
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourcePath_ = stringDirectory_.mDirectorys["checkName"];
            string path_ = nServer.MapPath(sourcePath_);
            mCheckNames = Deserialize<Dictionary<string, CheckName>>(path_);
        }
        public void runCommand(string nCommand)
        {
            CommandMgr commandMgr_ = CommandMgr.instance();
            if (!mCheckNames.ContainsKey(nCommand))
            {
                string value_ = string.Format("{0} command not find", nCommand);
                commandMgr_.mQueue.Enqueue(value_);
                return;
            }
            CheckName checkName_ = mCheckNames[nCommand];
            commandMgr_.mCommand = checkName_;
            checkName_.runCommand();
            commandMgr_.mCommand = null;
        }
        public static CheckNameMgr instance()
        {
            return mInstance;
        }
        static CheckNameMgr mInstance = new CheckNameMgr();
    }
}

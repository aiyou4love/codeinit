using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class CommandMgr : SerailizeJson
    {
        public ICommand mCommand { get; set; }

        public Dictionary<int, Command> mCommands;

        public VersionNo mVersionNo;

        public Queue<string> mQueue = new Queue<string>();

        public void runInit(HttpServerUtilityBase nServer, int nType)
        {
            if (null != mCommands)
            {
                mCommands.Clear();
            }
            mVersionNo = null;

            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourcePath_ = stringDirectory_.mDirectorys["commands"];
            string path_ = nServer.MapPath(sourcePath_);
            mCommands = Deserialize<Dictionary<int, Command>>(path_);

            sourcePath_ = stringDirectory_.mDirectorys["versionNo"];
            path_ = nServer.MapPath(sourcePath_);
            mVersionNo = Deserialize<VersionNo>(path_);

            string directory_ = "~/version_";
            directory_ += mVersionNo.mApkNo;
            directory_ += mVersionNo.mUpdateNo;

            mCommandId = 0;

            mQueue.Clear();
        }
        public void runNext()
        {
            if ( (null != mCommand) 
                && (mCommand.isStop()) )
            {
                return;
            }
            mCommandId++;
            this.runCommand();
        }
        public void runPrev()
        {
            if (mCommand.isStop())
            {
                return;
            }
            this.runCommand();
        }
        void runCommand()
        {
            if (!mCommands.ContainsKey(mCommandId))
            {
                mQueue.Enqueue("工作流结束,请关闭窗口");
                return;
            }
            Command command_ = mCommands[mCommandId];
            if ("shell" == command_.mType)
            {
                ShellCommandMgr shellCommandMgr_ = ShellCommandMgr.instance();
                shellCommandMgr_.runCommand(command_.mName);
            }
            else if ("checkName" == command_.mType)
            {
                CheckNameMgr checkNameMgr_ = CheckNameMgr.instance();
                checkNameMgr_.runCommand(command_.mName);
            }
        }
        public static CommandMgr instance()
        {
            return mInstance;
        }

        static CommandMgr mInstance = new CommandMgr();

        int mCommandId;
    }
}

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

        public Queue<string> mQueue = new Queue<string>();

        public void runInit(HttpServerUtilityBase nServer)
        {
            if (null != mCommands)
            {
                mCommands.Clear();
            }
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourcePath_ = stringDirectory_.mDirectorys["commands"];
            string path_ = nServer.MapPath(sourcePath_);
            mCommands = Deserialize<Dictionary<int, Command>>(path_);

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
        }
        public static CommandMgr instance()
        {
            return mInstance;
        }

        static CommandMgr mInstance = new CommandMgr();

        int mCommandId;
    }
}

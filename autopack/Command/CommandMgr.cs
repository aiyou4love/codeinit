using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class CommandMgr : SerailizeJson
    {
        public Dictionary<int, Command> mCommands;

        public Queue<string> mQueue = new Queue<string>();

        public void runInit(HttpServerUtilityBase nServer)
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourcePath_ = stringDirectory_.mDirectorys["commands"];
            string path_ = nServer.MapPath(sourcePath_);
            mCommands = Deserialize<Dictionary<int, Command>>(path_);

            mQueue.Clear();
        }

        public static CommandMgr instance()
        {
            return mInstance;
        }

        static CommandMgr mInstance = new CommandMgr();
    }
}

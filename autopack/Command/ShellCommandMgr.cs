using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class ShellCommandMgr : SerailizeJson
    {
       Dictionary<string, ShellCommand> mShellCommands;
       public void runInit(HttpServerUtilityBase nServer)
       {
           if (null != mShellCommands)
           {
               mShellCommands.Clear();
           }
           StringDirectory stringDirectory_ = StringDirectory.instance();
           string sourcePath_ = stringDirectory_.mDirectorys["shellCommand"];
           string path_ = nServer.MapPath(sourcePath_);
           mShellCommands = Deserialize<Dictionary<string, ShellCommand>>(path_);
       }
       public void runCommand(string nCommand)
       {
           CommandMgr commandMgr_= CommandMgr.instance();
           if (!mShellCommands.ContainsKey(nCommand))
           {
               string value_ = string.Format("{0} command not find", nCommand);
               commandMgr_.mQueue.Enqueue(value_);
               return;
           }
           ShellCommand shellCommand_ = mShellCommands[nCommand];
           shellCommand_.runCommand();
       }
       public static ShellCommandMgr instance()
       {
           return mInstance;
       }
       static ShellCommandMgr mInstance = new ShellCommandMgr();
    }
}

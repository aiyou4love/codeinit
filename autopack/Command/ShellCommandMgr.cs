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
           StringDirectory stringDirectory_ = StringDirectory.instance();
           string sourcePath_ = stringDirectory_.mDirectorys["shellCommand"];
           string path_ = nServer.MapPath(sourcePath_);
           mShellCommands = Deserialize<Dictionary<string, ShellCommand>>(path_);
       }
       public static ShellCommandMgr instance()
       {
           return mInstance;
       }
       static ShellCommandMgr mInstance = new ShellCommandMgr();
    }
}

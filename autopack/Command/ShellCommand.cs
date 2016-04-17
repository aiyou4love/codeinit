using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    [Serializable]
    public class ShellCommand : ICommand
    {
        public string mDirectory { get; set; }
        public string mCommand { get; set; }
        public void runCommand()
        {
            Script script_ = Script.instance();
            script_.runCommand(mDirectory, mCommand);
        }
        public bool isStop()
        {
            Script script_ = Script.instance();
            return script_.isStop();
        }
    }
}

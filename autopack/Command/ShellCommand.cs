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
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourcePath_ = stringDirectory_.mDirectorys[mDirectory];

            Script script_ = Script.instance();
            script_.runCommand(sourcePath_, mCommand);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class BaseCommand
    {
        protected void runCommand(string nWorkDirectory, string nCommand)
        {
            Script script_ = Script.instance();
            script_.runCommand(nWorkDirectory, nCommand);
        }
    }
}

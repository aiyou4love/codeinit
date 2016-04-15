using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public interface IScript
    {
        void runCommand(string nWorkDirectory, string nCommand);

        void runInit();

        bool isStop();
    }
}

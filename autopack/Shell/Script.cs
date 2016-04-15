using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class Script
    {
        public void runCommand(string nWorkDirectory, string nCommand)
        {
            mScript.runCommand(nWorkDirectory, nCommand);
        }

        public void runInit()
        {
            OperatingSystem operatingSystem_ = Environment.OSVersion;
            PlatformID platformID_ = operatingSystem_.Platform;
            if ( (platformID_ == PlatformID.Unix)
                || (platformID_ == PlatformID.Unix))
            {
                mScript = new BatScript();
            }
            else
            {
                mScript = new ShScript();
            }
            mScript.runInit();
        }
        public static Script instance()
        {
            return mInstance;
        }

        public Script()
        {
            mInstance.runInit();
        }

        static Script mInstance = new Script();

        IScript mScript;
    }
}

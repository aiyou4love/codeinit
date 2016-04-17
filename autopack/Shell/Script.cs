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
        public bool isStop()
        {
            return mScript.isStop();
        }
        public void runInit()
        {
            if (null != mScript) return;
            OperatingSystem operatingSystem_ = Environment.OSVersion;
            PlatformID platformID_ = operatingSystem_.Platform;
            if ( (platformID_ == PlatformID.Unix)
                || (platformID_ == PlatformID.MacOSX))
            {
                mScript = new ShScript();
            }
            else
            {
                mScript = new BatScript();
            }
            mScript.runInit();
        }
        public static Script instance()
        {
            return mInstance;
        }

        static Script mInstance = new Script();

        IScript mScript;
    }
}

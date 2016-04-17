using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace autopack
{
    public class StringDirectory : SerailizeJson
    {
        public Dictionary<string, string> mDirectorys { get; set; }

        public void runInit(HttpServerUtilityBase nServer, string nConfigFile)
        {
            string path_ = nServer.MapPath(nConfigFile);
            mDirectorys = Deserialize<Dictionary<string, string>>(path_);
        }
        public static StringDirectory instance()
        {
            return mInstance;
        }
        static StringDirectory mInstance = new StringDirectory();
    }
}

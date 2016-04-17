using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class IfFile
    {
        public bool isExists()
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string directory_ = stringDirectory_.mDirectorys[mDirectory];
            foreach (string i in mIfDirectorys)
            {
                if (Directory.Exists(Path.Combine(directory_, i)))
                {
                    return true;
                }
            }
            foreach (string i in mIfFiles)
            {
                if (File.Exists(Path.Combine(directory_, i)))
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> mIfDirectorys { get; set; }

        public List<string> mIfFiles { get; set; }

        public string mDirectory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class Md5Info
    {
        public long mLength { get; set; }

        public string mMD5 { get; set; }

        public string mFileName { get; set; }
    }
}

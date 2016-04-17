using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class Header
    {
        public string mLanguage { get; set; }

        public string mPackage { get; set; }

        public string mType { get; set; }

        public string mNo { get; set; }

        public string mFile { get; set; }
    }
}

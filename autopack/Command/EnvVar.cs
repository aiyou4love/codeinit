using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    [Serializable]
    public class EnvVar
    {
        public string mName { get; set; }
        public string mValue { get; set; }
        public string mType { get; set; }
        public bool mAdd { get; set; }
    }
}

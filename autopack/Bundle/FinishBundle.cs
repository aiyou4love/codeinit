using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class FinishBundle : SerailizeJson
    {
        public void runCommand()
        {
            foreach (CopyOnce i in mCopyOnces)
            {
                i.runCopy();
            }
            foreach (DeleteOnce i in mDeleteOnces)
            {
                i.runDelete();
            }
            foreach (ZipOnce i in mZipOnces)
            {
                i.runZip();
            }
        }

        public List<CopyOnce> mCopyOnces { get; set; }

        public List<DeleteOnce> mDeleteOnces { get; set; }

        public List<ZipOnce> mZipOnces { get; set; }
    }
}

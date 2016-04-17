using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class CopyOnce
    {
        public void runModify(BundleInfo nBundleInfo, string nDirectory)
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourceDirectory_ = stringDirectory_.mDirectorys[mSourceDirectory];

            foreach (CopyDirectory i in mCopyDirectorys)
            {
                i.runModify(sourceDirectory_, nBundleInfo, nDirectory);
            }
            foreach (CopyFile i in mCopyFiles)
            {
                i.runModify(sourceDirectory_, nBundleInfo, nDirectory);
            }
        }

        public void runMd5(BundleInfo nBundleInfo)
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourceDirectory_ = stringDirectory_.mDirectorys[mSourceDirectory];

            foreach (CopyDirectory i in mCopyDirectorys)
            {
                i.runMd5(sourceDirectory_, nBundleInfo);
            }
            foreach (CopyFile i in mCopyFiles)
            {
                i.runMd5(sourceDirectory_, nBundleInfo);
            }
        }

        public void runCopy()
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string destDirectory_ = stringDirectory_.mDirectorys[mDestDirectory];
            string sourceDirectory_ = stringDirectory_.mDirectorys[mSourceDirectory];

            foreach (CopyDirectory i in mCopyDirectorys)
            {
                i.runCopy(sourceDirectory_, destDirectory_);
            }
            foreach (CopyFile i in mCopyFiles)
            {
                i.runCopy(sourceDirectory_, destDirectory_);
            }
        }

        public List<CopyDirectory> mCopyDirectorys { get; set; }

        public List<CopyFile> mCopyFiles { get; set; }

        public string mDestDirectory { get; set; }

        public string mSourceDirectory { get; set; }
    }
}

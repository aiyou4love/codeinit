using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class ZipIfOnce
    {
        void runZip(ZipFile nZipFile)
        {
            foreach (SZipDirectory i in mZipDirectorys)
            {
                i.runZip(nZipFile);
            }
            foreach (string i in mZipFiles)
            {
                if (File.Exists(i))
                {
                    nZipFile.Add(i);
                }
            }
        }

        public void runZip()
        {
            bool isExists = false;
            foreach (IfFile i in mIfFiles)
            {
                if (i.isExists())
                {
                    isExists = true;
                    break;
                }
            }
            if (!isExists) return;

            StringDirectory stringDirectory_ = StringDirectory.instance();
            string destDirectory_ = stringDirectory_.mDirectorys[mDestDirectory];

            string zipFile_ = Path.Combine(destDirectory_, mZipFile);
            if (File.Exists(zipFile_))
            {
                ZipFile zipFile = new ZipFile(zipFile_);
                zipFile.BeginUpdate();
                runZip(zipFile);
                zipFile.CommitUpdate();
                zipFile.Close();
            }
            else
            {
                ZipFile zipFile = ZipFile.Create(zipFile_);
                zipFile.BeginUpdate();
                runZip(zipFile);
                zipFile.CommitUpdate();
                zipFile.Close();
            }
        }

        public List<IfFile> mIfFiles { get; set; }

        public List<SZipDirectory> mZipDirectorys { get; set; }

        public List<string> mZipFiles { get; set; }

        public string mZipFile { get; set; }

        public string mDestDirectory { get; set; }
    }
}

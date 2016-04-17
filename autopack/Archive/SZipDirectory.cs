using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autopack
{
    [Serializable]
    public class SZipDirectory
    {
        void zipDirectory(string nPath, ZipFile nZipFile)
        {
            nZipFile.AddDirectory(nPath);
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nPath);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                if (mWithouts.Contains(fileInfo_.Extension))
                {
                    continue;
                }
                nZipFile.Add(Path.Combine(nPath, fileInfo_.Name));
            }
            foreach (DirectoryInfo suDirectoryInfo_ in directoryInfo_.GetDirectories())
            {
                zipDirectory(Path.Combine(nPath, suDirectoryInfo_.Name), nZipFile);
            }
        }

        public void runZip(ZipFile nZipFile)
        {
            if (Directory.Exists(mSourceDirectory))
            {
                zipDirectory(mSourceDirectory, nZipFile);
            }
        }

        public string mSourceDirectory { get; set; }

        public HashSet<string> mWithouts { get; set; }
    }
}

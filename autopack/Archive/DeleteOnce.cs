using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class DeleteOnce
    {
        void runDelete(string nDirectory)
        {
            if (!Directory.Exists(nDirectory))
            {
                return;
            }
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nDirectory);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                fileInfo_.Attributes = fileInfo_.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                fileInfo_.Delete();
            }
            foreach (DirectoryInfo suDirectoryInfo_ in directoryInfo_.GetDirectories())
            {
                runDelete(suDirectoryInfo_.FullName);
            }
            directoryInfo_.Attributes = directoryInfo_.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            directoryInfo_.Delete();
        }

        public void runDelete()
        {
            StringDirectory stringDirectory_ = StringDirectory.instance();
            string directory_ = stringDirectory_.mDirectorys[mDirectory];

            foreach (string i in mDeleteDirectorys)
            {
                runDelete(Path.Combine(directory_, i));
            }
            foreach (string i in mDeleteFiles)
            {
                if (File.Exists(Path.Combine(directory_, i)))
                {
                    File.Delete(Path.Combine(directory_, i));
                }
            }
        }

        public List<string> mDeleteDirectorys { get; set; }

        public List<string> mDeleteFiles { get; set; }

        public string mDirectory { get; set; }
    }
}

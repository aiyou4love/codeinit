using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace autopack
{
    [Serializable]
    public class BuildPackage : SerailizeJson
    {
        //void runDelete(string nDirectory)
        //{
        //    if (!Directory.Exists(nDirectory))
        //    {
        //        return;
        //    }
        //    DirectoryInfo directoryInfo_ = new DirectoryInfo(nDirectory);
        //    foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
        //    {
        //        fileInfo_.Attributes = fileInfo_.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
        //        fileInfo_.Delete();
        //    }
        //    foreach (DirectoryInfo suDirectoryInfo_ in directoryInfo_.GetDirectories())
        //    {
        //        runDelete(suDirectoryInfo_.FullName);
        //    }
        //    directoryInfo_.Attributes = directoryInfo_.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
        //    directoryInfo_.Delete();
        //}

        //void runClear(string nDirectory, Bundle nBundle)
        //{
        //    string directory_ = nBundle.mDirectorys[nDirectory];
        //    if (!Directory.Exists(directory_))
        //    {
        //        return;
        //    }
        //    DirectoryInfo directoryInfo_ = new DirectoryInfo(directory_);
        //    foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
        //    {
        //        fileInfo_.Attributes = fileInfo_.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
        //        fileInfo_.Delete();
        //    }
        //    foreach (DirectoryInfo suDirectoryInfo_ in directoryInfo_.GetDirectories())
        //    {
        //        runDelete(suDirectoryInfo_.FullName);
        //    }
        //}

        //public void runCommand(Bundle nBundle)
        //{
        //    foreach (string i in mClearFiles)
        //    {
        //        string directory_ = nBundle.mDirectorys[i];
        //        if (File.Exists(directory_))
        //        {
        //            FileInfo fileInfo_ = new FileInfo(directory_);
        //            fileInfo_.Attributes = fileInfo_.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
        //            fileInfo_.Delete();
        //        }
        //    }

        //    foreach (string i in mClearDirectorys)
        //    {
        //        runClear(i, nBundle);
        //    }

        //    foreach (CopyOnce i in mCopyOnces)
        //    {
        //        i.runCopy(nBundle);
        //    }
        //}
        public List<string> mClearFiles { get; set; }

        public List<string> mClearDirectorys { get; set; }

        public List<CopyOnce> mCopyOnces { get; set; }
    }
}

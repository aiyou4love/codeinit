using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace autopack
{
    public class AutoPack : SerailizeJson
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

        public VersionNo mVersionNo;

        public string mDirectory;

        public void runInit(HttpServerUtilityBase nServer, int nType)
        {
            mVersionNo = null;
            mDirectory = null;

            StringDirectory stringDirectory_ = StringDirectory.instance();
            string sourcePath_ = stringDirectory_.mDirectorys["versionNo"];
            string path_ = nServer.MapPath(sourcePath_);
            mVersionNo = Deserialize<VersionNo>(path_);

            string directory_ = "~/version_";
            if (0 == (nType % 2))
            {
                directory_ += mVersionNo.mApkNo;
                directory_ += "_";
                directory_ += mVersionNo.mUpdateNo + 1;
            }
            else
            {
                directory_ += mVersionNo.mApkNo + 1;
                directory_ += "_";
                directory_ += mVersionNo.mUpdateNo;
            }
            mDirectory = nServer.MapPath(directory_);

            this.runDelete(mDirectory);

            Directory.CreateDirectory(mDirectory);

        }
    }
}

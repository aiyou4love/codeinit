using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace autopack
{
    [Serializable]
    public class CopyFile
    {
        public void runModify(string nSourceDirectory, BundleInfo nBundleInfo, string nDirectory)
        {
            string source_ = Path.Combine(nSourceDirectory, mSourceFile);
            string dest_ = Path.Combine(nDirectory, mDestFile);
            FileInfo fileInfo_ = new FileInfo(source_);
            if (!nBundleInfo.mMd5Infos.ContainsKey(mSourceFile))
            {
                File.Copy(fileInfo_.FullName, dest_);
                return;
            }
            Md5Info md5Info_ = nBundleInfo.mMd5Infos[mSourceFile];
            if (md5Info_.mLength != fileInfo_.Length)
            {
                File.Copy(fileInfo_.FullName, dest_);
                return;
            }
            string fileMd5_ = this.genFileMD5(fileInfo_);
            if (md5Info_.mMD5 != fileMd5_)
            {
                File.Copy(fileInfo_.FullName, dest_);
            }
        }

        string genFileMD5(FileInfo nFileInfo)
        {
            FileStream fileStream_ = new FileStream(nFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] byteHash_ = mMD5.ComputeHash(fileStream_);
            fileStream_.Close();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < byteHash_.Length; i++)
            {
                stringBuilder.Append(byteHash_[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public void runMd5(string nSourceDirectory, BundleInfo nBundleInfo)
        {
            string path_ = Path.Combine(nSourceDirectory, mSourceFile);
            FileInfo fileInfo_ = new FileInfo(path_);
            Md5Info md5Info_ = new Md5Info();
            md5Info_.mMD5 = this.genFileMD5(fileInfo_);
            md5Info_.mLength = fileInfo_.Length;
            md5Info_.mFileName = fileInfo_.Name;
            nBundleInfo.mMd5Infos[mSourceFile] = md5Info_;
        }

        public void runCopy(string nSourceDirectory, string nDestDirectory)
        {
            File.Copy(Path.Combine(nSourceDirectory, mSourceFile),
                Path.Combine(nDestDirectory, mDestFile));
        }

        public string mSourceFile { get; set; }

        public string mDestFile { get; set; }

        MD5 mMD5 = new MD5CryptoServiceProvider();
    }
}

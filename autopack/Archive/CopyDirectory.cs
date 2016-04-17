using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace autopack
{
    [Serializable]
    public class CopyDirectory
    {
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

        bool isFileModify(FileInfo nFileInfo, string nKey, BundleInfo nBundleInfo)
        {
            string key_ = Path.Combine(nKey, nFileInfo.Name);
            if (!nBundleInfo.mMd5Infos.ContainsKey(key_))
            {
                return true;
            }
            Md5Info md5Info_ = nBundleInfo.mMd5Infos[key_];
            if (md5Info_.mLength != nFileInfo.Length)
            {
                return true;
            }
            string fileMd5_ = this.genFileMD5(nFileInfo);
            if (md5Info_.mMD5 != fileMd5_)
            {
                return true;
            }
            return false;
        }

        bool isDirectoryModify(string nPath, string nKey, BundleInfo nBundleInfo)
        {
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nPath);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                if (mWithouts.Contains(fileInfo_.Extension))
                {
                    continue;
                }
                if( isFileModify(fileInfo_, nKey, nBundleInfo) )
                {
                    return true;
                }
            }
            foreach (DirectoryInfo suDirectory_ in directoryInfo_.GetDirectories())
            {
                string path_ = Path.Combine(nKey, suDirectory_.Name);
                if ( isDirectoryModify(suDirectory_.FullName, path_, nBundleInfo) )
                {
                    return true;
                }
            }
            return false;
        }

        void runDirectoryModify(string nPath, string nkey, string nDirectory, BundleInfo nBundleInfo)
        {
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nPath);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                if (mWithouts.Contains(fileInfo_.Extension))
                {
                    continue;
                }
                if (isFileModify(fileInfo_, nkey, nBundleInfo))
                {
                    string path_ = Path.Combine(nDirectory, fileInfo_.Name.ToLower());
                    File.Copy(fileInfo_.FullName, path_);
                }
            }
            foreach (DirectoryInfo suDirectory_ in directoryInfo_.GetDirectories())
            {
                string key_ = Path.Combine(nkey, suDirectory_.Name);
                if (this.isDirectoryModify(suDirectory_.FullName, key_, nBundleInfo))
                {
                    string dest_ = Path.Combine(nDirectory, suDirectory_.Name.ToLower());
                    Directory.CreateDirectory(dest_);
                    runDirectoryModify(suDirectory_.FullName, key_, dest_, nBundleInfo);
                }
            }
        }

        public void runModify(string nSourceDirectory, BundleInfo nBundleInfo, string nDirectory)
        {
            string path_ = Path.Combine(nSourceDirectory, mSourceDirectory);
            if (this.isDirectoryModify(path_, mSourceDirectory, nBundleInfo))
            {
                string dest_ = Path.Combine(nDirectory, mDestDirectory);
                Directory.CreateDirectory(dest_);
                runDirectoryModify(path_, mSourceDirectory, dest_, nBundleInfo);
            }
        }

        void runFileMd5(FileInfo nFileInfo, string nKey, BundleInfo nBundleInfo)
        {
            FileStream fileStream_ = new FileStream(nFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] byteHash_ = mMD5.ComputeHash(fileStream_);
            fileStream_.Close();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < byteHash_.Length; i++)
            {
                stringBuilder.Append(byteHash_[i].ToString("x2"));
            }
            Md5Info md5Info_ = new Md5Info();
            md5Info_.mMD5 = stringBuilder.ToString();
            md5Info_.mLength = nFileInfo.Length;
            md5Info_.mFileName = nFileInfo.Name;

            string key_ = Path.Combine(nKey, nFileInfo.Name);
            nBundleInfo.mMd5Infos[key_] = md5Info_;
        }

        void runDirectoryMd5(string nPath, string nKey, BundleInfo nBundleInfo)
        {
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nPath);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                if (mWithouts.Contains(fileInfo_.Extension))
                {
                    continue;
                }
                runFileMd5(fileInfo_, nKey, nBundleInfo);
            }
            foreach (DirectoryInfo suDirectory_ in directoryInfo_.GetDirectories())
            {
                string path_ = Path.Combine(nKey, suDirectory_.Name);
                runDirectoryMd5(suDirectory_.FullName, path_, nBundleInfo);
            }
        }

        public void runMd5(string nSourceDirectory, BundleInfo nBundleInfo)
        {
            string path_ = Path.Combine(nSourceDirectory, mSourceDirectory);
            runDirectoryMd5(path_, mSourceDirectory, nBundleInfo);
        }

        void copyDirectory(string nSourceDirectory, string nDestDirectory)
        {
            if (!Directory.Exists(nDestDirectory))
            {
                Directory.CreateDirectory(nDestDirectory);
            }
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nSourceDirectory);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                if (mWithouts.Contains(fileInfo_.Extension))
                {
                    continue;
                }
                string path_ = Path.Combine(nDestDirectory, fileInfo_.Name.ToLower());
                File.Copy(fileInfo_.FullName, path_);
            }
            foreach (DirectoryInfo suDirectory_ in directoryInfo_.GetDirectories())
            {
                string path_ = Path.Combine(nDestDirectory, suDirectory_.Name.ToLower());
                copyDirectory(suDirectory_.FullName, path_);
            }
        }

        public void runCopy(string nSourceDirectory, string nDestDirectory)
        {
            copyDirectory(Path.Combine(nSourceDirectory, mSourceDirectory),
                Path.Combine(nDestDirectory, mDestDirectory));
        }

        public string mSourceDirectory { get; set; }

        public string mDestDirectory { get; set; }

        public HashSet<string> mWithouts { get; set; }

        MD5 mMD5 = new MD5CryptoServiceProvider();
    }
}

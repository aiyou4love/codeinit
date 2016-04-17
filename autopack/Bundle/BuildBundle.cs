using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace autopack
{
    [Serializable]
    public class BuildBundle : SerailizeXml
    {
        public void runHeader(Bundle nBundle, VersionNo nVersionNo)
        {
            foreach (Header i in mHeaders)
            {
                this.runHeader(nBundle, nVersionNo, i);
            }
        }

        void runHeader(Bundle nBundle, VersionNo nVersionNo, Header nHeader)
        {
            string directory_ = nBundle.mDirectorys["header"];
            directory_ = Path.Combine(directory_, nHeader.mFile);
            if (nHeader.mLanguage == "java")
            {
                nVersionNo.mUpdateNo = 0;
                string value_ = nHeader.mPackage + "\r\npublic class APKVERSION {\r\n  public static final int NO = ";
                value_ += nVersionNo.mApkNo;
                value_ += ";\r\n  public static final int P = ";
                value_ += nHeader.mNo;
                value_ += ";\r\n  public static final int V = ";
                value_ += nHeader.mType;
                value_ += ";\r\n}\r\n";
                FileInfo fileInfo_ = new FileInfo(directory_);
                StreamWriter streamWriter_ = fileInfo_.CreateText();
                streamWriter_.Write(value_);
                streamWriter_.Close();
            }
            else if (nHeader.mLanguage == "objective-c")
            {
                nVersionNo.mUpdateNo = 0;
                string value_ = "\r\n#define APKMIN ";
                value_ += nVersionNo.mApkNo;
                value_ += "\r\n#define P ";
                value_ += nHeader.mNo;
                value_ += "\r\n#define V ";
                value_ += nHeader.mType;
                value_ += "\r\n";
                FileInfo fileInfo_ = new FileInfo(directory_);
                StreamWriter streamWriter_ = fileInfo_.CreateText();
                streamWriter_.Write(value_);
                streamWriter_.Close();
            }
            else if (nHeader.mLanguage == "c++")
            {
                nVersionNo.mUpdateNo = 0;
                string value_ = "#pragma once\r\n\r\n#define APKMIN ";
                value_ += nVersionNo.mApkNo;
                value_ += "\r\n#define PACKAGENO ";
                value_ += nHeader.mNo;
                value_ += "\r\n#define PACKAGETYPE ";
                value_ += nHeader.mType;
                value_ += "\r\n";
                FileInfo fileInfo_ = new FileInfo(directory_);
                StreamWriter streamWriter_ = fileInfo_.CreateText();
                streamWriter_.Write(value_);
                streamWriter_.Close();
            }
        }

        void runMd5(Bundle nBundle, VersionNo nVersionNo)
        {
            BundleInfo bundleInfo_ = new BundleInfo();
            bundleInfo_.mMd5Infos = new SerializableDictionary<string, Md5Info>();
            foreach (CopyOnce i in mCopyOnces)
            {
                i.runMd5(nBundle, bundleInfo_);
            }
            string md5File_ = nBundle.mDirectorys["md5File"];
            md5File_ += "_";
            md5File_ += nVersionNo.mApkNo;
            md5File_ += "_";
            md5File_ += nVersionNo.mUpdateNo;
            md5File_ += ".xml";
            Serialize<BundleInfo>(md5File_, bundleInfo_);
        }

        void runBuild(Bundle nBundle, VersionNo nVersionNo)
        {
            foreach (CopyOnce i in mCopyOnces)
            {
                i.runCopy(nBundle);
            }
            this.runMd5(nBundle, nVersionNo);
            this.runHeader(nBundle, nVersionNo);
        }

        void runApkModify(Bundle nBundle, VersionNo nVersionNo)
        {
            string apk_ = nBundle.mDirectorys["modify"];
            string apkMd5_ = nBundle.mDirectorys["md5File"];
            apkMd5_ += "_";
            apkMd5_ += nVersionNo.mApkNo;
            apkMd5_ += "_1.xml";
            BundleInfo apkBundle_ = Deserialize<BundleInfo>(apkMd5_);
            foreach (CopyOnce i in mCopyOnces)
            {
                i.runModify(nBundle, apkBundle_, apk_);
            }
        }

        void runUpdateModify(Bundle nBundle, VersionNo nVersionNo)
        {
            string update_ = nBundle.mDirectorys["update"];
            string updateMd5_ = nBundle.mDirectorys["md5File"];
            updateMd5_ += "_";
            updateMd5_ += nVersionNo.mApkNo;
            updateMd5_ += "_";
            updateMd5_ += (nVersionNo.mUpdateNo - 1);
            updateMd5_ += ".xml";

            BundleInfo apkBundle_ = Deserialize<BundleInfo>(updateMd5_);
            foreach (CopyOnce i in mCopyOnces)
            {
                i.runModify(nBundle, apkBundle_, update_);
            }
        }

        void runModify(Bundle nBundle, VersionNo nVersionNo)
        {
            this.runApkModify(nBundle, nVersionNo);
            this.runUpdateModify(nBundle, nVersionNo);
            this.runMd5(nBundle, nVersionNo);
        }

        public void runCommand(string nCommand, Bundle nBundle)
        {
            string versionNoXml_ = nBundle.mDirectorys["versionNo"];
            VersionNo versionNo_ = Deserialize<VersionNo>(versionNoXml_);

            foreach (CheckNameOnce i in mCheckNameOnces)
            {
                i.runCheck(nBundle);
            }
            if ("build" == nCommand)
            {
                this.runBuild(nBundle, versionNo_);
            }
            else if ("modify" == nCommand)
            {
                this.runModify(nBundle, versionNo_);
            }
        }

        public List<CheckNameOnce> mCheckNameOnces { get; set; }

        public List<CopyOnce> mCopyOnces { get; set; }

        public List<Header> mHeaders { get; set; }
    }
}

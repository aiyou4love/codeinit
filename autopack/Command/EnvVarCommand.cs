using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autopack
{
    public class EnvVarCommand : ICommand
    {
        public List<EnvVar> mEnvVars { get; set; }

        public void runEnvVar(EnvVar nEnvVar)
        {
            string value_ = Environment.GetEnvironmentVariable(nEnvVar.mName, EnvironmentVariableTarget.User);
            if ("system" == nEnvVar.mType)
            {
                value_ = Environment.GetEnvironmentVariable(nEnvVar.mName);
            }
            if (nEnvVar.mAdd)
            {
                value_ = value_.Trim(new char[]{';'});
                value_ += ";";
                value_ += nEnvVar.mValue;
            }
            else
            {
                value_ = nEnvVar.mValue;
            }
            if ("system" == nEnvVar.mType)
            {
                Environment.SetEnvironmentVariable(nEnvVar.mName, value_);
            }
            else
            {
                Environment.SetEnvironmentVariable(nEnvVar.mName, value_, EnvironmentVariableTarget.User);
            }

        }

        public void runCommand()
        {
            foreach (EnvVar i in mEnvVars)
            {
                this.runEnvVar(i);
            }
        }

        public bool isStop()
        {
            return true;
        }
    }
}

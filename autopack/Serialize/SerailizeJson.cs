using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Web;
using System.Web.Configuration;

namespace autopack
{
    public class SerailizeJson
    {
        protected void Serialize<T>(string nFileName, T nT)
        {
            StreamWriter streamWriter_ = new StreamWriter(nFileName);
            JsonWriter jsonWriter_ = new JsonTextWriter(streamWriter_);
            JsonSerializer jsonSerializer_ = new JsonSerializer();
            jsonSerializer_.Serialize(jsonWriter_, nT);
            jsonWriter_.Close();
            streamWriter_.Close();
        }

        protected T Deserialize<T>(string nFileName)
        {
            StreamReader streamReader_ = new StreamReader(nFileName);
            JsonReader jsonReader_ = new JsonTextReader(streamReader_);
            JsonSerializer jsonSerializer_ = new JsonSerializer();
            T result_ = jsonSerializer_.Deserialize<T>(jsonReader_);
            jsonReader_.Close();
            streamReader_.Close();
            return result_;
        }
    }
}

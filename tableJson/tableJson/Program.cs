using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;
using System.Data;
using Newtonsoft.Json;

namespace tableJson
{
    class Program
    {
        static void Serialize<T>(string nFileName, T nT)
        {
            StreamWriter streamWriter_ = new StreamWriter(nFileName);
            JsonWriter jsonWriter_ = new JsonTextWriter(streamWriter_);
            JsonSerializer jsonSerializer_ = new JsonSerializer();
            jsonSerializer_.Serialize(jsonWriter_, nT);
            jsonWriter_.Close();
            streamWriter_.Close();
        }
        static List<Dictionary<string, string>> mValues = new List<Dictionary<string, string>>();
        static void runDataSet(DataTable nDataTable, string nDestDirectory)
        {
            mValues.Clear();
			
            if (nDataTable.Columns.Count <= 0) return;
            if (nDataTable.Rows.Count <= 3) return;
            DataRow types_ = nDataTable.Rows[0];
            for (int i = 2; i < nDataTable.Rows.Count; i++)
            {
                Dictionary<string, string> row_ = new Dictionary<string, string>();
                DataRow values_ = nDataTable.Rows[i];
                foreach (DataColumn j in nDataTable.Columns)
                {
                    string type_ = (string)types_[j];
                    if ("null" == type_) continue;
                    object value_ = values_[j];
                    if (value_.GetType() == typeof(double)) {
                        double number_ = (double)value_;
                        if ((int)number_ == number_) {
                            value_ = (int)number_;
                        }
                    }
                    row_[j.ToString()] = value_.ToString();
                }
                mValues.Add(row_);
            }
            string path_ = Path.Combine(nDestDirectory, nDataTable.TableName);
            path_ += ".json";
            Serialize<List<Dictionary<string, string>>>(path_, mValues);
        }

        static void runDirectory(string nDirectory, string nDestDirectory)
        {
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nDirectory);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                runFile(fileInfo_.FullName, nDestDirectory);
            }
            foreach (DirectoryInfo suDirectory_ in directoryInfo_.GetDirectories())
            {
                string path_ = Path.Combine(nDirectory, suDirectory_.Name);
                runDirectory(path_, nDestDirectory);
            }
        }
        static void runFile(string nFile, string nDestDirectory)
        {
            FileStream excelFile_ = File.Open(nFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader_ = ExcelReaderFactory.CreateOpenXmlReader(excelFile_);
            excelReader_.IsFirstRowAsColumnNames = true;
            DataSet excelData_ = excelReader_.AsDataSet();
            for (int i = 0; i < excelData_.Tables.Count; ++i)
            {
                runDataSet(excelData_.Tables[i], nDestDirectory);
            }
            excelReader_.Close();
            excelFile_.Close();
        }
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("args < 2");
                Console.ReadKey(true);
                return;
            }
            runDirectory(args[0], args[1]);
        }
    }
}

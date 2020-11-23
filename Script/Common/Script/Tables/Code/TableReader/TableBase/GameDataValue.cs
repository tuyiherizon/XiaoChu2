using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GameDataValueRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<int> Values { get; set; }
        public GameDataValueRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Values = new List<int>();
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            foreach (var testTableItem in Values)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class GameDataValue : TableFileBase
    {
        public Dictionary<string, GameDataValueRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GameDataValueRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GameDataValue" + ": " + id, ex);
            }
        }

        public GameDataValue(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GameDataValueRecord>();
            if(isPath)
            {
                string[] lines = File.ReadAllLines(pathOrContent, Encoding.Default);
                lines[0] = lines[0].Replace("\r\n", "\n");
                ParserTableStr(string.Join("\n", lines));
            }
            else
            {
                ParserTableStr(pathOrContent.Replace("\r\n", "\n"));
            }
        }

        private void ParserTableStr(string content)
        {
            StringReader rdr = new StringReader(content);
            using (var reader = new CsvReader(rdr))
            {
                HeaderRecord header = reader.ReadHeaderRecord();
                while (reader.HasMoreRecords)
                {
                    DataRecord data = reader.ReadDataRecord();
                    if (data[0].StartsWith("#"))
                        continue;

                    GameDataValueRecord record = new GameDataValueRecord(data);
                    Records.Add(record.Id, record);
                }
            }
        }

        public void CoverTableContent()
        {
            foreach (var pair in Records)
            {
                pair.Value.Name = TableReadBase.ParseString(pair.Value.ValueStr[1]);
                pair.Value.Desc = TableReadBase.ParseString(pair.Value.ValueStr[2]);
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[3]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[4]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[5]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[6]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
            }
        }
    }

}
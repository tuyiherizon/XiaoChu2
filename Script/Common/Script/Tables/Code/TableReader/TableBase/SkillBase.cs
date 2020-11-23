using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class SkillBaseRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public int CD { get; set; }
        public int PreCD { get; set; }
        public string Script { get; set; }
        public List<int> Param { get; set; }
        public SkillBaseRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Param = new List<int>();
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(CD));
            recordStrList.Add(TableWriteBase.GetWriteStr(PreCD));
            recordStrList.Add(TableWriteBase.GetWriteStr(Script));
            foreach (var testTableItem in Param)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class SkillBase : TableFileBase
    {
        public Dictionary<string, SkillBaseRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public SkillBaseRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("SkillBase" + ": " + id, ex);
            }
        }

        public SkillBase(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, SkillBaseRecord>();
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

                    SkillBaseRecord record = new SkillBaseRecord(data);
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
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.CD = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.PreCD = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.Script = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
                pair.Value.Param.Add(TableReadBase.ParseInt(pair.Value.ValueStr[13]));
            }
        }
    }

}
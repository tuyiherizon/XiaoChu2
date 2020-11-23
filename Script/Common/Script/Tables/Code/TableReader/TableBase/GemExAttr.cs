using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemExAttrRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string NameDict { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public string Script { get; set; }
        public List<string> Params { get; set; }
        public GemExAttrRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Params = new List<string>();
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(NameDict));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(Level));
            recordStrList.Add(TableWriteBase.GetWriteStr(Script));
            foreach (var testTableItem in Params)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class GemExAttr : TableFileBase
    {
        public Dictionary<string, GemExAttrRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GemExAttrRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GemExAttr" + ": " + id, ex);
            }
        }

        public GemExAttr(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GemExAttrRecord>();
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

                    GemExAttrRecord record = new GemExAttrRecord(data);
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
                pair.Value.NameDict = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.Level = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.Script = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.Params.Add(TableReadBase.ParseString(pair.Value.ValueStr[7]));
                pair.Value.Params.Add(TableReadBase.ParseString(pair.Value.ValueStr[8]));
                pair.Value.Params.Add(TableReadBase.ParseString(pair.Value.ValueStr[9]));
            }
        }
    }

}
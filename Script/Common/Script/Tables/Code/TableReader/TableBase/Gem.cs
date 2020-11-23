using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string NameDict { get; set; }
        public ITEM_QUALITY Quality { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public List<int> Attrs { get; set; }
        public GemRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Attrs = new List<int>();
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(NameDict));
            recordStrList.Add(((int)Quality).ToString());
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(Level));
            foreach (var testTableItem in Attrs)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class Gem : TableFileBase
    {
        public Dictionary<string, GemRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GemRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("Gem" + ": " + id, ex);
            }
        }

        public Gem(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GemRecord>();
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

                    GemRecord record = new GemRecord(data);
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
                pair.Value.Quality =  (ITEM_QUALITY)TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.Level = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                pair.Value.Attrs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.Attrs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.Attrs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.Attrs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.Attrs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.Attrs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
            }
        }
    }

}
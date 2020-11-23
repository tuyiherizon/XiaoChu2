using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class MonsterBaseRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string Model { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int HP { get; set; }
        public ELEMENT_TYPE ElementType { get; set; }
        public List<SkillBaseRecord> Skills { get; set; }
        public MonsterBaseRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Skills = new List<SkillBaseRecord>();
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(Model));
            recordStrList.Add(TableWriteBase.GetWriteStr(Attack));
            recordStrList.Add(TableWriteBase.GetWriteStr(Defence));
            recordStrList.Add(TableWriteBase.GetWriteStr(HP));
            recordStrList.Add(((int)ElementType).ToString());
            foreach (var testTableItem in Skills)
            {
                if (testTableItem != null)
                {
                    recordStrList.Add(testTableItem.Id);
                }
                else
                {
                    recordStrList.Add("");
                }
            }

            return recordStrList.ToArray();
        }
    }

    public partial class MonsterBase : TableFileBase
    {
        public Dictionary<string, MonsterBaseRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public MonsterBaseRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("MonsterBase" + ": " + id, ex);
            }
        }

        public MonsterBase(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, MonsterBaseRecord>();
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

                    MonsterBaseRecord record = new MonsterBaseRecord(data);
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
                pair.Value.Model = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.Attack = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.Defence = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                pair.Value.HP = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.ElementType =  (ELEMENT_TYPE)TableReadBase.ParseInt(pair.Value.ValueStr[8]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[9]))
                {
                    pair.Value.Skills.Add( TableReader.SkillBase.GetRecord(pair.Value.ValueStr[9]));                }
                else
                {
                    pair.Value.Skills.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[10]))
                {
                    pair.Value.Skills.Add( TableReader.SkillBase.GetRecord(pair.Value.ValueStr[10]));                }
                else
                {
                    pair.Value.Skills.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[11]))
                {
                    pair.Value.Skills.Add( TableReader.SkillBase.GetRecord(pair.Value.ValueStr[11]));                }
                else
                {
                    pair.Value.Skills.Add(null);
                }
            }
        }
    }

}
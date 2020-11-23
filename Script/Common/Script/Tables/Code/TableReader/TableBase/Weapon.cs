using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class WeaponRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string SkillName { get; set; }
        public string SkillDesc { get; set; }
        public string SkillIcon { get; set; }
        public string NameDict { get; set; }
        public string Script { get; set; }
        public int UnLockLevel { get; set; }
        public int MaxLevel { get; set; }
        public int Price { get; set; }
        public WeaponRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillName));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillDesc));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillIcon));
            recordStrList.Add(TableWriteBase.GetWriteStr(NameDict));
            recordStrList.Add(TableWriteBase.GetWriteStr(Script));
            recordStrList.Add(TableWriteBase.GetWriteStr(UnLockLevel));
            recordStrList.Add(TableWriteBase.GetWriteStr(MaxLevel));
            recordStrList.Add(TableWriteBase.GetWriteStr(Price));

            return recordStrList.ToArray();
        }
    }

    public partial class Weapon : TableFileBase
    {
        public Dictionary<string, WeaponRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public WeaponRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("Weapon" + ": " + id, ex);
            }
        }

        public Weapon(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, WeaponRecord>();
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

                    WeaponRecord record = new WeaponRecord(data);
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
                pair.Value.SkillName = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.SkillDesc = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.SkillIcon = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.NameDict = TableReadBase.ParseString(pair.Value.ValueStr[7]);
                pair.Value.Script = TableReadBase.ParseString(pair.Value.ValueStr[8]);
                pair.Value.UnLockLevel = TableReadBase.ParseInt(pair.Value.ValueStr[9]);
                pair.Value.MaxLevel = TableReadBase.ParseInt(pair.Value.ValueStr[10]);
                pair.Value.Price = TableReadBase.ParseInt(pair.Value.ValueStr[11]);
            }
        }
    }

}
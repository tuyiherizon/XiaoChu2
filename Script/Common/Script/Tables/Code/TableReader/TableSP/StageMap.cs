using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tables
{
    public class StageMapRecord
    {
        public static Dictionary<string, StageMapRecord> StageMapRecords = new Dictionary<string, StageMapRecord>();

        public int _Width = 0;
        public int _Height = 0;
        public Dictionary<string, string> _MapDefaults = new Dictionary<string, string>();
        public StageLogic _MapStageLogic = new StageLogic();
        public List<string> _StarInfos = new List<string>();
        public List<int> _HPBall = new List<int>();

        public string _ResPath = "";

        public string GetMapPos(int x, int y)
        {
            string key = x + "," + y;
            if (_MapDefaults.ContainsKey(key))
            {
                return _MapDefaults[key];
            }

            return "";
        }

        public void WriteStageMap()
        {
            string stageMapPath = Application.dataPath + "/XiaoChu/ResourcesTest/" + _ResPath + ".txt";
            System.IO.StreamWriter write = new System.IO.StreamWriter(stageMapPath);
            foreach (var mapPos in _MapDefaults)
            {
                string posLine = mapPos.Key + "=" + mapPos.Value;
                write.WriteLine(posLine);
            }

            string sizeLine = "Size=" + _Width + "," + _Height;
            write.WriteLine(sizeLine);

            string monsterLine = "Monster=";
            foreach (var wave in _MapStageLogic._Waves)
            {
                foreach (var monster in wave.NPCs)
                {
                    monsterLine += monster + ",";
                }
                monsterLine = monsterLine.Trim(',');
                monsterLine += ";";
            }
            monsterLine = monsterLine.Trim(';');
            write.WriteLine(monsterLine);

            string starInfoLine = "Star=";
            foreach (var starInfo in _StarInfos)
            {
                starInfoLine += starInfo + ";";
            }
            starInfoLine = starInfoLine.Trim(';');
            write.WriteLine(starInfoLine);

            string hpBallLine = "HPBall=";
            foreach (var hpBallInfo in _HPBall)
            {
                hpBallLine += hpBallInfo + ",";
            }
            hpBallLine = hpBallLine.Trim(',');
            write.WriteLine(hpBallLine);

            write.Close();
        }

        public static StageMapRecord ReadStageMap(string path)
        {
            if (StageMapRecords.ContainsKey(path))
                return StageMapRecords[path];

            StageMapRecord record = new StageMapRecord();
            record._ResPath = path;
            var tableAsset = ResourceManager.GetTable(path);
            string[] splits = tableAsset.Split('\n');
            foreach (var splitStr in splits)
            {
                string[] keyValue = splitStr.Split('=');
                if (keyValue.Length < 2)
                    continue;

                if (keyValue[0].Equals("Size"))
                {
                    string[] sizeStrs = keyValue[1].Split(',');
                    record._Width = int.Parse(sizeStrs[0]);
                    record._Height = int.Parse(sizeStrs[1]);
                }
                else if (keyValue[0].Equals("Monster"))
                {
                    string[] waves = keyValue[1].Split(';');
                    foreach (var wave in waves)
                    {
                        WaveInfo waveInfo = new WaveInfo();
                        waveInfo.NPCs = new List<string>();
                        string[] npcs = wave.Split(',');
                        foreach (var npc in npcs)
                        {
                            string npcID = npc.Trim(' ', ',','\r','\n');
                            if (!string.IsNullOrEmpty(npcID))
                            {
                                waveInfo.NPCs.Add(npcID);
                            }
                        }

                        record._MapStageLogic._Waves.Add(waveInfo);
                    }
                }
                else if (keyValue[0].Equals("Star"))
                {
                    string[] sizeStrs = keyValue[1].Split(';');
                    foreach (var sizStr in sizeStrs)
                    {
                        string trimStr = sizStr.Trim(' ', ',', '\r', '\n');
                        if (!string.IsNullOrEmpty(trimStr))
                        {
                            record._StarInfos.Add(trimStr);
                        }
                    }
                }
                else if (keyValue[0].Equals("HPBall"))
                {
                    string[] sizeStrs = keyValue[1].Split(',');
                    foreach (var sizStr in sizeStrs)
                    {
                        string trimStr = sizStr.Trim(' ', ',', '\r', '\n');
                        if (string.IsNullOrEmpty(trimStr))
                            continue;

                        int hpBall = int.Parse(trimStr);
                        if (hpBall > 0)
                        {
                            if (record._HPBall.Count > 0)
                            {
                                record._HPBall.Add(hpBall - record._HPBall[record._HPBall.Count - 1]);
                            }
                            else
                            {
                                record._HPBall.Add(hpBall);
                            }
                        }
                    }
                }
                else
                {
                    record._MapDefaults.Add(keyValue[0].Trim('\n', '\r', ' '), keyValue[1].Trim('\n', '\r', ' '));
                }
                 
            }

            StageMapRecords.Add(path, record);
            return record;
        }

    }

    public class StageMap
    {
        
    }
}
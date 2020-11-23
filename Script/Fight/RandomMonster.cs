using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Tables;

public class RandomMonster
{
    #region skill

    public static List<BoxSkillInfo> _BoxSkillList = new List<BoxSkillInfo>();

    public class BoxSkillInfo
    {
        public string _ID;
        public float _IncludeBombRate;
        public float _IncludeSameRate;
        public BallType _TrapType;
        public int _CD;
        public int _PreCD;
        public List<int> _TrapParam;
    }

    public static int MAX_LEVEL = 200;

    public static BoxSkillInfo GetRandomBoxSkill(int level, BallType ballType, string monsterID)
    {
        BoxSkillInfo boxSkillInfo = new BoxSkillInfo();

        if (level > 180)
        {
            int i = 1 + 1;
        }
        float levelRate = (float)((level) / (MAX_LEVEL * 0.9f));
        levelRate = Mathf.Clamp(levelRate, 0, 1);
        int trapLevel = (int)(((float)level / MAX_LEVEL) * 100);
        trapLevel = Mathf.Clamp(trapLevel, 0, 100);
        var trapList = GetRandomValue(trapLevel, 2, 50, 5);

       
        if (ballType == BallType.Stone)
        {
            int cdValue = (int)(trapList[0] * 0.16f);
            cdValue = (int)Mathf.Clamp(cdValue + 2, 2, 10);
            var cdIndependList = GetRandomValue(cdValue, 2, 5, 1);
            int skillValue = (int)Mathf.Clamp(trapList[1] * 0.24f, 1, 12);
            boxSkillInfo._TrapParam = GetRandomIndependValue(skillValue, 4, 3, 0);
            boxSkillInfo._CD = 6 - cdIndependList[0];
            boxSkillInfo._PreCD = 5 - cdIndependList[1];
        }
        else if (ballType == BallType.Posion)
        {
            int cdValue = (int)(trapList[0] * 0.12f);
            cdValue = (int)Mathf.Clamp(cdValue + 2, 2, 8);
            var cdIndependList = GetRandomValue(cdValue, 2, 4, 1);
            int skillValue = (int)Mathf.Clamp(trapList[1] * 0.24f, 1, 12);
            boxSkillInfo._TrapParam = GetRandomIndependValue(skillValue, 4, 3, 0);
            boxSkillInfo._CD = 5 - cdIndependList[0];
            boxSkillInfo._PreCD = 4 - cdIndependList[1];
        }
        else
        {
            int cdValue = (int)(trapList[0] * 0.12f);
            cdValue = (int)Mathf.Clamp(cdValue + 2, 2, 8);
            var cdIndependList = GetRandomValue(cdValue, 2, 4, 1);
            int skillValue = (int)Mathf.Clamp(trapList[1] * 0.24f, 1, 12);
            boxSkillInfo._TrapParam = GetRandomIndependValue(skillValue, 4, 3, 0);
            boxSkillInfo._CD = 4 - cdIndependList[0];
            boxSkillInfo._CD = Mathf.Max(boxSkillInfo._CD, 1);
            boxSkillInfo._PreCD = 4 - cdIndependList[1];
        }
        boxSkillInfo._IncludeBombRate = levelRate * 0.5f;
        boxSkillInfo._IncludeSameRate = levelRate;
        boxSkillInfo._TrapType = ballType;
        boxSkillInfo._ID = monsterID;

        _BoxSkillList.Add(boxSkillInfo);
        return boxSkillInfo;
    }

    public static void TestRandomTrap()
    {
        int level = 68;
        var boxSkillInfo = GetRandomBoxSkill(level, BallType.Ice, "1");
        string skillStr = "";
        skillStr += level;
        skillStr += ":(" + boxSkillInfo._IncludeBombRate + "," + boxSkillInfo._IncludeSameRate + ")";
        skillStr += ",(" + boxSkillInfo._CD + "," + boxSkillInfo._PreCD + ")";
        string skillParamStr = "";
        foreach (var skillParam in boxSkillInfo._TrapParam)
        {
            skillParamStr += "," + skillParam;
        }
        skillStr += ",(" + skillParamStr + ")";
        Debug.Log(skillStr);
    }

    static int MAX_TRAP_NUM = 4;
    static int MAX_TRAP_PARAM = 3;
    public static List<int> GetRandomIndependValue(int value, int maxNum, int maxParam, int minParam)
    {
        List<int> trapParams = new List<int>();

        int remainValue = value;
        while (remainValue > 0)
        {
            int paramValueMin = remainValue - (maxNum - trapParams.Count - 1) * maxParam;
            int paramValueMax = remainValue;
            int paramMin = Mathf.Max(paramValueMin, minParam);
            int paramMax = Mathf.Min(paramValueMax, maxParam);

            if (paramMin == paramMax)
            {
                trapParams.Add(paramMin);
                remainValue -= paramMin;
            }
            else
            {
                int randomParam = GetRandomTrapParam(paramMin, paramMax);
                trapParams.Add(randomParam);
                remainValue -= randomParam;
            }
        }

        return trapParams;
    }

    public static List<int> GetRandomValue(int totalVal, int num, int maxParam, int minParam)
    {
        List<int> randomValList = new List<int>();

        int remainValue = totalVal;
        for (int i = 0; i < num; ++i)
        {
            int paramValueMin = remainValue - (num - i - 1) * maxParam;
            int paramValueMax = remainValue - (num - i - 1) * minParam;
            int paramMin = Mathf.Max(paramValueMin, minParam);
            int paramMax = Mathf.Min(paramValueMax, maxParam);

            if (paramMin == paramMax)
            {
                randomValList.Add(paramMin);
                remainValue -= paramMin;
            }
            else
            {
                int randomParam = Random.Range(paramMin, paramMax);
                randomValList.Add(randomParam);
                remainValue -= randomParam;
            }
        }

        return randomValList;
    }

    public static int GetRandomTrapParam(int min, int max, float minWeight = 0.5f)
    {
        int mid =(max + 1) / 2;
        float minRate = Random.Range(0, 1.0f);
        if (minRate < minWeight)
        {
            if (min == mid)
                return min;
            return Random.Range(min, mid + 1);
        }
        else
        {
            if (max == mid)
                return max;
            return Random.Range(mid, max + 1);
        }
    }

    #endregion

    #region monster

    public static Dictionary<string, MonsterInfo> RandomMonsterList = new Dictionary<string, MonsterInfo>();

    public class MonsterInfo
    {
        public string ID;
        public ELEMENT_TYPE Element;
        public int Diff; // 1normal,2hp,3atk,4skill,5hpatk,6hpskill,7atkskill,8hpatkskill
        public int AttackRate;
        public int HpRate;
        public string AttackSkill;
        public BoxSkillInfo ExSkill;
    }

    public static StageLogic RandomMonsters(string stageID, List<ELEMENT_TYPE> skillElement, List<ELEMENT_TYPE> monsterEle)
    {
        StageLogic stageLogic = new StageLogic();
        stageLogic._Waves = new List<WaveInfo>();

        int randomWave = GameRandom.GetRandomLevel(10, 60, 35) + 1;
        List<int> waves = GetRandomValue(randomWave, 3, 3, 1);
        int stageIdInt = int.Parse(stageID);
        string stageBaseID = (stageIdInt + 1000).ToString();

        for (int i = 0; i < randomWave; ++i)
        {
            WaveInfo waveInfo = new WaveInfo();
            stageLogic._Waves.Add(waveInfo);
            waveInfo.NPCs = new List<string>();

            int atkTotal = 12;
            int randomNpcCnt = GameRandom.GetRandomLevel(10, 45, 45) + 1;
            for (int j = 0; j < randomNpcCnt; ++j)
            {
                MonsterInfo monster = new MonsterInfo();
                monster.ID = stageBaseID + i + j;
                int isBoxSkill = Random.Range(0, 10000);
                if (skillElement.Count > 0 && stageIdInt >= MAX_LEVEL * 0.1f && isBoxSkill < 3333)
                {
                    int randomEleIdx = Random.Range(0, skillElement.Count);
                    monster.Element = skillElement[randomEleIdx];
                    monster.ExSkill = GetRandomBoxSkill(stageIdInt, BattleField.ElementTypeToTrapType(monster.Element), monster.ID);
                }
                else
                {
                    int randomEleIdx = Random.Range(0, skillElement.Count + monsterEle.Count);
                    if (randomEleIdx > skillElement.Count - 1)
                    {
                        monster.Element = monsterEle[randomEleIdx - skillElement.Count];
                    }
                    else
                    {
                        monster.Element = skillElement[randomEleIdx];
                    }
                }
                monster.AttackSkill = Random.Range(1, 10).ToString();
                int atkMax = Mathf.Min(atkTotal, 10);
                monster.AttackRate = Random.Range(1, atkMax);
                atkTotal -= monster.AttackRate;
                atkTotal = Mathf.Max(1, atkTotal);
                monster.HpRate = Random.Range(1, 11);

                waveInfo.NPCs.Add(monster.ID);
                RandomMonsterList.Add(monster.ID, monster);
            }
        }

        return stageLogic;
    }

    public static StageLogic RandomMonstersV2(string stageID, List<ELEMENT_TYPE> skillElement, List<ELEMENT_TYPE> monsterEle)
    {
        StageLogic stageLogic = new StageLogic();
        stageLogic._Waves = new List<WaveInfo>();

        int stageIdInt = int.Parse(stageID);
        string stageBaseID = (stageIdInt + 1000).ToString();

        int monsterCnt = GameRandom.GetRandomLevel(2, 4, 10, 15, 37, 15, 10, 4, 3) + 1;
        if (stageIdInt == 1)
        {
            monsterCnt = 3;
        }
        else if (stageIdInt < 6)
        {
            monsterCnt = GameRandom.GetRandomLevel(0, 0, 30, 30, 40, 0, 0, 0, 0) + 1;
        }
        else if (stageIdInt < 20)
        {
            monsterCnt = GameRandom.GetRandomLevel(0, 5, 15, 20, 30, 15, 10, 5, 0) + 1;
        }
        List<int> waves = GetRandomIndependValue(monsterCnt, 3, 3, 1);

        for (int i = 0; i < waves.Count; ++i)
        {
            WaveInfo waveInfo = new WaveInfo();
            stageLogic._Waves.Add(waveInfo);
            waveInfo.NPCs = new List<string>();

            int atkTotal = 12;
            for (int j = 0; j < waves[i]; ++j)
            {
                MonsterInfo monster = new MonsterInfo();
                monster.ID = stageBaseID + i + j;
                int isBoxSkill = Random.Range(0, 10000);
                if (skillElement.Count > 0 && stageIdInt >= MAX_LEVEL * 0.1f && isBoxSkill < 3333)
                {
                    int randomEleIdx = Random.Range(0, skillElement.Count);
                    monster.Element = skillElement[randomEleIdx];
                    monster.ExSkill = GetRandomBoxSkill(stageIdInt, BattleField.ElementTypeToTrapType(monster.Element), monster.ID);
                }
                else
                {
                    int randomEleIdx = Random.Range(0, skillElement.Count + monsterEle.Count);
                    if (randomEleIdx > skillElement.Count - 1)
                    {
                        monster.Element = monsterEle[randomEleIdx - skillElement.Count];
                    }
                    else
                    {
                        monster.Element = skillElement[randomEleIdx];
                    }
                }
                monster.AttackSkill = "";
                
                int atkMax = Mathf.Min(atkTotal, 9);
                if (stageIdInt <= 10)
                {
                    atkMax = 5;
                    monster.AttackSkill = Random.Range(4, 10).ToString();
                }
                else if (stageIdInt <= 20)
                {
                    atkMax = 7;
                    monster.AttackSkill = Random.Range(2, 10).ToString();
                }
                monster.AttackRate = Random.Range(1, atkMax);

                if (string.IsNullOrEmpty(monster.AttackSkill))
                {
                    if (monsterCnt > 5 && monster.AttackRate > 5)
                    {
                        monster.AttackSkill = Random.Range(5, 10).ToString();
                    }
                    else if (monsterCnt > 5 && monster.AttackRate < 4)
                    {
                        monster.AttackSkill = Random.Range(2, 10).ToString();
                    }
                    else if (monsterCnt < 5 && monster.AttackRate < 4)
                    {
                        monster.AttackSkill = Random.Range(1, 4).ToString();
                    }
                    else if (monsterCnt < 5 && monster.AttackRate > 5)
                    {
                        monster.AttackSkill = Random.Range(2, 10).ToString();
                    }
                    else if(monster.AttackRate < 4)
                    {
                        monster.AttackSkill = Random.Range(1, 7).ToString();
                    }
                    else if (monster.AttackRate > 5)
                    {
                        monster.AttackSkill = Random.Range(4, 10).ToString();
                    }
                    else
                    {
                        monster.AttackSkill = Random.Range(1, 10).ToString();
                    }
                }

                atkTotal -= monster.AttackRate;
                atkTotal = Mathf.Max(1, atkTotal);

                int minHPRate = 4;
                int maxHPRate = 10;
                if (monsterCnt == 4)
                {
                    maxHPRate = 8;
                }
                if (monsterCnt == 5)
                {
                    maxHPRate = 7;
                }
                if (monsterCnt == 6)
                {
                    minHPRate = 2;
                    maxHPRate = 6;
                }
                if (monsterCnt == 7)
                {
                    minHPRate = 2;
                    maxHPRate = 5;
                }
                if (monsterCnt == 8)
                {
                    minHPRate = 2;
                    maxHPRate = 5;
                }
                if (monsterCnt >= 9)
                {
                    minHPRate = 1;
                    maxHPRate = 4;
                }
                monster.HpRate = Random.Range(minHPRate, maxHPRate);

                waveInfo.NPCs.Add(monster.ID);
                RandomMonsterList.Add(monster.ID, monster);
            }
        }

        return stageLogic;
    }

    public static void RefreshMonsterHP(StageMapRecord stageMapRecord)
    {
        int monsterCnt = 0;
        foreach (var wave in stageMapRecord._MapStageLogic._Waves)
        {
            foreach (var monsterInfo in wave.NPCs)
            {
                ++monsterCnt;
            }
        }

        int minHPRate = 4;
        int maxHPRate = 10;
        if (monsterCnt == 4)
        {
            maxHPRate = 8;
        }
        if (monsterCnt == 5)
        {
            maxHPRate = 7;
        }
        if (monsterCnt == 6)
        {
            minHPRate = 2;
            maxHPRate = 6;
        }
        if (monsterCnt == 7)
        {
            minHPRate = 2;
            maxHPRate = 5;
        }
        if (monsterCnt == 8)
        {
            minHPRate = 2;
            maxHPRate = 5;
        }
        if (monsterCnt >= 9)
        {
            minHPRate = 1;
            maxHPRate = 4;
        }

        foreach (var wave in stageMapRecord._MapStageLogic._Waves)
        {
            foreach (var monsterInfo in wave.NPCs)
            {
                RandomMonsterList[monsterInfo].HpRate = Random.Range(minHPRate, maxHPRate + 1);
            }
        }
    }

    public static void RefreshMonsterAtk(StageMapRecord stageMapRecord)
    {
        int monsterCnt = 0;
        int maxAtk = 0;
        foreach (var wave in stageMapRecord._MapStageLogic._Waves)
        {
            foreach (var monsterID in wave.NPCs)
            {
                var monTable = RandomMonsterList[monsterID];
                if (maxAtk < monTable.AttackRate)
                {
                    maxAtk = monTable.AttackRate;
                }
            }
        }

        if (maxAtk < 5)
        {
            var wave = stageMapRecord._MapStageLogic._Waves[stageMapRecord._MapStageLogic._Waves.Count - 1];
            int idx = Random.Range(0, wave.NPCs.Count);
            RandomMonsterList[wave.NPCs[idx]].AttackRate = Random.Range(5, 10);
        }
    }

    public static List<int> GetHpBalls(StageMapRecord stageRecord)
    {
        List<int> fullDamageRound = new List<int>();

        int round = 0;
        int monAtk = 0;
        int roleAtk = 0;
        int waveIdx = 0;
        while (fullDamageRound.Count < 10)
        {
            ++round;
            roleAtk += 1;
            float tempAtk = roleAtk;
            foreach (var monster in stageRecord._MapStageLogic._Waves[waveIdx].NPCs)
            {
                var monsterInfo = RandomMonsterList[monster];
                float monHP = GameDataValue.GetMonsterHPRate(monsterInfo.HpRate);
                tempAtk = tempAtk - monHP;
                if (tempAtk <= 0)
                {
                    var atkSkill = TableReader.SkillBase.GetRecord(monsterInfo.AttackSkill);
                    int skillRound = round - 1;
                    if (skillRound - atkSkill.PreCD == 0)
                    {
                        monAtk += monsterInfo.AttackRate;
                    }
                    else if (skillRound - atkSkill.PreCD > 0)
                    {
                        if (atkSkill.CD == 0)
                        {
                            monAtk += monsterInfo.AttackRate;
                        }
                        else if ((skillRound - atkSkill.PreCD) % atkSkill.CD == 0)
                        {
                            monAtk += monsterInfo.AttackRate;
                        }
                    }
                }
            }

            if (tempAtk > 0)
            {
                ++waveIdx;
                roleAtk = 0;
                if (waveIdx >= stageRecord._MapStageLogic._Waves.Count)
                    break;
            }

            if (monAtk >= 5)
            {
                monAtk = 0;
                fullDamageRound.Add(round);
            }
        }

        List<int> hpBall = new List<int>();
        foreach (var fullDamage in fullDamageRound)
        {
            int ballCnt = 0;
            for (int i = 0; i < fullDamage; ++i)
            {
                if (stageRecord._MapDefaults.Count > 4)
                {
                    ballCnt += Random.Range(10, 15);
                }
                else if (stageRecord._MapDefaults.Count > 6)
                {
                    ballCnt += Random.Range(6, 12);
                }
                else
                {
                    ballCnt += Random.Range(12, 20);
                }
            }

            hpBall.Add(ballCnt);
        }
        return hpBall;
    }

    public static List<string> GetStarInfo(StageMapRecord stageMapRecord, int level)
    {
        List<string> starInfos = new List<string>();
        starInfos.Add("PassStage");

        int round = 0;
        int monAtk = 0;
        int roleAtk = 0;
        int waveIdx = 0;
        while (round < 50)
        {
            ++round;
            roleAtk += 1;
            float tempAtk = roleAtk;
            foreach (var monster in stageMapRecord._MapStageLogic._Waves[waveIdx].NPCs)
            {
                //var monsterInfo = TableReader.MonsterBase.GetRecord(monster);
                var monsterInfo = RandomMonsterList[monster];
                float monHP = GameDataValue.GetMonsterHPRate(monsterInfo.HpRate);
                tempAtk = tempAtk - monHP;
            }

            if (tempAtk > 0)
            {
                ++waveIdx;
                roleAtk = 0;
                if (waveIdx >= stageMapRecord._MapStageLogic._Waves.Count)
                    break;
            }
        }
        Debug.Log("GetStarInfo round:" + round);
        int randomStar2 = Random.Range(0, 2);
        if (randomStar2 == 0)
        {
            starInfos.Add("FightRound," + (round + 1));
        }
        else
        {
            int remainHpRandom = Random.Range(4, 9);
            starInfos.Add("RemainHP," + remainHpRandom);
        }

        int mapTrapCnt = 0;
        foreach (var trap in stageMapRecord._MapDefaults)
        {
            if (trap.Value.Contains("11")
                || trap.Value.Contains("12")
                || trap.Value.Contains("13")
                || trap.Value.Contains("14"))
            {
                ++mapTrapCnt;
            }
        }

        if (mapTrapCnt > 8)
        {
            if (level > MAX_LEVEL * 0.7f)
            {
                starInfos.Add("Trap," + (mapTrapCnt));
            }
            else if (level > MAX_LEVEL * 0.5f)
            {
                starInfos.Add("Trap," + (mapTrapCnt - 1));
            }
            else if (level > MAX_LEVEL * 0.3f)
            {
                starInfos.Add("Trap," + (mapTrapCnt - 2));
            }
            else
            {
                starInfos.Add("Trap," + (mapTrapCnt - 3));
            }
        }
        else
        {
            float levelRate = (level / MAX_LEVEL) * 0.4f + 0.2f;
            starInfos.Add("Bomb," + (int)(round* levelRate));
        }

        return starInfos;
    }

    public static List<ELEMENT_TYPE> GetStageElementTypes(StageMapRecord stageMapRecord)
    {
        List<ELEMENT_TYPE> recordElements = new List<ELEMENT_TYPE>();
        foreach (var trap in stageMapRecord._MapDefaults)
        {
            ELEMENT_TYPE trapElement = ELEMENT_TYPE.NONE;
            if (trap.Value.Contains("11"))
            {
                trapElement = ELEMENT_TYPE.ICE;
            }
            else if (trap.Value.Contains("12"))
            {
                trapElement = ELEMENT_TYPE.FIRE;
            }
            else if (trap.Value.Contains("13"))
            {
                trapElement = ELEMENT_TYPE.LIGHT;
            }
            else if (trap.Value.Contains("14"))
            {
                trapElement = ELEMENT_TYPE.WIND;
            }
            else if (trap.Value.Contains("15"))
            {
                trapElement = ELEMENT_TYPE.DARK;
            }

            if (trapElement != ELEMENT_TYPE.NONE && !recordElements.Contains(trapElement))
            {
                recordElements.Add(trapElement);
            }           
        }

        return recordElements;
    }

    public static List<ELEMENT_TYPE> GetStageExElement(List<ELEMENT_TYPE> trapEles)
    {
        List<ELEMENT_TYPE> lastEles = new List<ELEMENT_TYPE>();
        for (int i = (int)ELEMENT_TYPE.FIRE; i < (int)ELEMENT_TYPE.DARK + 1; ++i)
        {
            if (trapEles.Contains((ELEMENT_TYPE)i))
                continue;
            lastEles.Add((ELEMENT_TYPE)i);
        }

        List<ELEMENT_TYPE> exEles = new List<ELEMENT_TYPE>();
        if (lastEles.Count == 5)
        {
            int eleCnt = GameRandom.GetRandomLevel(20, 50, 30) + 1;
            var idxList = GameRandom.GetIndependentRandoms(0, 4, eleCnt);
            foreach (var idx in idxList)
            {
                exEles.Add(lastEles[idx]);
            }
        }
        else if (lastEles.Count == 4)
        {
            int eleCnt = GameRandom.GetRandomLevel(20, 50, 30);
            var idxList = GameRandom.GetIndependentRandoms(0, 3, eleCnt);
            foreach (var idx in idxList)
            {
                exEles.Add(lastEles[idx]);
            }
        }
        else if (lastEles.Count == 3)
        {
            int eleCnt = GameRandom.GetRandomLevel(50, 50);
            var idxList = GameRandom.GetIndependentRandoms(0, 2, eleCnt);
            foreach (var idx in idxList)
            {
                exEles.Add(lastEles[idx]);
            }
        }

        return exEles;
    }

    public static StageMapRecord RandomStage(string stageID,StageMapRecord stageMap)
    {
        int level = int.Parse(stageID);
        List<ELEMENT_TYPE> skillEles = GetStageElementTypes(stageMap);
        List<ELEMENT_TYPE> atkEles = GetStageExElement(skillEles);
        stageMap._MapStageLogic = RandomMonstersV2(stageID, skillEles, atkEles);
        stageMap._StarInfos = GetStarInfo(stageMap, level);
        stageMap._HPBall = GetHpBalls(stageMap);

        return stageMap;
    }

    public static StageMapRecord RefreshStageHPBall(StageMapRecord stageMap)
    {
        if (stageMap._HPBall.Count == 0)
        {
            Debug.Log("_HPBall Empty:" + stageMap._ResPath);
            return stageMap;
        }
        List<int> newHPBalls = new List<int>();
        newHPBalls.Add(stageMap._HPBall[0]);
        for (int i = 1; i < stageMap._HPBall.Count; ++i)
        {
            newHPBalls.Add(stageMap._HPBall[i] - stageMap._HPBall[i - 1]);
        }
        stageMap._HPBall = newHPBalls;
        return stageMap;
    }

    public static StageMapRecord RefreshStageStar(StageMapRecord stageMap, string stageID)
    {
        int level = int.Parse(stageID);
        stageMap._StarInfos = GetStarInfo(stageMap, level);

        return stageMap;
    }

    #endregion

    public static void WriteSkills()
    {
        string stageMapPath = Application.dataPath + "/XiaoChu/ResourcesTest/SkillBase.csv";
        System.IO.StreamWriter write = new System.IO.StreamWriter(stageMapPath);
        string titleLine = "ID,,,,CD,PreCD,Script,SameRate,BombRate,TrapType,TrapParam1,TrapParam2,TrapParam3,TrapParam4";
        write.WriteLine(titleLine);
        foreach (var skillInfo in _BoxSkillList)
        {
            string skillLine = skillInfo._ID + ",,,," + skillInfo._CD + "," + skillInfo._PreCD + ",SkillBoxBall," + 
                GameDataValue.ConfigFloatToInt(skillInfo._IncludeSameRate) + "," + GameDataValue.ConfigFloatToInt(skillInfo._IncludeBombRate)
                + "," + (int)skillInfo._TrapType;
            for(int i = 0; i< 4; ++i) 
            {
                if (i < skillInfo._TrapParam.Count)
                {
                    skillLine += "," + skillInfo._TrapParam[i];
                }
                else
                {
                    skillLine += ",0";
                }
            }
            write.WriteLine(skillLine);
        }
        write.Close();
    }

    public static void WriteMonsters()
    {
        string stageMapPath = Application.dataPath + "/XiaoChu/ResourcesTest/MonsterBase.csv";
        System.IO.StreamWriter write = new System.IO.StreamWriter(stageMapPath);
        string titleLine = "ID,,,,Model,Attack,Defence,HP,ElementType,Skills1,Skills2,Skill3";
        write.WriteLine(titleLine);
        foreach (var monsterInfo in RandomMonsterList.Values)
        {
            string model = "";
            if (monsterInfo.ExSkill != null && monsterInfo.AttackRate > 7 && monsterInfo.HpRate > 7)
            {
                model = "MonDamageBoss";
            }
            else if (monsterInfo.Element == ELEMENT_TYPE.FIRE)
            {
                if (monsterInfo.ExSkill != null)
                {
                    model = "MonFireSkill";
                }
                else if (monsterInfo.AttackRate > 5)
                {
                    model = "MonFireDamage";
                }
                else
                {
                    model = "MonFireNormal";
                }
            }
            else if (monsterInfo.Element == ELEMENT_TYPE.ICE)
            {
                if (monsterInfo.ExSkill != null)
                {
                    model = "MonIceSkill";
                }
                else if (monsterInfo.AttackRate > 5)
                {
                    model = "MonIceDamage";
                }
                else
                {
                    model = "MonIceNormal";
                }
            }
            else if (monsterInfo.Element == ELEMENT_TYPE.WIND)
            {
                if (monsterInfo.ExSkill != null)
                {
                    model = "MonPosionSkill";
                }
                else if (monsterInfo.AttackRate > 5)
                {
                    model = "MonPosionDamage";
                }
                else
                {
                    model = "MonPosionNormal";
                }
            }
            else if (monsterInfo.Element == ELEMENT_TYPE.LIGHT)
            {
                if (monsterInfo.ExSkill != null)
                {
                    model = "MonLightSkill";
                }
                else if (monsterInfo.AttackRate > 5)
                {
                    model = "MonLightDamage";
                }
                else
                {
                    model = "MonLightNormal";
                }
            }
            else if (monsterInfo.Element == ELEMENT_TYPE.DARK)
            {
                if (monsterInfo.ExSkill != null)
                {
                    model = "MonDarkSkill";
                }
                else if (monsterInfo.AttackRate > 5)
                {
                    model = "MonDarkDamage";
                }
                else
                {
                    model = "MonDarkNormal";
                }
            }
            string skillLine = monsterInfo.ID+ ",,,," + model + "," + monsterInfo.AttackRate + ",10," + monsterInfo.HpRate + "," + (int)monsterInfo.Element;
            if (monsterInfo.ExSkill != null)
            {
                skillLine += "," + monsterInfo.ExSkill._ID;
            }
            skillLine += "," + monsterInfo.AttackSkill;
            write.WriteLine(skillLine);
        }
        write.Close();
    }

    #region record maps

    public static List<List<string>> _MapRecord = new List<List<string>>();

    public static void RecordMap(BallInfo[][] ballInfos)
    {
        List<string> recordStr = new List<string>();
        for (int j = 0; j < ballInfos[0].Length; ++j)
        {
            for (int i = 0; i < ballInfos.Length; ++i)
            {
                string ballStr = i + "," + j + "=";
                if (ballInfos[i][j].IsBombBall())
                {
                    ballStr += (int)ballInfos[i][j].BallSPType + "," + (int)ballInfos[i][j].BallType;
                }
                else if (ballInfos[i][j].IsTrapBall())
                {
                    ballStr += (int)ballInfos[i][j].BallSPType + "," + ((BallInfoSPTrapBase)ballInfos[i][j]._BallInfoSP).ElimitNum;
                }
                else
                {
                    ballStr += (int)ballInfos[i][j].BallType + "," + (int)ballInfos[i][j].BallType;
                }
                ballStr += ",N";
                recordStr.Add( ballStr);
            }
        }

        _MapRecord.Add(recordStr);
    }

    public static void WriteMapRecord(string stageID)
    {
        string path = Application.persistentDataPath + "/maprecord/" + stageID;

        for(int i = 0; i< _MapRecord.Count; ++i)
        {
            string recordPath = path + "_" + i + ".txt";
            CommonDefine.CheckTargetPath(recordPath);
            System.IO.StreamWriter write = new System.IO.StreamWriter(recordPath);
            foreach (var line in _MapRecord[i])
            {
                write.WriteLine(line);
            }
            write.Close();
        }
    }

    #endregion
}

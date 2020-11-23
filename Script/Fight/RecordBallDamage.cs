using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordBallDamage
{
    private static List<List<int>> _DamageRecords = new List<List<int>>();
    private static int _RecordTimeIdx = 0;

    public static int _RecordRound = 20;
    public static int _RecordTimes = 10;

    public static void ClearRecords()
    {
        _DamageRecords.Clear();
        _RecordTimeIdx = 0;
    }

    public static void RecordDamage(int damage)
    {
        if (_DamageRecords.Count == 0)
        {
            _DamageRecords.Add(new List<int>());
        }
        _DamageRecords[_RecordTimeIdx].Add(damage);

        if (_DamageRecords[_RecordTimeIdx].Count == _RecordRound)
        {
            if (_DamageRecords.Count == _RecordTimes)
            {
                RecordDamageInfo();
            }
            else
            {
                ++_RecordTimeIdx;
                _DamageRecords.Add(new List<int>());
            }
        }
    }

    private static Dictionary<string, string> _StageDamageRecords = new Dictionary<string, string>();

    public static string LoadingStageID = "";

    public static void RecordDamageInfo()
    {
        
        string stageDamageStr = LoadingStageID;
        string strArith = "";
        string strMedian = "";
        for (int i = 0; i < _DamageRecords[0].Count; ++i)
        {
            List<int> mathList = new List<int>();
            for (int j = 0; j < _DamageRecords.Count; ++j)
            {
                mathList.Add(_DamageRecords[j][i]);
            }
            int arithmetic = GetArithmetic(mathList);
            int median = GetMedian(mathList);

            strArith += "\t" + arithmetic;
            strMedian += "\t" + median;
        }
        stageDamageStr += strArith + strMedian;

        _StageDamageRecords.Add(LoadingStageID, stageDamageStr);

        _DamageRecords.Clear();
        _RecordTimeIdx = 0;
    }

    public static void WriteRecords()
    {
        string recordPath = Application.dataPath + "/../build/Records/StageWeapon" + "_" + WeaponDataPack.Instance.SelectedWeaponItem.ItemDataID + ".txt";
        StreamWriter writer = new StreamWriter(recordPath);

        foreach (var record in _StageDamageRecords)
        {
            writer.WriteLine(record.Value);
        }

        writer.Close();
    }

    static int GetArithmetic(List<int> mathList)
    {
        int total = 0;
        foreach (var num in mathList)
        {
            total += num;
        }
        return total / mathList.Count;
    }

    static int GetMedian(List<int> mathList)
    {
        mathList.Sort();
        return mathList[(int)(mathList.Count * 0.5f)];
    }

}

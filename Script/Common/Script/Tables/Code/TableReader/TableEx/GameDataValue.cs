using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{

    public enum VALUE_IDX
    {
        FIGHT_VALUE = 0,
        STAGE_GOLD = 1,
    }

    public partial class GameDataValueRecord : TableRecordBase
    {

    }

    public partial class GameDataValue : TableFileBase
    {


        public static int GetLevelDataValue(int level, VALUE_IDX valueIdx)
        {
            var record = TableReader.GameDataValue.GetRecord(level.ToString());
            return record.Values[(int)valueIdx];
        }
    }

}
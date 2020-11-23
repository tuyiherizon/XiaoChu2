using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarInfoBase
{

    public static bool isCanGetStar(string starParamStr)
    {
        string[] starParams = starParamStr.Split(',');

        switch (starParams[0])
        {
            case "PassStage":
                return true;
            case "FightRound":
                int round = int.Parse(starParams[1]);
                if (BattleField.Instance._BattleRound - 1 < round)
                {
                    return true;
                }
                break;
            case "RemainHP":
                int hp = int.Parse(starParams[1]);
                if ((float)BattleField.Instance._RoleMotion._HP / BattleField.Instance._RoleMotion._MaxHP * 10 >= hp)
                {
                    return true;
                }
                break;
            case "Bomb":
                int bomb = int.Parse(starParams[1]);
                if (BallBox.Instance._ElimitBombCnt >= bomb)
                {
                    return true;
                }
                break;
            case "Trap":
                int trap = int.Parse(starParams[1]);
                if (BallBox.Instance._ElimitTrapCnt >= trap)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public static string GetStarConditionStr(string starParamStr)
    {
        string[] starParams = starParamStr.Split(',');

        string param = "";
        if (starParams.Length > 1)
        {
            param = starParams[1];
        }

        switch (starParams[0])
        {
            case "PassStage":
                return Tables.StrDictionary.GetFormatStr(11000);
            case "FightRound":
                return Tables.StrDictionary.GetFormatStr(11001, param);
            case "RemainHP":
                int remainHP = int.Parse(param) * 10;
                return Tables.StrDictionary.GetFormatStr(11002, remainHP);
            case "Bomb":
                return Tables.StrDictionary.GetFormatStr(11003, param);
            case "Trap":
                return Tables.StrDictionary.GetFormatStr(11004, param);
        }

        return "";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataValue
{
    public static float ConfigIntToFloat(int val)
    {
        var resultVal = new decimal(0.0001) * new decimal(val);
        return (float)resultVal;
    }

    public static float ConfigIntToFloatDex1(int val)
    {
        int dex = Mathf.RoundToInt(val * 0.1f);
        var resultVal = new decimal(0.001) * new decimal(dex);
        return (float)resultVal;
    }

    public static float ConfigIntToPersent(int val)
    {

        var resultVal = new decimal(0.01) * new decimal(val);
        return (float)resultVal;
    }

    public static int ConfigFloatToInt(float val)
    {
        return Mathf.RoundToInt(val * 10000);
    }

    public static int ConfigFloatToPersent(float val)
    {
        float largeVal = val * 100;
        var intVal = Mathf.RoundToInt(largeVal);
        return intVal;
    }

    public static int GetMaxRate()
    {
        return 10000;
    }

    #region weapon

    public static int GetLevelAtk(int level)
    {
        int fightValue = Tables.GameDataValue.GetLevelDataValue(level, Tables.VALUE_IDX.FIGHT_VALUE);

        return fightValue + 5;
    }

    public static int GetLevelHP(int level)
    {
        int fightValue = Tables.GameDataValue.GetLevelDataValue(level, Tables.VALUE_IDX.FIGHT_VALUE);

        return (int)(fightValue * 1.4f) + 10;
    }

    public static float GetMonsterHPRate(int rateLevel)
    {
        float baseRate = ((float)rateLevel) / 4 + 0.5f;
        return baseRate;        
    }

    public static float GetMonsterHPGemFix(int level)
    {
        float fix = 0;
        if (level > 30)
            fix = (level - 30) * 0.012f;

        return 1 + fix;
    }

    public static int GetWeaponLevelUpMoney(WeaponDataItem weaponItem)
    {
        return 1;
    }

    #endregion

    #region resource exchange

    public static int DiamondExchangeGold(int diamond)
    {
        int value = (int)(diamond * Tables.GameDataValue.GetLevelDataValue(101, Tables.VALUE_IDX.STAGE_GOLD) * 10);
        return value;
    }

    public static int DiamondExchangeGemfrag(int diamond)
    {
        int value = (int)(diamond * GemDataPack.GetCombineCost(2));
        return value;
    }

    public static int DiamondExchange(string moneyId, int diamondVal)
    {
        if (moneyId.Equals(PlayerDataPack.MoneyGold))
        {
            return DiamondExchangeGold(diamondVal);
        }
        else if (moneyId.Equals(PlayerDataPack.MoneyGemFrag))
        {
            return DiamondExchangeGemfrag(diamondVal);
        }

        return 0;
    }

    #endregion
}

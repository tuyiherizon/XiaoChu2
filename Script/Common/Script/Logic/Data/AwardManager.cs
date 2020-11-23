using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;

public class AwardItem
{
    public AwardItem(string awardType, int awardValue)
    {
        AwardType = awardType;
        AwardValue = awardValue;
    }

    public string AwardType;
    public int AwardValue;

    public CommonItemRecord ItemRecord
    {
        get
        {
            return TableReader.CommonItem.GetRecord(AwardType);
        }
    }
}

public class AwardManager
{
    public static AwardItem AddAward(string awardType, string awardValue)
    {
        int value = int.Parse(awardValue);

        return AddAward(awardType, value);
    }

    public static AwardItem AddAward(string awardType, int awardValue)
    {
        AwardItem awardItem = new AwardItem(awardType, awardValue);

        if (PlayerDataPack.IsMoney(awardType))
        {
            PlayerDataPack.Instance.AddMoney(awardType, awardValue);
        }

        //gem
        if (awardType.Equals("20001"))
        {
            GemDataPack.Instance.AddRandomGem(1);
        }
        else if (awardType.Equals("20002"))
        {
            GemDataPack.Instance.AddRandomGem(2);
        }
        else if (awardType.Equals("20003"))
        {
            GemDataPack.Instance.AddRandomGem(3);
        }
        else if (awardType.Equals("20004"))
        {
            GemDataPack.Instance.AddRandomGem(4);
        }

        return awardItem;
    }
}

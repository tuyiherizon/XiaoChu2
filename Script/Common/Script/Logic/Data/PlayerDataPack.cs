using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;



public class PlayerDataPack : DataPackBase
{
    #region 单例

    private static PlayerDataPack _Instance;
    public static PlayerDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PlayerDataPack();
            }
            return _Instance;
        }
    }

    private PlayerDataPack()
    {
        _SaveFileName = "PlayerDataPack";
    }

    #endregion

    #region money

    public const string MoneyGold = "10001";
    public const string MoneyDiamond = "10002";
    public const string MoneyGemFrag = "10003";

    public class MoneyInfo
    {
        [SaveField(1)]
        public string ID;
        [SaveField(2)]
        public int Value;
    }

    [SaveField(1)]
    public List<MoneyInfo> _MoneyInfos;

    public static bool IsMoney(string id)
    {
        if (id.Equals(MoneyGold)
            || id.Equals(MoneyDiamond)
            || id.Equals(MoneyGemFrag))
            return true;

        return false;
    }

    public int GetMoney(string id)
    {
        var moneyInfo = GetMoneyInfo(id);

        if (moneyInfo == null)
            return 0;

        return moneyInfo.Value;
    }

    public MoneyInfo GetMoneyInfo(string id)
    {
        var find = _MoneyInfos.Find((moneyInfo) =>
        {
            if (moneyInfo.ID == id)
                return true;
            return false;
        });
        return find;
    }

    public void AddMoney(string id, int value)
    {
        var moneyInfo = GetMoneyInfo(id);
        if (moneyInfo == null)
        {
            moneyInfo = new MoneyInfo();
            moneyInfo.ID = id;
            moneyInfo.Value = value;
            _MoneyInfos.Add(moneyInfo);
        }
        else
        {
            moneyInfo.Value += value;
        }
        SaveClass(true);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_REFRESH_MONEY, this, null);
    }

    public bool DecMoney(string id, int value)
    {
        var moneyInfo = GetMoneyInfo(id);
        if (moneyInfo == null)
        {
            var commonitem = TableReader.CommonItem.GetRecord(id);
            UIMessageTip.ShowMessageTip(StrDictionary.GetFormatStr(2001, StrDictionary.GetFormatStr(commonitem.NameStrDict)));
            return false;
        }
        else if(moneyInfo.Value < value)
        {
            var commonitem = TableReader.CommonItem.GetRecord(id);
            UIMessageTip.ShowMessageTip(StrDictionary.GetFormatStr(2001, StrDictionary.GetFormatStr(commonitem.NameStrDict)));
            return false;
        }

        moneyInfo.Value -= value;
        SaveClass(true);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_REFRESH_MONEY, this, null);
        return true;
    }
    
    #endregion

    
}


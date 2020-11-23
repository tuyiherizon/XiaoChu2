using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class GemDataItem :ItemBase
{
    private Tables.GemRecord _GemRecord;
    public GemRecord GemRecord
    {
        get
        {
            if (_GemRecord == null)
            {
                if (!string.IsNullOrEmpty(ItemDataID))
                {
                    _GemRecord = TableReader.Gem.GetRecord(ItemDataID);
                }
            }
            return _GemRecord;
        }
    }

    public string ExAttrID
    {
        get
        {
            if (_DynamicDataEx.Count > 0)
                return _DynamicDataEx[0]._StrParams[0];
            return "";
        }
        set
        {
            if (_DynamicDataEx.Count == 0)
            {
                _DynamicDataEx.Add(new ItemExData());
                _DynamicDataEx[0]._StrParams.Add(value);
            }

            _DynamicDataEx[0]._StrParams[0] = value;
        }
    }

    private Tables.GemExAttrRecord _GemExAttrRecord;
    public GemExAttrRecord GemExAttrRecord
    {
        get
        {
            if (_GemExAttrRecord == null)
            {
                if (!string.IsNullOrEmpty(ExAttrID))
                {
                    _GemExAttrRecord = TableReader.GemExAttr.GetRecord(ExAttrID);
                }
            }
            return _GemExAttrRecord;
        }
    }

    public float GetHPBallRate()
    {
        if (GemExAttrRecord != null && GemExAttrRecord.Script.Equals("HPEnhance"))
        {
            int param = int.Parse(GemExAttrRecord.Params[0]);
            return GameDataValue.ConfigIntToFloat(param);
        }

        float addHPRate = GameDataValue.ConfigIntToFloat(GemDataPack.Instance.SelectedGemItem.GemRecord.Attrs[5]);
        return addHPRate;
    }
}

public class GemDataPack : DataPackBase
{
    #region 单例

    private static GemDataPack _Instance;
    public static GemDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GemDataPack();
            }
            return _Instance;
        }
    }

    private GemDataPack()
    {
        _SaveFileName = "GemDataPack";
    }

    #endregion

    #region 

    [SaveField(1)]
    private int _SelectedGemIdx;

    private GemDataItem _SelectedGemItem;
    public GemDataItem SelectedGemItem
    {
        get
        {
            return _SelectedGemItem;
        }
    }

    public ItemPackBase<GemDataItem> _GemItems = new ItemPackBase<GemDataItem>();

    public const int _BuyGemCost1 = 50;
    public const int _BuyGemCost2 = 100;

    public static int GetCombineCost(int level)
    {
        switch (level)
        {
            case 2:
                return 20;
            case 3:
                return 50;
            case 4:
                return 100;
            case 5:
                return 250;
        }

        return 0;
    }

    public void InitGemInfo()
    {
        _SelectedGemItem = null;

        _GemItems = new ItemPackBase<GemDataItem>();
        _GemItems._SaveFileName = "GemDataItems";
        _GemItems.LoadClass(true);

        if (_GemItems._PackItems == null ||  _GemItems._PackItems.Count == 0)
        {
            _GemItems._PackItems = new List<GemDataItem>();
            GemDataItem defaultGem = new GemDataItem();
            defaultGem.ItemDataID = "1";
            _GemItems.AddItem(defaultGem);
            _SelectedGemItem = defaultGem;
            //_GemItems.SaveClass(true);
        }
        if (_GemItems._PackItems.Count > _SelectedGemIdx && _SelectedGemIdx >= 0)
        {
            _SelectedGemItem = _GemItems._PackItems[_SelectedGemIdx];
        }
    }

    public override void SaveClass(bool isSaveChild)
    {
        _SelectedGemIdx = _GemItems._PackItems.IndexOf(_SelectedGemItem);

        base.SaveClass(isSaveChild);
    }

    public void SelectGem(GemDataItem selectGem)
    {
        _SelectedGemItem = selectGem;
        SaveClass(false);
    }

    public GemDataItem GetRandomGemItem(int level)
    {
        List<GemRecord> levelGems = new List<GemRecord>();
        foreach (var gemRecord in TableReader.Gem.Records.Values)
        {
            if (gemRecord.Level == level)
            {
                levelGems.Add(gemRecord);
            }
        }

        List<GemExAttrRecord> levelGemAttrs = new List<GemExAttrRecord>();
        foreach (var gemAttr in TableReader.GemExAttr.Records.Values)
        {
            if (gemAttr.Level <= level)
            {
                levelGemAttrs.Add(gemAttr);
            }
        }

        if (levelGems.Count == 0)
            return null;

        int randomIdx = Random.Range(0, levelGems.Count);

        GemDataItem gemDataItem = new GemDataItem();
        gemDataItem.ItemDataID = levelGems[randomIdx].Id;

        if (levelGemAttrs.Count > 0)
        {
            int randomAttrIdx = Random.Range(0, levelGemAttrs.Count);
            gemDataItem.ExAttrID = levelGemAttrs[randomAttrIdx].Id;
        }

        return gemDataItem;
    }

    public List<GemDataItem> BuyGemItem(int level, int buyNum)
    {
        List<GemDataItem> resultItems = new List<GemDataItem>();

        if (level == 0)
        {
            if (!PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyGemFrag, _BuyGemCost1 * buyNum))
                return null;

            for (int i = 0; i < buyNum; ++i)
            {
                int gemLevel = GameRandom.GetRandomLevel(50, 50) + 1;
                gemLevel = Mathf.Clamp(gemLevel, 1, 5);
                var randomItem = GetRandomGemItem(gemLevel);
                _GemItems.AddItem(randomItem);
                resultItems.Add(randomItem);
            }
        }
        else if (level == 1)
        {
            if (!PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyGemFrag, _BuyGemCost2 * buyNum))
                return null;

            for (int i = 0; i < buyNum; ++i)
            {
                int gemLevel = GameRandom.GetRandomLevel(0,50, 50) + 1;
                gemLevel = Mathf.Clamp(gemLevel, 1, 5);
                var randomItem = GetRandomGemItem(gemLevel);
                _GemItems.AddItem(randomItem);
                resultItems.Add(randomItem);
            }
        }

        //_GemItems.SaveClass(true);
        Hashtable hash = new Hashtable();
        hash.Add("Num", buyNum);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_BUY_GEM, this, hash);

        return resultItems;
    }

    public GemDataItem AddRandomGem(int level)
    {
        var randomItem = GetRandomGemItem(level);
        _GemItems.AddItem(randomItem);

        //_GemItems.SaveClass(true);

        UIMainFun.ShowRedTip();
        return randomItem;
    }

    public GemDataItem CombineGemItem(List<GemDataItem> combineItems)
    {
        if (combineItems.Count < 5)
            return null;

        foreach (var gemItem in combineItems)
        {
            if (!_GemItems._PackItems.Contains(gemItem))
            {
                return null;
            }
        }

        int matLevel = 0;
        foreach (var combineItem in combineItems)
        {
            if (matLevel == 0)
            {
                matLevel = combineItem.GemRecord.Level;
            }
            else if(matLevel > combineItem.GemRecord.Level)
            {
                matLevel = combineItem.GemRecord.Level;
            }
        }

        int gemLevel = matLevel + 1;
        gemLevel = Mathf.Clamp(gemLevel, 1, 5);
        if (!PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyGemFrag, GetCombineCost(gemLevel)))
            return null;

        foreach (var combineItem in combineItems)
        {
            _GemItems.DecItem(combineItem);
        }

        var randomItem = GetRandomGemItem(gemLevel);
        _GemItems.AddItem(randomItem);

        Hashtable hash = new Hashtable();
        hash.Add("Level", gemLevel);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_COMBINE_GEM, this, hash);

        //_GemItems.SaveClass(true);
        return randomItem;
    }

    public bool IsSelectedGemLvLow()
    {
        foreach (var gemItem in _GemItems._PackItems)
        {
            if (gemItem.GemRecord.Level > SelectedGemItem.GemRecord.Level)
            {
                return true;
            }
        }

        return false;
    }

    #endregion


}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum MONEYTYPE
{
    GOLD = 0,
    DIAMOND,
    ITEM,
}

public class UICurrencyItem : UIItemBase
{

    #region 

    public Image _CurrencyIcon;
    public Text _CurrencyValue;

    public string _ShowOwnCurrency;

    private int _CurrencyIntValue;
    public int CurrencyIntValue
    {
        get
        {
            return _CurrencyIntValue;
        }
    }

    #endregion

    void OnEnable()
    {
        if (!string.IsNullOrEmpty(_ShowOwnCurrency))
        {
            ShowOwnCurrency(_ShowOwnCurrency);
        }
    }

    void OnDisable()
    {

        if (PlayerDataPack.IsMoney(_ShowOwnCurrency))
        {
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_REFRESH_MONEY, RefreshOwnCurrency);
        }

    }

    #region 

    public void SetValue(int value)
    {
        _CurrencyValue.text = value.ToString();
        _CurrencyIntValue = value;
    }

    public void ShowCurrency(string itemID, long currencyValue)
    {
        //_CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];
        var itemBase = Tables.TableReader.CommonItem.GetRecord(itemID);
        ResourceManager.Instance.SetImage(_CurrencyIcon, itemBase.Icon);

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = (int)currencyValue;
    }


    public void ShowCostCurrency(string itemDataID, int costValue, int ownCnt = -1)
    {
        int Ownvalue = ownCnt;
        if (Ownvalue < 0)
        {
           
        }
        ShowCurrency(itemDataID, Ownvalue);

        string currencyStr = "";
        if (costValue > Ownvalue)
        {
            currencyStr = CommonDefine.GetEnableRedStr(0) + Ownvalue.ToString() + "</color>/" + costValue.ToString();
        }
        else
        {
            currencyStr = CommonDefine.GetEnableRedStr(1) + Ownvalue.ToString() + "</color>/" + costValue.ToString();
        }
        _CurrencyValue.text = currencyStr;
    }

    public void ShowOwnCurrency(string itemDataID)
    {
        if (PlayerDataPack.IsMoney(itemDataID))
        {
            if (!_ShowOwnCurrency.Equals(itemDataID))
            {
                _ShowOwnCurrency = itemDataID;
            }
            ShowCurrency(itemDataID, PlayerDataPack.Instance.GetMoney(itemDataID));
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_REFRESH_MONEY, RefreshOwnCurrency);
        }
        else
        {

        }
    }

    private void RefreshOwnCurrency(object sender, Hashtable hash)
    {
        if (PlayerDataPack.IsMoney(_ShowOwnCurrency))
        {
            ShowCurrency(_ShowOwnCurrency, PlayerDataPack.Instance.GetMoney(_ShowOwnCurrency));
        }
    }

    #endregion

    public void OnBtnAddClick()
    {
        //if (PlayerDataPack.IsMoney(_ShowOwnCurrency))
        //{
        //    PlayerDataPack.Instance.AddMoney(_ShowOwnCurrency, 1000);
        //}

        UIMoneyLackTip.ShowAsyn(_ShowOwnCurrency);
    }
}


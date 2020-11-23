using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIMoneyLackTip : UIBase
{

    #region static funs

    public static void ShowAsyn(string moneyID)
    {
        Hashtable hash = new Hashtable();
        hash.Add("MoneyID", moneyID);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMoneyLackTip, UILayer.Sub2PopUI, hash);
    }

    #endregion

    #region 

    public UICurrencyItem _CurrencyOwn;

    public UICurrencyItem _Exchange1;
    public UINumInput _CostDiamond;

    public int _DefaultCost = 10;

    #endregion

    #region 

    private string _ExChangeMoneyID = "";

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        string lackMoney = (string)hash["MoneyID"];

        _ExChangeMoneyID = lackMoney;
        _CostDiamond.Init(_DefaultCost, 1, 999999);
        Refresh();
    }

    public void Refresh()
    {
        _CurrencyOwn.ShowOwnCurrency(_ExChangeMoneyID);

        int value = _CostDiamond.Value;
        _Exchange1.ShowCurrency(_ExChangeMoneyID, GameDataValue.DiamondExchange(_ExChangeMoneyID, value));
    }

    public void OnBtnExchange()
    {
        int costDiamondValue = _CostDiamond.Value;
        if (PlayerDataPack.Instance.DecMoney(PlayerDataPack.MoneyDiamond, costDiamondValue))
        {
            int addValue = GameDataValue.DiamondExchange(_ExChangeMoneyID, costDiamondValue);
            PlayerDataPack.Instance.AddMoney(_ExChangeMoneyID, addValue);
        }
        else
        {
            UIRechargePack.ShowAsyn();
        }
    }
    #endregion

    
    
}


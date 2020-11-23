using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemBuyPanel : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemBuyPanel, UILayer.SubPopUI, hash);
    }

    #endregion

    public UICurrencyItem _BuyCost1;
    public UICurrencyItem _BuyCost2;
    public UICurrencyItem _BuyCost3;
    public UICurrencyItem _BuyCost4;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _BuyCost1.ShowCurrency(PlayerDataPack.MoneyGemFrag, GemDataPack._BuyGemCost1);
        _BuyCost2.ShowCurrency(PlayerDataPack.MoneyGemFrag, GemDataPack._BuyGemCost1 * 5);
        _BuyCost3.ShowCurrency(PlayerDataPack.MoneyGemFrag, GemDataPack._BuyGemCost2);
        _BuyCost4.ShowCurrency(PlayerDataPack.MoneyGemFrag, GemDataPack._BuyGemCost2 * 5);
    }

    public override void Hide()
    {
        base.Hide();

        UIGemPack.Refresh();
    }

    public void BuyGemElementary1()
    {
        var gotItems = GemDataPack.Instance.BuyGemItem(0, 1);
        if (gotItems == null || gotItems.Count == 0)
            return;

        UIGemGetEffect.ShowAsyn(gotItems);
    }

    public void BuyGemElementary5()
    {
        var gotItems = GemDataPack.Instance.BuyGemItem(0, 5);
        if (gotItems == null || gotItems.Count == 0)
            return;

        UIGemGetEffect.ShowAsyn(gotItems);
    }

    public void BuyGemAdvanced1()
    {
        var gotItems = GemDataPack.Instance.BuyGemItem(1, 1);
        if (gotItems == null || gotItems.Count == 0)
            return;

        UIGemGetEffect.ShowAsyn(gotItems);
    }

    public void BuyGemAdvanced5()
    {
        var gotItems = GemDataPack.Instance.BuyGemItem(1, 5);
        if (gotItems == null || gotItems.Count == 0)
            return;

        UIGemGetEffect.ShowAsyn(gotItems);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIGemPack : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemPack, UILayer.PopUI, hash);
    }

    public static int GetShowTag()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return 0;

        if (!instance.isActiveAndEnabled)
            return 0;

        return instance._ShowTag;
    }

    public static void SetSelectGem(GemDataItem chooseItem)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.OnChooseGem(chooseItem);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    public UIContainerBase _GemContainer;
    public Button _BtnEquip;
    public Button _BtnLvUp;
    private int _ShowTag = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshItems();
    }

    private void RefreshItems()
    {
        List<GemDataItem> showGems = new List<GemDataItem>(GemDataPack.Instance._GemItems._PackItems);
        showGems.Sort((gemA, gemB) =>
        {
            if (gemA.GemRecord.Level > gemB.GemRecord.Level)
                return -1;
            else if (gemA.GemRecord.Level < gemB.GemRecord.Level)
                return 1;
            else
            {
                return 0;
            }
        });
        _GemContainer.InitContentItem(showGems);
    }

    private void OnChooseGem(GemDataItem chooseGem)
    {
        GemDataPack.Instance.SelectGem(chooseGem);
        _GemContainer.RefreshItems();
    }

    public void OnBtnBuy()
    {
        UIGemBuyPanel.ShowAsyn();
    }

    public void OnBtnCombine()
    {
        UIGemCombinePanel.ShowAsyn();
    }
}
